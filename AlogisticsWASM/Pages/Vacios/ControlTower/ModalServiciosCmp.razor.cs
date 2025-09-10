using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Utilerias;
using ALOG.Modelos.Modelos.DTO.Vacios;
using ALOG.Modelos.Modelos.Orden;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Control.IControl;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.ControlTower
{
    public partial class ModalServiciosCmp
    {

        [Parameter] public int IdReferencia { get; set; }
        [Parameter] public int IdContenedor { get; set; }
        [Parameter] public bool Agregarservicio { get; set; }
        [Parameter] public string Contenedor { get; set; }
        [Parameter] public string AduanaNombre { get; set; }
        [Parameter] public Ordenes Orden { get; set; }
        [Parameter] public List<int> IdsServiciosContenedor { get; set; }
        [Inject] private ICatServicioService ServicioService { get; set; }
        [Inject] private IOrdenService OrdenServicio { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] public NotificationService NotificationService { get; set; }
        [Inject] public IControlService ControlService { get; set; }

        private RadzenDataGrid<CatLineaNegocioTariPrecio> _gridServicios;
        private List<CatLineaNegocioTariPrecio> _servicios;
        private IList<CatLineaNegocioTariPrecio> _serviciosSeleccionados;
        private PeticionesServiciosClienteExternoDTO _servicio;
        private List<CatLineaNegocioTariPrecio> _tarifarioServicios;
        private List<CatLineaNegocioTariPrecio> _tarifarioServiciosOriginal;

        DataGridEditMode editMode = DataGridEditMode.Single;

        List<CatLineaNegocioTariPrecio> servicioToInsert = new List<CatLineaNegocioTariPrecio>();
        List<CatLineaNegocioTariPrecio> servicioToUpdate = new List<CatLineaNegocioTariPrecio>();
        bool allowRowSelectOnRowClick = true;
        bool _habilitarBotonEdicion;
        private FiltroTarifarioServicioDTO _filtro;
        private string _nombreFiltro;
        private double _precioOriginal;

        public class Servicio
        {

            public int IdServicio { get; set; }
            public string Nombre { get; set; }
            public double Subtotal { get; set; }

        }

        protected override async Task OnInitializedAsync()
        {
            _servicio = new PeticionesServiciosClienteExternoDTO();
            _serviciosSeleccionados = new List<CatLineaNegocioTariPrecio>();
            _habilitarBotonEdicion = false;
            _filtro = new FiltroTarifarioServicioDTO();
            _tarifarioServicios = new List<CatLineaNegocioTariPrecio>();
            _tarifarioServiciosOriginal = new List<CatLineaNegocioTariPrecio>();
        }

        protected override async Task OnParametersSetAsync()
        {

            if (Orden != null)
            {
                _filtro.IdCatEmpresa = 2;
                _filtro.IdCatAduana = Orden.catAduana.IdCatAduana;
                _filtro.IdLineaNegocio = 1;
                var filtroEncriptado = await ControlService.GetFiltroCifrado(_filtro);
                _tarifarioServicios = await ServicioService.ObtenerTarifarioServicios(filtroEncriptado);
                _tarifarioServiciosOriginal = _tarifarioServicios;
            }

        }

        private bool EsServicioCotizacion(CatLineaNegocioTariPrecio servicio)
        {

            CatServicios catServicio = servicio.catLineaNegocioTarifa.catServicios;

            return catServicio.IdCatServicio == 1 ||
                   catServicio.IdCatServicio == 2 ||
                   catServicio.IdCatServicio == 3 ||
                   catServicio.IdCatServicio == 4;

        }

        private bool ServicioEsValido()
        {
            var servicio = _serviciosSeleccionados?.FirstOrDefault();
            if (servicio == null)
            {
                MostrarNotificacion(
                        "¡Advertencia!",
                        "Debes seleccionar un servicio para poder continuar",
                        NotificationSeverity.Warning,
                        4000
                    );
                return false;
            }

            IdsServiciosContenedor = IdsServiciosContenedor ?? new List<int>();
            CatServicios catServicio = servicio.catLineaNegocioTarifa.catServicios;
            if (catServicio.IdCatServicio != 1 && !IdsServiciosContenedor.Contains(1))
            {
                if (catServicio.IdCatServicio != 2)
                {
                    MostrarNotificacion(
                        "¡Advertencia!",
                        $"No se puede asignar el servicio {catServicio.Nombre} al contenedor {Contenedor}. Primero se debe asignar el servicio de MANIOBRAS DE VACIO para continuar.",
                        NotificationSeverity.Warning,
                        0
                    );
                    return false;
                }
            }

            if (IdsServiciosContenedor.Contains(catServicio.IdCatServicio))
            {
                MostrarNotificacion(
                    "¡Advertencia!",
                    $"El servicio {catServicio.Nombre} ya se encuentra asignado al contenedor {Contenedor}",
                    NotificationSeverity.Warning,
                    0
                );
                return false;
            }
            // Si el servicio a agregar es el de maniobras de vacíos
            if (catServicio.IdCatServicio == 1)
            {
                // Pregunta si el contenedor tiene asignado el servicio de gestión de eir
                if (IdsServiciosContenedor.Contains(2))
                {
                    MostrarNotificacion(
                        "¡Advertencia!",
                        $"No se puede asignar el servicio {catServicio.Nombre}, ya que el contenedor {Contenedor} ya cuenta con el servicio de GESTION ELECTRONICA EIR CONTENEDOR VACIO",
                        NotificationSeverity.Warning,
                        0
                    );
                    return false;
                }
            }
            // Si el servicio a agregar es el de gestión de eir
            if (catServicio.IdCatServicio == 2)
            {
                // Pregunta si el contenedor tiene asignado el servicio de maniobras de vacíos
                if (IdsServiciosContenedor.Contains(1))
                {
                    MostrarNotificacion(
                        "¡Advertencia!",
                        $"No se puede asignar el servicio {catServicio.Nombre}, ya que el contenedor {Contenedor} ya cuenta con el servicio de MANIOBRAS DE VACIO",
                        NotificationSeverity.Warning,
                        0
                    );
                    return false;
                }
            }

            return true;
        }

        public async Task AgregarServicioContenedorAsync()
        {

            if (IdReferencia > 0 && IdContenedor > 0 && Agregarservicio)
            {
                var servicioSeleccionado = _serviciosSeleccionados.FirstOrDefault();
                CatServicios catServicioSeleccionado = servicioSeleccionado.catLineaNegocioTarifa.catServicios;
                if (IdsServiciosContenedor.Contains(catServicioSeleccionado.IdCatServicio))
                {
                    return;
                }

                _servicio.IdCatServicio = catServicioSeleccionado.IdCatServicio;

                await OrdenServicio.AgregarServicioAContenedor(IdReferencia, IdContenedor, _servicio);
            }
        }

        private void Aceptar()
        {
            if (ServicioEsValido())
                DialogService.Close(_serviciosSeleccionados);
        }

        private void Cancelar()
        {
            DialogService.Close(null);
        }

        void Reset()
        {
            servicioToInsert.Clear();
            servicioToUpdate.Clear();
            _precioOriginal = 0;
        }

        void Reset(CatLineaNegocioTariPrecio servicio)
        {
            servicioToInsert.Remove(servicio);
            servicioToUpdate.Remove(servicio);
            _precioOriginal = 0;
        }

        async Task EditRow(CatLineaNegocioTariPrecio servicio)
        {
            if (!_gridServicios.IsValid) return;

            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }
            _precioOriginal = servicio.Precio;
            servicioToUpdate.Add(servicio);
            await _gridServicios.EditRow(servicio);
        }

        void OnUpdateRow(CatLineaNegocioTariPrecio servicio)
        {
            Reset(servicio);
        }

        async Task SaveRow(CatLineaNegocioTariPrecio servicio)
        {
            if (_precioOriginal > servicio.Precio)
            {
                IJsHelper.MostrarNotificacion(
                    NotificationService,
                    "¡Advertencia!",
                    "El subtotal ingresado no puede ser inferior al subtotal del tarifario",
                    NotificationSeverity.Warning,
                    0
                );
                servicio.Precio = _precioOriginal;
                CancelEdit(servicio);
                return;
            }

            if (servicio.Precio <= 0)
            {
                IJsHelper.MostrarNotificacion(
                        NotificationService,
                        "¡Advertencia!",
                        "El subtotal no puedo ser menor o igual a cero",
                        NotificationSeverity.Warning,
                        0
                    );
                servicio.Precio = _precioOriginal;
                CancelEdit(servicio);
                return;
            }

            await _gridServicios.UpdateRow(servicio);
        }

        void CancelEdit(CatLineaNegocioTariPrecio servicio)
        {
            Reset(servicio);

            _gridServicios.CancelEditRow(servicio);

            //var orderEntry = dbContext.Entry(order);
            //if (orderEntry.State == EntityState.Modified) {
            //    orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
            //    orderEntry.State = EntityState.Unchanged;
            //}
        }

        private void MostrarNotificacion(string titulo, string mensaje, NotificationSeverity notificationSeverity, int duration)
        {

            NotificationService.Notify(new NotificationMessage
            {
                Severity = notificationSeverity,
                Summary = titulo,
                Detail = mensaje,
                Duration = duration == 0 ? 4000 : duration
            });

        }

        private void FiltrarNombre()
        {
            _tarifarioServicios = _tarifarioServiciosOriginal
                .Where(s => s.catLineaNegocioTarifa.catServicios.Nombre
                .Contains(_nombreFiltro, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

    }
}
