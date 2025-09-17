using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using AlogisticsWASM.Layout.Utilerias;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaPageRZ
    {
        #region SERVICIOS
        [Inject] private IAcarreosService _acarreoService { get; set; }
        [Inject] SweetAlertService SweetAlertService { get; set; }
        [Inject] private IUltimaMillaEncabezadoService _encabezadoService { get; set; }
        [Inject] private ISLOSolicitudesService _solicitudesService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private ILoginService loginService { get; set; }
        [Inject] ICatTipoEstadoService CatTipoEstadoService { get; set; }
        [Inject] ICatAduanaService CatAduanaService { get; set; }
        [Inject] ICatClientesService ClientesService { get; set; }
        [Inject] ISLOTControlTerrestreService SLOTControlTerrestreService { get; set; }
        [Inject] ISLOSolicitudesDetalleService solicitudesDetalleService { get; set; }
        [Inject] IJSRuntime JS { get; set; }



        //[Inject] public IOrdenService OrdenService { get; set; }
        [Inject] ILoginService _loginService { get; set; }
        #endregion

        #region Variables
        private int pageSize = 10; // Valor inicial
        private ICollection<DtUltimaMillaEnc> _encabezados;
        private IEnumerable<DtUltimaMillaEnc> _IEencabezados;
        private ODataEnumerable<DtUltimaMillaEnc> _ODencabezados;
        //private UltimaMillaEncabezadoEditarDTO? _encabezadoSeleccionado;
        private ICollection<DtAcarreos>? _acarreos;
        private int _ordenesCompletadas = 0;
        private int _ordenesCanceladas = 0;
        private int _ordenesEnProceso = 0;
        private List<DtUltimaMillaEnc> _lstUltimaMillaEnc;
        RadzenDataGrid<DtUltimaMillaEnc> grid;
        //private int pageSize = 10;
        private int currentPage = 1;
        private int totalPages;
        private string _tipoEstado = string.Empty;
        private string ClienteFiltrar = string.Empty;
        private FiltroDtUltimaMillaDTO _filtro;
        int position = 1;
        private UtileriasPage objutileriasPage = new UtileriasPage();
        LogicalFilterOperator logicalFilterOperator = LogicalFilterOperator.And;
        FilterCaseSensitivity filterCaseSensitivity = FilterCaseSensitivity.CaseInsensitive;
        private DateTime dFSolicitudIni;
        private DateTime dFSolicitudFin;
        private DateTime dFSolicitudCierre;
        private string strPNumPart;
        private int intPviaje;
        private string strPcliente;
        private string strPFactura;
        private string strProveedor;
        IEnumerable<string> selectedCustomers;
        private bool isLoading = false;

        private int intIdCatProveedor = 0;
        private int[] lstintIdCatEmpresa;
        private int intIdCatCliente = 0;

        private ICollection<CatTipoEstados> lstCatTipoEstados;
        private ICollection<CatAduana> lstCatAduanas;

        FiltroSLOSolicitudes objFiltroSolicitudes = new FiltroSLOSolicitudes
        {
            IdCatTipoEstado = null,
            IdCatCliente = null,
            FechaPosicionamientoInicio = null,
            FechaPosicionamientoFin = null
        };

        private ICollection<SLOSolicitudes> lstSLOSolicitudes = null, listadas;
        public RadzenDataGrid<SLOSolicitudes> gridSolicitudes;
        private ICollection<CatClientes> lstCatClientes;
        private bool cargando = true;
        private Dictionary<int, string> tiposMercanciaPorSolicitud = new();
        int alto;
        int ancho;
        private string gridClass = "";
        private string zoomType = "";
        #endregion Variables

        #region Init
        protected override async Task OnInitializedAsync()
        {            
            await CargaDatos();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var size = await JS.InvokeAsync<ScreenSize>("getSizeScreen");
                ancho = size.Width;
                alto = size.Height;
                //await CargaDatos();
                if (ancho < 1300)
                {
                    gridClass = "grid-zoom";
                    zoomType = "display-zoom";
                }
                else
                {
                    gridClass = "grid-amplio";
                    zoomType = "";
                }
                    StateHasChanged();

                // Registrar listener de resize
                await JS.InvokeVoidAsync("registerResizeHandler", DotNetObjectReference.Create(this));
            }
        }
        #endregion

        #region DTO´s
        public class ScreenSize
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }

        #endregion

        #region CONTROLAR TAMAÑO PANTALLA
        [JSInvokable]
        public Task OnBrowserResize(ScreenSize size)
        {
            ancho = size.Width;
            alto = size.Height;

            // Aplicar estilos dinámicos o lógica según el tamaño
            if (ancho < 1300)
            {
                gridClass = "grid-zoom";
                zoomType = "display-zoom";
            }
            else
            {
                gridClass = "grid-amplio";
                zoomType = "";
            }
            StateHasChanged(); // fuerza re-render
            return Task.CompletedTask;
        }
        #endregion
        
        #region CARGAR DATOS
        async Task CargaDatos()
        {
            try
            {
                UsuarioTokenDTO usuarioToken = await _loginService.ObtenerdatosToken();
                lstCatTipoEstados = await CatTipoEstadoService.GetTiposEstado();
                lstCatAduanas = await CatAduanaService.GetAduanas();
                lstCatClientes = await ClientesService.GetClientes();
                lstSLOSolicitudes = await _solicitudesService.GetSolicitudes(objFiltroSolicitudes);
                //lstSLOSolicitudes = await _solicitudesService.GetSolicitudesListar();
                //listadas = await _solicitudesService.GetSolicitudesListar();

                //encabezados = await _encabezadoService.GetEncabezados(filtro);
                
                //Obtener Mercancía por Solicitud
                foreach(var solicitud in lstSLOSolicitudes)
                {
                    List<SLOSolicitudesDetalle> mercancia = await solicitudesDetalleService.SLOSolicitudesDetalleObtener(solicitud.IdSLOSolicitud);

                    var tipos = mercancia.Where(m => m.Activo && m.catTipoMercancia != null)
                        .Select(m => m.catTipoMercancia.Nombre)
                        .Distinct();

                    tiposMercanciaPorSolicitud[solicitud.IdSLOSolicitud] = string.Join(", ", tipos);
                }
            }
            finally
            {
                cargando = false;
                StateHasChanged();
            }            

        }
        #endregion

        #region Modal
        async Task OpenModal(string modo, SLOSolicitudes? solicitud = null)
        {

            var parameters = new Dictionary<string, object>
    {
        { "Modo", modo },
        { "UsuarioToken", await _loginService.ObtenerdatosToken() }
    };

            if (modo == "E" && solicitud != null)
            {
                parameters.Add("Solicitud", solicitud);
            }

            var response = await DialogService.OpenAsync<UltimaMillaDetPageRZ>(
                "Solicitud de servicios",
                parameters,
                new DialogOptions
                {
                    Width = "60%",
                    Height = "80%",
                    Resizable = true,
                    Draggable = true,
                    Style = "border-radius: 12px;"
                });

            if (response == true)
            {
                //await SweetAlertService.FireAsync(
                //    "Solicitud de servicio actualizada",
                //    "La solicitud fue modificada correctamente.",
                //    SweetAlertIcon.Success
                //);

                //await CargaDatos(); // Recarga el grid
                lstSLOSolicitudes = await _solicitudesService.GetSolicitudes(objFiltroSolicitudes);
                cargando = true;
                StateHasChanged();
                await gridSolicitudes.Reload();
                cargando = false;
                StateHasChanged();
            } if(response == false)
            {
                lstSLOSolicitudes = await _solicitudesService.GetSolicitudes(objFiltroSolicitudes);
                await gridSolicitudes.Reload();
            }
        }



        async Task SolicitudTransportista(string modo, SLOSolicitudes? solicitud = null)
        {
            try
            {
                if (solicitud.sloSOlicitudesDetalle.Any())
                {
                    var response = await DialogService.OpenAsync<UltimaMillaTranPageRZ>(
                    "SOLICITUD DE SERVICIO A TRANSPORTISTA",
                    new Dictionary<string, object>
                    {
                        /*{ "servicios", servicios },*/ // servicios debe ser List<Servicio>
                        { "Solicitud", solicitud },
                        //{"UsuarioToken", await _loginService.ObtenerdatosToken() },
                        { "Modo", modo }
                        //{"TransporteAsignando",null },
                        //{"TransporteDetalle", null }, //List         
                        //{"TransporteCronologia", null },
                        //{"SolicitudesDocumentos", null}
                    },
                    new DialogOptions
                    {
                        Width = "70%",
                        Height = "90%",
                        Resizable = true,
                        Draggable = true,
                        Style = "border-radius: 12px;"
                    });

                    if (response == true)
                    {
                        await CargaDatos(); // Recarga el grid
                        await gridSolicitudes.Reload();
                    }
                }
                else
                {
                    await SweetAlertService.FireAsync(
                    "",
                    "No es posible generar una solicitud de transporte debido a que esta solicitud de servicio ya no cuenta con mercacia disponible.",
                    SweetAlertIcon.Info
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        async Task BajaSolicitud(int id)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "¿Está seguro?",
                Text = $"¿Desea eliminar la solicitud con id: {id}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí, eliminar",
                CancelButtonText = "Cancelar"
            });
            if (result.IsConfirmed)
            {
                RespuestaGenericaDTO respuestaGenericaDTO = await _solicitudesService.BajaSolicitudSLO(id);
                if (respuestaGenericaDTO.IsSuccess == true)
                {
                    await SweetAlertService.FireAsync(
                       "Solicitud de servicio dada de baja",
                       "La solicitud ha sido dada de baja correctamente.",
                       SweetAlertIcon.Success
                   );

                    await CargaDatos(); // Recarga el grid
                    await gridSolicitudes.Reload();
                }
            }
        }
        #endregion Modal

        private async Task ControlTower(SLOSolicitudes solicitud)
        {
            //Verificar si existe el un transporte asignado para obtener el Control Tower
            RespuestaGenericaDTO respuestaGenerica = await SLOTControlTerrestreService.ObtenerPorIdTControlTerreste(solicitud.IdSLOSolicitud);

            if (respuestaGenerica.IsSuccess && respuestaGenerica.Entidades != null)
            {

                var response = await DialogService.OpenAsync<UltimaMillaTimeLineControlPageRZ>(
                $"Torre de control", new Dictionary<string, object>
                {
                    { "UsuarioDTO", await _loginService.ObtenerdatosToken() },
                    {"TControlTerrestre", respuestaGenerica.Entidades.Cast<SLOTControlTerrestre>().ToList() }

                },
                new DialogOptions
                {
                    Width = "100%",
                    Height = "100%",
                    Draggable = true,
                    Resizable = true
                });

               
                    await CargaDatos(); // Recarga el grid
                    await gridSolicitudes.Reload();
                
            }
            else
            {
                await SweetAlertService.FireAsync(
                    "Error",
                    "No se ha podido recuperar el registro de Torre de Control o no existe aún",
                    SweetAlertIcon.Error
                    );
            }


        }
        //private async Task MostrarEstado(Servicio item, string tipo)
        //{
        //    // Lógica para abrir modal según el tipo
        //  var response =  await DialogService.OpenAsync<UltimaMillaTimeLineControlPageRZ>(
        //        $"Estado del servicio - {tipo.ToUpper()}",
        //        new Dictionary<string, object>
        //        {
        //            {"Datos", item },
        //            {"Tipo", tipo }
        //        },
        //        new DialogOptions
        //        {
        //            Width = "100%",
        //            Height = "100%",
        //            Draggable = true,
        //            Resizable = true
        //        });

        //    if(response == true)
        //    {
        //        await CargaDatos();
        //        await gridSolicitudes.Reload();
        //    }
        //}

        private async Task FiltrarDTO()
        {
            List<string> validarList = new List<string>();

            if (objFiltroSolicitudes.FechaPosicionamientoInicio > objFiltroSolicitudes.FechaPosicionamientoFin)
                validarList.Add("<strong>Fecha Inicio</strong> no puede ser mayor que <strong>Fecha Entrega</strong>");

            string mensaje = "<ul style='padding-left: 20px; line-height: 1.6;'>" +
                             string.Join("", validarList.Select(e => $"<li>{e}</li>")) +
                             "</ul>";

            if (validarList.Any())
            {
                SweetAlertService.FireAsync("Filtro no valido", mensaje, SweetAlertIcon.Warning);
                return;
            }

            cargando = true;
            await CargaDatos();
            await gridSolicitudes.Reload();
        }
        private async Task BorrarFiltro()
        {
            cargando = true;
            objFiltroSolicitudes = new FiltroSLOSolicitudes();
            await FiltrarDTO();
        }

        private string GetTiposMercancia(int solicitudId)
        {
            if (tiposMercanciaPorSolicitud.TryGetValue(solicitudId, out var tipos))
            {
                return tipos;
            }
            return string.Empty;
        }

    }
}