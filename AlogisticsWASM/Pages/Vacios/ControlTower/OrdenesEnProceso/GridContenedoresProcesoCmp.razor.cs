using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using AlogisticsWASM.Layout.Vacios.Contenedores;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.ControlTower.OrdenesEnProceso
{
    public partial class GridContenedoresProcesoCmp
    {

        [Parameter] public RenderFragment<PeticionesContenedores> ChildContent { get; set; }
        [Parameter][EditorRequired] public UsuarioTokenDTO UsuarioTokenDTO { get; set; }
        [Parameter] public List<PeticionesContenedores> Contenedores { get; set; }
        [Parameter] public EventCallback<bool> OnRecargarDatos { get; set; }
        [Parameter] public EventCallback<PeticionesContenedores> OnContenedorExpandido { get; set; }
        [Parameter] public FiltroOrdenesReferenciasDTO Filtro { get; set; }
        [Parameter] public Ordenes Orden { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private IReferenciaService ReferenciaService { get; set; }
        [Inject] public IContenedorService ContenedorService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }

        private RadzenDataGrid<PeticionesContenedores> _gridContenedores;
        private List<PeticionesContenedores> _contenedores;
        private bool _allowRowSelectOnRowClick = true;
        private List<string> _errores;
        private bool _cargando;
        private PeticionesContenedores? _contenedorExpandido;
        private int? _idContenedorExpandido;

        DataGridEditMode editMode = DataGridEditMode.Single;
        List<PeticionesContenedores> ordersToInsert = new List<PeticionesContenedores>();
        List<PeticionesContenedores> _contenedoresActualizar = new List<PeticionesContenedores>();

        protected override async Task OnInitializedAsync()
        {
            _errores = new List<string>();
            _contenedores = new List<PeticionesContenedores>();
            //await GetContenedoresEnProceso();
        }

        protected override async Task OnParametersSetAsync()
        {
            //_contenedores = Orden.peticionesReferencias.FirstOrDefault().Contenedores.ToList();
            _contenedores = Contenedores;
            if (_gridContenedores != null)
            {
                await InvokeAsync(async () =>
                {
                    await _gridContenedores.Reload();
                });
            }
        }

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


        private void OnRowRender(RowRenderEventArgs<PeticionesContenedores> args)
        {
            args.Expandable = args.Data.Servicios?.Any() == true;
            //await _gridContenedores.ExpandRow(args.Data);
            //_idContenedorExpandido = args.Data.IdContenedor;
        }

        void Reset()
        {
            ordersToInsert.Clear();
            _contenedoresActualizar.Clear();
        }

        void Reset(PeticionesContenedores order)
        {
            ordersToInsert.Remove(order);
            _contenedoresActualizar.Remove(order);
        }

        async Task EditRow(PeticionesContenedores order)
        {
            if (!_gridContenedores.IsValid) return;

            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            _contenedoresActualizar.Add(order);
            await _gridContenedores.EditRow(order);
        }

        void OnUpdateRow(PeticionesContenedores order)
        {
            Reset(order);

            //dbContext.Update(order);

            //dbContext.SaveChanges();
        }

        async Task SaveRow(PeticionesContenedores contenedor)
        {
            await ActualizarFolioManiobraAsync(contenedor);
            Reset();
            await _gridContenedores.UpdateRow(contenedor);
        }

        void CancelEdit(PeticionesContenedores order)
        {
            Reset(order);

            _gridContenedores.CancelEditRow(order);

            //var orderEntry = dbContext.Entry(order);
            //if (orderEntry.State == EntityState.Modified) {
            //    orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
            //    orderEntry.State = EntityState.Unchanged;
            //}
        }

        async Task DeleteRow(PeticionesContenedores order)
        {
            Reset(order);

            if (_contenedores.Contains(order))
            {
                //dbContext.Remove<PeticionesContenedores>(order);

                //dbContext.SaveChanges();

                await _gridContenedores.Reload();
            }
            else
            {
                _gridContenedores.CancelEditRow(order);
                await _gridContenedores.Reload();
            }
        }

        async Task InsertRow()
        {

            if (Orden != null)
            {
                CatLineaNegocioTariPrecio servicio = await MostrarModalServicios();
                if (servicio == null || servicio.catLineaNegocioTarifa == null) return;

                if (servicio.catLineaNegocioTarifa.catServicios.IdCatServicio == 0)
                {
                    MostrarNotificacion(
                        "¡Advertencia!",
                        "Debes seleccionar un servicio para poder continuar",
                        NotificationSeverity.Warning,
                        4000
                    );
                    return;
                }
                await AbrirModalAgregarContenedor(servicio);

            }
            //if (!_gridContenedores.IsValid) return;

            //if (editMode == DataGridEditMode.Single) {
            //    Reset();
            //}

            //var contenedor = new PeticionesContenedores();
            //ordersToInsert.Add(contenedor);
            //await _gridContenedores.InsertRow(contenedor);
        }

        async Task InsertAfterRow(PeticionesContenedores row)
        {
            if (!_gridContenedores.IsValid) return;

            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            var contenedor = new PeticionesContenedores();
            ordersToInsert.Add(contenedor);
            await _gridContenedores.InsertAfterRow(contenedor, row);
        }

        void OnCreateRow(PeticionesContenedores order)
        {
            //dbContext.Add(order);

            //dbContext.SaveChanges();

            ordersToInsert.Remove(order);
        }


        private async Task AbrirModalDocumentosServicios(PeticionesContenedores contenedor)
        {
            // Se prepara el parámetro que se enviará al componente modal:
            // ["Contenedor"]. Es la variable Parameter que se encuentra en el modal
            // contenedor. Es el objeto que se enviará al modal
            var parametros = new Dictionary<string, object>()
            {
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
        }

        private async Task AbrirModalAgregarContenedor(CatLineaNegocioTariPrecio servicio)
        {

            var parametros = new Dictionary<string, object>()
            {
                ["AgregarServicio"] = false,
                ["IdReferencia"] = Orden.peticionesReferencias.FirstOrDefault().IdReferencia,
                ["Servicio"] = servicio,
                ["Orden"] = Orden
            };
            // Se abre el modal
            var resultado = await DialogService.OpenAsync<FormularioContenedorCmp>(
                title: "Agregar contenedor",
                parameters: parametros,
                options: new DialogOptions
                {
                    Width = "1000px",
                    Height = "700px",
                    CloseDialogOnOverlayClick = true
                }
            );

            if (resultado is bool recargarDatos)
            {
                if (recargarDatos)
                {
                    await OnContenedorExpandido.InvokeAsync();
                    await OnRecargarDatos.InvokeAsync(recargarDatos);
                }
            }
        }

        private async Task<CatLineaNegocioTariPrecio> MostrarModalServicios()
        {

            CatLineaNegocioTariPrecio servicio = new CatLineaNegocioTariPrecio();

            var parametros = new Dictionary<string, object>()
            {
                ["Agregarservicio"] = false,
                ["AduanaNombre"] = Orden.catAduana.Nombre,
                ["Orden"] = Orden
            };

            var resultado = await DialogService.OpenAsync<ModalServiciosCmp>(
                    title: "Asignación de servicios",
                    parameters: parametros,
                    options: new DialogOptions
                    {
                        Width = "800px",
                        Height = "660px",
                        CloseDialogOnOverlayClick = false,
                        ShowClose = true // Oculta la equis del modal
                    }
                );


            // Verifica si se devolvió algo y cámbialo a tu tipo
            if (resultado is IList<CatLineaNegocioTariPrecio> serviciosSeleccionados)
            {
                // Aquí puedes trabajar con la lista seleccionada
                servicio = serviciosSeleccionados.FirstOrDefault();
            }
            return servicio;
        }

        private async Task<bool> ActualizarFolioManiobraAsync(PeticionesContenedores contenedor)
        {
            var cont = _contenedoresActualizar.First();
            SolActualizarFolioManiobraDTO actualizarFolio = new SolActualizarFolioManiobraDTO();
            actualizarFolio.FolioManiobra = cont.FolioManiobra;
            actualizarFolio.IdContenedor = cont.IdContenedor;
            actualizarFolio.IdReferencia = cont.IdReferencia;

            var respuesta = await ContenedorService.ActualizaFolioContenedor(actualizarFolio);

            if (respuesta)
            {
                MostrarNotificacion(
                    "Folio de maniobra actualizado con éxito",
                    $"Se asignó el folio de maniobra al contenedor {contenedor.Contenedor} de manera éxitosa",
                    NotificationSeverity.Success,
                    8000
                );
            }
            else
            {
                MostrarNotificacion(
                        "Error al actualizar folio de maniobras",
                        $"ocurrió un error inesperado al actualizar el folio de maniobras al contenedor {contenedor.Contenedor}",
                        NotificationSeverity.Success,
                        8000
                    );
            }
            return respuesta;
        }
    }
}
