using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using AlogisticsWASM.Layout.Utilerias;
using AlogisticsWASM.Layout.Vacios.Contenedores;
using ALOGRepositorios.Services.Logisticos;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ALOGRespositorios.Modelos.Dtos.Control;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.ControlTower.OrdenesEnProceso
{
    public partial class GridServiciosCmp
    {

        [Parameter] public RenderFragment<PeticionesServicios> ChildContent { get; set; }
        [Parameter][EditorRequired] public UsuarioTokenDTO UsuarioTokenDTO { get; set; }
        [Parameter] public EventCallback<bool> ServicioEstaTerminado { get; set; }
        [Parameter] public EventCallback<bool> OnRecargarDatos { get; set; }
        [Parameter] public PeticionesContenedores Contenedor { get; set; }
        [Parameter] public Ordenes Orden { get; set; }

        [Inject] public IContenedorService ContenedorService { get; set; }
        [Inject] public TooltipService TooltipService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public IOrdenService OrdenService { get; set; }

        private RadzenDataGrid<PeticionesServicios> _gridServicios;
        private List<PeticionesServicios> _servicios;
        private List<CatReferenciaEstado> _estadosReferencia;
        private Ordenes _orden;
        private UtileriasPage _utileriasPage;
        private IRadzenFormComponent _editor;
        private bool _mostrarBotonModal;
        private RadzenButton _btnAgregarDocumentosRef;
        private bool _servicioTerminado;

        protected override void OnInitialized()
        {
            _servicios = new List<PeticionesServicios>();
            _utileriasPage = new UtileriasPage();
            _estadosReferencia = _utileriasPage.ObtenerEstadosReferencia();
        }

        protected override void OnParametersSet()
        {
            _servicios = Contenedor.Servicios.ToList();
            _orden = Orden;
            if (_gridServicios != null)
                _gridServicios.Reload();
        }

        private void OnRowRender(RowRenderEventArgs<PeticionesServicios> args)
        {
            args.Expandable = args.Data.Documentos?.Any() == true;
        }

        private async Task AgregarServicio()
        {

            CatLineaNegocioTariPrecio servicio = await MostrarModalServicios();

            if (servicio == null || servicio.catLineaNegocioTarifa == null) return;

            List<int> idsServicios = new List<int>();

            idsServicios = Contenedor.Servicios.Select(servicio => servicio.catServicios.IdCatServicio).ToList();

            var parametros = new Dictionary<string, object>()
            {
                ["AgregarServicio"] = true,
                ["IdReferencia"] = Contenedor.IdReferencia,
                ["IdContenedor"] = Contenedor.IdContenedor,
                ["Contenedor"] = Contenedor,
                ["IdsServiciosContenedor"] = idsServicios,
                ["Servicio"] = servicio,
                ["Orden"] = Orden
            };
            // Se abre el modal
            var resultado = await DialogService.OpenAsync<FormularioContenedorCmp>(
                title: "Agregar servicio",
                parameters: parametros,
                options: new DialogOptions
                {
                    Width = "1000px",
                    Height = "612px",
                    CloseDialogOnOverlayClick = true
                }
            );

            if (resultado is bool recargarDatos)
            {
                if (recargarDatos)
                {
                    await OnRecargarDatos.InvokeAsync(true);
                }
            }
        }

        private async Task<CatLineaNegocioTariPrecio> MostrarModalServicios()
        {

            CatLineaNegocioTariPrecio servicio = new CatLineaNegocioTariPrecio();

            List<int> idsServicios = new List<int>();

            idsServicios = Contenedor.Servicios.Select(servicio => servicio.catServicios.IdCatServicio).ToList();

            var parametros = new Dictionary<string, object>()
            {
                ["Agregarservicio"] = false,
                ["IdsServiciosContenedor"] = idsServicios,
                ["Contenedor"] = Contenedor.Contenedor,
                ["AduanaNombre"] = Contenedor.Aduana,
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

        private async Task MostrarModalAsiganacionDocumentoAsync(PeticionesServicios servicio)
        {

            SolCargarArchivoWASMDTO documento = new SolCargarArchivoWASMDTO();

            documento.IdOrden = Orden.IdOrden;
            documento.IdReferencia = (int)Orden.peticionesReferencias?.FirstOrDefault()?.IdReferencia;
            documento.IdContenedor = servicio.IdContenedor;
            documento.IdServicio = servicio.IdServicio;
            documento.IdCatLineaNegocio = 1;

            var parametros = new Dictionary<string, object>()
            {
                ["Documento"] = documento,
                ["IdCatServicio"] = servicio.catServicios.IdCatServicio
            };

            var resultado = await DialogService.OpenAsync<ModalAsignarDocumentoServicioCmp>(
                    title: "Asignación de documentación",
                    parameters: parametros,
                    options: new DialogOptions
                    {
                        Width = "820px",
                        Height = "260px",
                        CloseDialogOnOverlayClick = false,
                        ShowClose = true // Oculta la equis del modal
                    }
                );


            // Verifica si se devolvió algo y cámbialo a tu tipo
            if (resultado is bool respuesta)
            {
                if (respuesta)
                {
                    await OnRecargarDatos.InvokeAsync(true);
                    await _gridServicios.Reload();
                }
            }
            //return servicio;

        }

        private async Task CambiarEstadoServicioAsync(PeticionesServicios servicio)
        {
            SolCambioEstadoDTO estado = new SolCambioEstadoDTO();

            estado.IdReferencia = Contenedor.IdReferencia;
            estado.IdCatReferenciaEstado = servicio.IdEstadoServicio;
            estado.IdContenedor = Contenedor.IdContenedor;
            estado.IdServicio = servicio.IdServicio;
            await ContenedorService.ActualizaEstadoServicio(estado);

        }

        private async Task EditarFila(PeticionesServicios servicio)
        {

            if (servicio.IdEstadoServicio == 5 || servicio.IdEstadoServicio == 6)
                return;
            await _gridServicios.EditRow(servicio);
        }

        private async Task OnUpdateRow(PeticionesServicios servicio)
        {
            // Actualiza el estado en el backend
            await CambiarEstadoServicioAsync(servicio);
            //await _gridServicios.CloseEditRow(servicio);
        }

        private async Task OnCancelRow(PeticionesServicios servicio)
        {
            //await _gridServicios.CancelEditRow(servicio);
        }

        private void Guardar(PeticionesServicios servicio)
        {
        }

        private async Task OnDropdownChange(object valor, PeticionesServicios servicio)
        {
            if (valor is int nuevoEstado)
            {
                servicio.IdEstadoServicio = nuevoEstado;
                await CambiarEstadoServicioAsync(servicio);
                //await CambiarEstadoServicioAsync(servicio);
                await _gridServicios.UpdateRow(servicio); // Esto dispara RowUpdate
            }
        }


        private async Task OnEstadoActualizado(PeticionesServicios servicio)
        {
            await CambiarEstadoServicioAsync(servicio);
            await _gridServicios.UpdateRow(servicio); // Esto dispara RowUpdate si lo deseas
        }

        private void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Hello!", options);

        private bool NoPermiteDocumentos(PeticionesServicios servicio)
        {
            return (
                        servicio.catServicios.IdCatServicio != 1 &&
                        servicio.catServicios.IdCatServicio != 2 &&
                        servicio.catServicios.IdCatServicio != 4
                    ) || (servicio.IdEstadoServicio == 5);
        }
        private void MostrarToolTip(ElementReference elementReference, PeticionesServicios servicio)
        {
            if (NoPermiteDocumentos(servicio))
            {
                var options = new TooltipOptions
                {
                    Position = TooltipPosition.Top,
                    Duration = 3000,
                    Style = "background-color: darkred; color: white"
                };
                string mensaje = servicio.IdEstadoServicio switch
                {
                    4 => "Este tipo de servicio no permite documentos",
                    5 => "El servicio ya se encuentra terminado.",
                    _ => ""
                };
                TooltipService.Open(elementReference, mensaje, options);
            }
        }

        private async Task ServicioEstaTerminadoAsync(bool servicioTerminado)
        {
            _servicioTerminado = servicioTerminado;
            await ServicioEstaTerminado.InvokeAsync(_servicioTerminado);
        }

        private bool ContenedorTieneServiciosTerminados()
        {
            return Contenedor.Servicios.All(servicio => servicio.IdEstadoServicio == 5);
        }
    }
}
