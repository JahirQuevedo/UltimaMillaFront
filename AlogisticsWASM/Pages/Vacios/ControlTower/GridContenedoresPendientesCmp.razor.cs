using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Utilerias;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using AlogisticsWASM.Layout.Utilerias;
using AlogisticsWASM.Layout.Vacios.Patios;
using ALOGRepositorios.Services.Control.IControl;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.ControlTower
{

    public partial class GridContenedoresPendientesCmp
    {

        [Parameter] public EventCallback<int> OnContenedoresSeleccionadosChanged { get; set; }
        [Parameter] public EventCallback<int> IdOrdenCancelar { get; set; }
        [Parameter] public EventCallback<bool> OnRecargarDatos { get; set; }
        private EventCallback<IList<PeticionesContenedores>> _onValueChanged;
        [Parameter] public List<PeticionesContenedores> Contenedores { get; set; }
        [Parameter] public Ordenes Orden { get; set; }
        [Parameter] public bool EstaCargando { get; set; }
        [Parameter] public IList<Ordenes> Ordenes { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] public IContenedorService ContenedorService { get; set; }
        [Inject] public IOrdenService OrdenService { get; set; }
        [Inject] public IControlService ControlService { get; set; }

        private List<PeticionesContenedores> _contenedores;
        private List<string> _errores;
        public IList<PeticionesContenedores> _contenedoresSeleccionados;
        private CatPatios _patioSeleccionado;
        private List<Ordenes> _ordenes;

        private RadzenDataGrid<PeticionesContenedores> _gridContenedores;

        private bool _allowRowSelectOnRowClick = true;
        private bool _existenContenedoresPendientes;

        private FiltrosCancelacionElementoDTO _filtroCancelacion;

        protected override void OnInitialized()
        {
            _filtroCancelacion = new FiltrosCancelacionElementoDTO();
            _errores = new List<string>();
            _contenedoresSeleccionados = new List<PeticionesContenedores>();
            _onValueChanged = EventCallback.Factory.Create<IList<PeticionesContenedores>>(this, OnValueChanged);
        }

        protected override void OnParametersSet()
        {

            if (_ordenes != null) _ordenes.Clear();

            if (_contenedores != null) _contenedores.Clear();

            _contenedores = Contenedores;
            _ordenes = Ordenes.ToList();

            foreach (var contenedor in _contenedores)
            {
                foreach (var servicio in contenedor.Servicios)
                {
                    if (servicio.catServicios != null)
                    {
                        servicio.DescServicio = servicio.catServicios.Nombre;
                    }
                }
            }
        }

        #region Métodos para la asignación de patios a contenedores
        public async Task AsignarPatio()
        {

            if (_contenedoresSeleccionados.Count() == 0)
            {
                MostrarNotificacion(
                    "¡Advertencia!",
                    "Debes seleccionar al menos un contenedor para asignar un patio",
                    NotificationSeverity.Warning,
                    0
                );
                return;
            }

            _errores.Clear();

            await MostrarModalPatios();

            if (_patioSeleccionado == null)
            {
                return;
            }

            foreach (var cont in _contenedoresSeleccionados)
            {
                cont.PatioId = _patioSeleccionado.IdCatPatios;
                cont.Patio_RFC = string.Empty;
                cont.Patio_RazonSocial = _patioSeleccionado.RazonSocial;

                var respuestaPatio = await ContenedorService.AsignarPatio(cont);

                if (respuestaPatio.IsSuccess == false || respuestaPatio.StatusCode != System.Net.HttpStatusCode.OK || respuestaPatio.lstrErrorMessages.Count() > 0)
                {
                    _errores.AddRange(respuestaPatio.lstrErrorMessages);
                    //_errores.Add($"Ocurrió un error inesperado al asignar el patio {_patioSeleccionado.RazonSocial} al contenedor {cont.Contenedor}. Por favor, intenta nuevamente");
                    cont.PatioId = 0;
                    cont.Patio_RFC = null;
                    cont.Patio_RazonSocial = null;
                }
            }

            if (ExistenErrores("Error al asignar patio a contenedores") == false)
            {
                MostrarNotificacion(
                    "Proceso finalizado con éxito",
                    "",
                    NotificationSeverity.Success,
                    4000
                );
            }
            _contenedoresSeleccionados.Clear();

            await OnContenedoresSeleccionadosChanged.InvokeAsync(_contenedoresSeleccionados.Count);
            await _gridContenedores.Reload();
        }

        private async Task MostrarModalPatios()
        {

            PeticionesContenedores contenedor = _contenedoresSeleccionados.FirstOrDefault();
            var parametros = new Dictionary<string, object>()
            {
                ["AduanaNombre"] = contenedor.Aduana
            };

            var resultado = await DialogService.OpenAsync<AsignarPatioCmp>(
                title: $"Catálogo de patios para la aduana de {contenedor.Aduana}",
                parameters: parametros,
                options: new DialogOptions
                {
                    Width = "800px",
                    Height = "600px",
                    CloseDialogOnOverlayClick = false,
                    ShowClose = true
                }
            );

            if (resultado is CatPatios patio)
            {
                _patioSeleccionado = patio;
            }
        }
        #endregion Métodos para la asignación de patios a contenedores

        #region Método para cancelar un contenedor con su respectivo comentario
        private async Task CancelarContenedor(PeticionesContenedores contenedor)
        {

            var result = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "¡Advertencia!",
                Text = $"¿Está seguro de cancelar el contenedor {contenedor.Contenedor}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí",
                CancelButtonText = "No"
            });

            if (string.IsNullOrEmpty(result.Value))
            {
                return;
            }

            _filtroCancelacion.IdContenedor = contenedor.IdContenedor;
            _filtroCancelacion.IdOrden = Orden.IdOrden;

            string filtroCifrado = await ControlService.GetFiltroCifrado(_filtroCancelacion);
            var respuestaCancelacion = await OrdenService.CancelarElemento(filtroCifrado);
            var orden = await OrdenService.GetOrden(Orden.IdOrden);

            _existenContenedoresPendientes = orden.peticionesReferencias.FirstOrDefault().Contenedores.Any(c => c.IdEstadoContenedor == 7);
            if (!_existenContenedoresPendientes) await IdOrdenCancelar.InvokeAsync(orden.IdOrden);

            if (_contenedores.Contains(contenedor))
            {
                _contenedores.Remove(contenedor);
                orden.peticionesReferencias.FirstOrDefault().Contenedores = _contenedores;
                await OnRecargarDatos.InvokeAsync(true);
                await _gridContenedores.Reload();
            }
        }

        private async Task<string> MostrarModalComentarios()
        {
            var resultado = await DialogService.OpenAsync<AgregarComentariosCmp>(
                title: "Agregar comentario de cancelación",
                parameters: null,
                options: new DialogOptions
                {
                    Width = "800px",
                    Height = "450px",
                    CloseDialogOnOverlayClick = false
                }
            );
            return resultado;
        }

        private void Reset(PeticionesContenedores contenedor)
        {
            _contenedores.Remove(contenedor);
        }
        #endregion Método para cancelar un contenedor

        #region Métodos para controlar el cambio de los checks y notificarlo al componente padre
        private async Task OnValueChanged(IList<PeticionesContenedores> seleccionados)
        {
            _contenedoresSeleccionados = seleccionados;
            await OnContenedoresSeleccionadosChanged.InvokeAsync(_contenedoresSeleccionados.Count);
        }

        private async Task OnSelectAllChanged(bool? isChecked)
        {
            _contenedoresSeleccionados = isChecked == true ? _contenedores.ToList() : new List<PeticionesContenedores>();
            await OnContenedoresSeleccionadosChanged.InvokeAsync(_contenedoresSeleccionados.Count);
        }
        #endregion Métodos para controlar el cambio de los checks y notificarlo al componente padre

        #region Métodos para iniciar el proceso operativo de los contenedores
        public async Task IniciarProceso(IList<Ordenes> ordenes)
        {
            _ordenes = ordenes.ToList();
            // Valida que cada contenedor de una orden seleccionada, tengo su respectivo patio asignado
            // En caso que un solo contenedor no tenga un patio asignado, no se puede dar inicio a la operación
            // Es decir, toda la orden queda con estatus Pendiente
            if (ContenedoresValidos())
            {
                // Realizar las siguientes transacciones a la base de datos
                // Actualizar contenedores con el patio asignado
                // Actualizar estatus de contenedores a En proceso
                _errores.Clear();
                var ids = _ordenes.Select(o => o.IdOrden).Distinct().ToList();
                //var ids = _ordenes.Select(o => o.IdOrden).ToList();

                //List<int> idsOrdenes = new List<int>();

                //foreach (var id in ids) if (!idsOrdenes.Contains(id)) idsOrdenes.Add(id);

                var respuesta = await OrdenService.IniciarProceso(ids);
                _errores = respuesta.ErrorMessages;

                if (ExistenErrores("Error al inicar proceso"))
                    return;

                MostrarNotificacion(
                    "Operación éxitosa",
                    $"",
                    NotificationSeverity.Success,
                    4000
                );
            }

        }

        private bool ContenedoresValidos()
        {
            if (_ordenes.Count() == 0)
            {
                MostrarNotificacion(
                    "¡Advertencia!",
                    "Debes marcar al menos una orden para dar inicio al proceso operativo de los contenedores",
                    NotificationSeverity.Warning,
                    0
                );
                return false;
            }

            foreach (var orden in _ordenes)
            {
                foreach (var referencia in orden.peticionesReferencias)
                {
                    foreach (var contenedor in referencia.Contenedores)
                    {
                        if (contenedor.PatioId == 0 || string.IsNullOrEmpty(contenedor.Patio_RazonSocial) && contenedor.IdEstadoContenedor == 7)
                        {
                            _errores.Add($"El contenedor {contenedor.Contenedor} no tiene asignado un patio. No se puede continuar");
                        }
                    }
                }
            }


            if (ExistenErrores("No se puede iniciar el proceso operativo"))
                return false;

            return true;
        }
        #endregion Métodos para iniciar el proceso operativo de los contenedores

        #region Métodos de utilidad

        private bool ExistenErrores(string tituloError)
        {
            if (_errores.Count() > 0)
            {
                string mensajesErrores = string.Join("<br />", _errores);
                MostrarNotificacion(
                    tituloError,
                    mensajesErrores,
                    NotificationSeverity.Error,
                    8000
                );
                _errores.Clear();
                return true;
            }
            return false;
        }

        private void MostrarNotificacion(string titulo, string mensaje, NotificationSeverity notificationSeverity, int duracion)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = notificationSeverity,
                Summary = titulo,
                Detail = mensaje,
                Duration = duracion == 0 ? 4000 : duracion,
            });
        }
        #endregion Métodos de utilidad
        private async Task AbrirModalDocumentosServicios(PeticionesContenedores contenedor)
        {
            // Se prepara el parámetro que se enviará al componente modal:
            // ["Contenedor"]. Es la variable Parameter que se encuentra en el modal
            // contenedor. Es el objeto que se enviará al modal
            var parametros = new Dictionary<string, object>()
            {
                ["Orden"] = Orden,
                ["Contenedor"] = contenedor
            };
            // Se abre el modal
            var resultado = await DialogService.OpenAsync<ModalDocumentacionServicioCmp>(
                title: "Expediente digital",
                parameters: parametros,
                options: new DialogOptions
                {
                    Width = "1200px",
                    Height = "600px",
                    CloseDialogOnOverlayClick = false
                }
            );

            if (resultado is bool existenServiciosPendientes)
            {
                if (existenServiciosPendientes)
                {
                    if (_contenedores.Contains(contenedor))
                    {
                        _contenedores.Remove(contenedor);
                        await _gridContenedores.Reload();
                    }
                }
            }

        }
    }

}
