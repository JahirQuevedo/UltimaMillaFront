using ALOG.Modelos;
using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using Radzen;
using Radzen.Blazor;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using static AlogisticsWASM.Pages.Logisticos.UltimaMilla.UltimaMillaPageRZ;
using static AlogisticsWASM.Pages.Vacios.Solicitudes.SolicitudesCRUDCMP;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaTranPageRZ
    {
        #region Parametros
        [Parameter] public String Modo { get; set; }
        [Parameter] public SLOSolicitudes Solicitud { get; set; }
        [Parameter] public SLOTransporteAsignado TransporteAsignado { get; set; }
        [Parameter] public SLOTransporteSolicitud SolicitudTransporte { get; set; }
        //[Parameter] public List<UltimaMillaPageRZ.Servicio> servicios { get; set; }
        #endregion

        #region Servicios
        [Inject] SweetAlertService SweetAlertService { get; set; }
        [Inject] ICatTransportistaService CatTransportistasService { get; set; }
        [Inject] ICatTipoOperacionesComercioService CatTipoOperacionesComercioService { get; set; }
        [Inject] ICatTipoEstadoService CatTipoEstadoService { get; set; }
        [Inject] ICatClientesUbicacionesService CatClientesUbicacionesService { get; set; }
        [Inject] ICatTipoTransporteService CatTipoTransporteService { get; set; }
        [Inject] ISLOTransporteSolicitudService SLOTransporteSolicitudService { get; set; }
        [Inject] ISLOTransporteAsignadoService SLOTransporteAsignadoService { get; set; }
        [Inject] ISLOTControlTerrestreService SLOTControlTerrestreService { get; set; }
        [Inject] private ISLOSolicitudesService sloSolicitudesService { get; set; }
        [Inject] private ISLOTransporteDetalleService sloTransporteDetalleService { get; set; }
        [Inject] private ILoginService _loginService { get; set; }
        [Inject] private ICatDocumentoService documentoService { get; set; }
        [Inject] private ISLODocumentosService sloDocumentosService { get; set; }
        [Inject] public NotificationService NotificationService { get; set; }
        [Inject] public ICatTipoOperacionesTransportesService CatTipoOperacionesTransportesService { get; set; }
        //[Inject] JSRuntime JS { get; set; }
        #endregion

        #region DTO's
        public class CargamentoDTO
        {
            public int Id { get; set; }
            public SLOTransporteDetalle? TransporteAsignado { get; set; }
            public SLOSolicitudesDetalle? SolicitudesDet { get; set; }
        }

        public class ScreenSize
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
        #endregion

        #region OBJETOS, LISTAS Y VARIABLES
        private SLOTransporteSolicitud objSLOTransporteSolicitud = new SLOTransporteSolicitud();
        private SLOTransporteDetalle objSLOTransporteDetalle = new SLOTransporteDetalle();
        private SLOTransporteAsignado objTransporteAsignado = new SLOTransporteAsignado();
        private SLOSolicitudes objSolicitudes = new SLOSolicitudes();
        private SLOCargarArchivo Documento = new SLOCargarArchivo();
        private ICollection<CatDocumentos> lstCatDocumentos;
        private ICollection<CatDocumentos> lstTransporteDocumentos;
        private List<CatTransportistas> lstCatTransportistas;
        private List<CatTipoOperacionComercio> lstCatOperacionComercio;
        private ICollection<CatTipoEstados> lstCatTipoEstados;
        private List<CatClientesUbicaciones> lstCatClientesUbicaciones;
        private ICollection<CatTipoTransporte> lstCatTipoTransporte;
        private List<SLOSolicitudesDetalle> lstCargamento = new();
        private List<SLOTransporteDetalle> lstTransporteDetalle = new();
        private List<SLOCargarArchivo> lstCargarArchivos = new List<SLOCargarArchivo>();
        private List<CatTipoOperacionesTransportes> lstCatTipoOperacionesTransporte = new List<CatTipoOperacionesTransportes>();
        private List<CatTipoOperacionesTransportes> lstCatTipoOperacionesTransporteTerrestre = new List<CatTipoOperacionesTransportes>();


        private SLOCargarArchivo archiCarga = new SLOCargarArchivo
        {
            TipoDocumento = null
        };
        private CargamentoDTO cargamentoDto;
        private List<CargamentoDTO> lstCargamentoDto = new();
        private RadzenDataGrid<CargamentoDTO> gridDetalles;
        private UsuarioTokenDTO UsuarioToken = new UsuarioTokenDTO();
        private HashSet<SLOSolicitudesDetalle> selectedItems = new();

        private RadzenAccordion? documentosAccordion;
        private RadzenUpload _radzenUpload;
        private Radzen.FileInfo _fileInfo;
        private string NombreArchivo;
        private long SizeFile;
        private int? _idDocumentoSeleccionado;

        private RadzenDataGrid<SLOCargarArchivo> documentosGrid;
        //private List<RadzenUpload> _radzenUploads;
        //private List<Radzen.FileInfo> _fileInfos;
        //private List<string> _nombresArchivos;
        //private List<long> _sizeFiles;
        //private List<int?> _idDocumentoSeleccionados;
        private string FolioUUID;

        private bool busy;

        private RadzenUpload uploadFiles;
        List<string> Validaciones = new();

        private int ancho;
        private int alto;
        private string zoomType;
        private int uploadKey = 0;
        private bool limpiarPendiente = false;
        private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
        private const int MaxCountFiles = 6;

        private int IdLineaNegocio;
        #endregion

        #region Funciones

        #region INICIALIZAR

        protected override async Task OnInitializedAsync()
        {
            UsuarioToken = await _loginService.ObtenerdatosToken();
            lstCatTransportistas = await CatTransportistasService.GetTransportistas();
            lstCatOperacionComercio = await CatTipoOperacionesComercioService.GetCatTipoOperacionesComercio();
            lstCatTipoEstados = await CatTipoEstadoService.GetTiposEstado();
            lstCatClientesUbicaciones = await CatClientesUbicacionesService.CatClientesUbicacionesListar();
            lstCatTipoTransporte = await CatTipoTransporteService.GetTipoTransporte();
            lstCatTipoOperacionesTransporte = await CatTipoOperacionesTransportesService.GetTiposOperacionesTransportes();
            objSLOTransporteSolicitud.IdCatTipoEstados = Solicitud.IdCatTipoEstado;
            IdLineaNegocio = (int)Solicitud?.Orden?.IdCatLineaNegocio;

            if (SolicitudTransporte != null && SolicitudTransporte.IdSLOTransporteSolicitud > 0)
            {
                lstTransporteDetalle = await sloTransporteDetalleService.GetObtenerTransporteDetalle(SolicitudTransporte.IdSLOTransporteSolicitud);
                var ids = lstTransporteDetalle.Select(x => x.IdSLOSolicitudDet).Distinct().ToList();
                lstCargamento = Solicitud.sloSOlicitudesDetalle.Where(d => ids.Contains(d.IdSLOSolicitudDet)).ToList();
            }
            else
            {
                if (lstTransporteDetalle.Any())
                {
                    var ids = lstTransporteDetalle.Select(x => x.IdSLOSolicitudDet).Distinct().ToList();
                    lstCargamento = Solicitud.sloSOlicitudesDetalle.Where(d => !ids.Contains(d.IdSLOSolicitudDet)).ToList();
                }
                else
                    lstCargamento.AddRange(Solicitud.sloSOlicitudesDetalle);
            }


            //lstCatDocumentos = await documentoService.GetTiposDocumento();
            lstCatDocumentos = await documentoService.ListarDocumentosLNegocio(IdLineaNegocio);
            lstTransporteDocumentos = lstCatDocumentos
                .Where(s => s.Acronimo == "CARTAPORTE")
                .ToList();

            lstCatTipoOperacionesTransporteTerrestre = lstCatTipoOperacionesTransporte.Where(t => t.IdCatTipoOperacionesSLO == 1).ToList();

            if (Modo == "R")
            {
                objSolicitudes = Solicitud;
                objSLOTransporteSolicitud = SolicitudTransporte;    
                objTransporteAsignado = TransporteAsignado;

                FolioUUID = lstTransporteDetalle?.FirstOrDefault()?.FolioUUID;

                foreach (var solicitudDetalle in lstCargamento)
                {
                    var transporte = lstTransporteDetalle.FirstOrDefault(t =>
                        t.IdSLOSolicitudDet == solicitudDetalle.IdSLOSolicitudDet);

                    cargamentoDto = new CargamentoDTO
                    {
                        SolicitudesDet = solicitudDetalle,
                        TransporteAsignado = transporte
                    };
                    lstCargamentoDto.Add(cargamentoDto);
                }

                await gridDetalles.Reload();
                lstCargamentoDto.Count();
            }
            else if (Modo == "C")
            {
                objSolicitudes = Solicitud;

                foreach (var solicitudDetalle in lstCargamento)
                {
                    bool tieneTransporte =
                        lstTransporteDetalle.Any(t => t.IdSLOSolicitudDet == solicitudDetalle.IdSLOSolicitudDet);

                    if (!tieneTransporte)
                    {
                        cargamentoDto = new CargamentoDTO()
                        {
                            SolicitudesDet = solicitudDetalle
                        };
                        lstCargamentoDto.Add(cargamentoDto);
                    }
                }

                await gridDetalles.Reload();
                lstCargamentoDto.Count();

                if (lstCargamentoDto.Count == 0)
                {
                    await SweetAlertService.FireAsync(
                        "Carga no disponible",
                        "El cargamento de esta solicitud se encuentra actualmente en transporte asignado",
                        SweetAlertIcon.Info);

                    DialogService.Close(false);
                }
            }
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
                    zoomType = "display-zoom";
                }
                else
                {
                    zoomType = "";
                }
                StateHasChanged();

                // Registrar listener de resize
                await JS.InvokeVoidAsync("registerResizeHandler", DotNetObjectReference.Create(this));
            }

            if (limpiarPendiente && uploadFiles != null)
            {
                limpiarPendiente = false; // quitar flag antes de limpiar para evitar recursión
                try
                {
                    await uploadFiles.ClearFiles();
                }
                catch
                {
                    // ignorar fallos al limpiar para no romper la UI
                }
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
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
                zoomType = "display-zoom";
            }
            else
            {
                zoomType = "";
            }
            StateHasChanged(); // fuerza re-render
            return Task.CompletedTask;
        }
        #endregion

        #region Funciones de UI
        #region VALIDAR EXPRESIONES REGULARES
        bool ContieneCaracteresInvalidos(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return false; // No tiene sentido validar si está vacío, eso ya se valida aparte

            // Regex que permite letras, números, guiones, espacios y puntos
            string patron = @"^[a-zA-Z0-9\s\.-]+$";
            return !Regex.IsMatch(valor, patron); // Devuelve true si hay caracteres inválidos
        }
        #endregion
        private async Task<bool> MostrarValidacion()
        {
            // Construir un mensaje HTML para SweetAlert
            string mensaje = "<ul style='padding-left:20px; line-height:1.5;'>" +
                             string.Join("", Validaciones.Select(e => $"<li>{e}</li>")) +
                             "</ul>";


            await SweetAlertService.FireAsync("Falta Información", mensaje, SweetAlertIcon.Warning);
            Validaciones = new();
            return false; // No continuar si hay errores
        }
        private async Task<bool> ValidarSolicitud()
        {
            //Validar Solicitud de Transporte
            if (objSLOTransporteSolicitud.IdCatTransportista == 0)
                Validaciones.Add("Se debe asignar un <strong>Transportista</strong>");

            if (objSLOTransporteSolicitud.IdCatTipoOperTransportes == 0)
                Validaciones.Add("Se debe asignar un <strong>Tipo de Transporte</strong>");

            if (selectedItems == null || !selectedItems.Any())
                Validaciones.Add("No hay <strong>Mercancía</strong> seleccionada para su transporte");

            if (Validaciones.Any())
                return await MostrarValidacion();
            //

            // Validación de Placas
            if (string.IsNullOrWhiteSpace(objTransporteAsignado.Placas))
            {
                Validaciones.Add("No se han agregado <strong>Placas</strong> de Transporte");
            }
            else
            {
                // Convertimos a mayúsculas para evitar errores por minúsculas
                string placas = objTransporteAsignado.Placas.Trim().ToUpper();

                // Regex: solo letras mayúsculas, números y guiones
                if (!Regex.IsMatch(placas, @"^[A-Z0-9-]+$"))
                {
                    Validaciones.Add("<strong>Placas</strong> solo pueden contener letras mayúsculas, números y guiones, sin otros símbolos");
                }

                // Asignamos la versión limpia de vuelta
                objTransporteAsignado.Placas = placas;
            }

            //if (string.IsNullOrWhiteSpace(objTransporteAsignado.Placas))
            //    Validaciones.Add("No se han agregado Placas de Transporte");
            //else if (ContieneCaracteresInvalidos(objTransporteAsignado.Placas))
            //    Validaciones.Add("Las Placas contienen caracteres no permitidos");

            if (string.IsNullOrWhiteSpace(objTransporteAsignado.Economico))
            {
                Validaciones.Add("No se ha agregado <strong>Económico</strong>");
            }
            else
            {
                string economico = objTransporteAsignado.Economico.Trim().ToUpper();

                // Regex: solo letras mayúsculas, números y guiones
                if (!Regex.IsMatch(economico, @"^[A-Z0-9-]+$"))
                {
                    Validaciones.Add("<strong>Económico</strong> solo puede contener letras mayúsculas, números y guiones, sin otros símbolos");
                }

                objTransporteAsignado.Economico = economico;
            }
            //else if (ContieneCaracteresInvalidos(objTransporteAsignado.Economico))
            //    Validaciones.Add("El valor Económico contiene caracteres no permitidos");

            // Validación de Color
            if (string.IsNullOrWhiteSpace(objTransporteAsignado.Color))
            {
                Validaciones.Add("No se ha agregado un <strong>Color</strong> de Transporte");
            }
            else
            {
                string color = objTransporteAsignado.Color.Trim().ToUpper();

                // Regex: solo letras mayúsculas (A-Z)
                if (!Regex.IsMatch(color, @"^[A-Z]+$"))
                {
                    Validaciones.Add("<strong>Color</strong> solo puede contener letras mayúsculas, sin números ni símbolos");
                }

                objTransporteAsignado.Color = color;
            }

            //else if (ContieneCaracteresInvalidos(objTransporteAsignado.Color))
            //    Validaciones.Add("El color contiene caracteres no permitidos");

            // Validación de Operador
            if (string.IsNullOrWhiteSpace(objTransporteAsignado.Operador))
            {
                Validaciones.Add("No se ha agregado información de <strong>Conductor</strong>");
            }
            else
            {
                string operador = objTransporteAsignado.Operador.Trim();

                // Regex: solo letras y espacios
                if (!Regex.IsMatch(operador, @"^[A-Za-zÁÉÍÓÚÑáéíóúñ\s]+$"))
                {
                    Validaciones.Add("<strong>Conductor</strong> solo puede contener letras y espacios");
                }

                objTransporteAsignado.Operador = operador;
            }

            //else if (ContieneCaracteresInvalidos(objTransporteAsignado.Operador))
            //    Validaciones.Add("El nombre del conductor contiene caracteres no permitidos")

            if (Validaciones.Any())
                return await MostrarValidacion();


            if (string.IsNullOrWhiteSpace(objTransporteAsignado.Marca))
            {
                Validaciones.Add("No se ha agregado <strong>Marca</strong> de transporte");
            }
            else
            {
                string marca = objTransporteAsignado.Marca.Trim();

                // Regex: letras, números y espacios
                if (!Regex.IsMatch(marca, @"^[A-Za-z0-9\s]+$"))
                {
                    Validaciones.Add("<strong>Marca</strong> solo puede contener letras, números y espacios");
                }

                objTransporteAsignado.Marca = marca;
            }

            //if (string.IsNullOrWhiteSpace(objTransporteAsignado.Marca))
            //    Validaciones.Add("No se ha agregado Marca de transporte");
            //else if (ContieneCaracteresInvalidos(objTransporteAsignado.Marca))
            //    Validaciones.Add("La marca contiene caracteres no permitidos");

            if (string.IsNullOrWhiteSpace(objSLOTransporteSolicitud.CAAT))
            {
                Validaciones.Add("No se ha agregado información en <strong>CAAT</strong>");
            }
            else if (!Regex.IsMatch(objSLOTransporteSolicitud.CAAT, @"^[a-zA-Z0-9-]+$"))
            {
                Validaciones.Add("<strong>CAAT</strong> contiene caracteres no permitidos (solo letras, números y guiones)");
            }

            if (string.IsNullOrWhiteSpace(objSLOTransporteDetalle.FolioUUID))
            {
                Validaciones.Add("No se asignó un <strong>FolioUUID</strong>");
            }
            else if (!Regex.IsMatch(objSLOTransporteDetalle.FolioUUID, @"^[a-zA-Z0-9-]+$"))
            {
                Validaciones.Add("<strong>FolioUUID</strong> contiene caracteres no permitidos (solo letras, números y guiones)");
            }


            //if (string.IsNullOrWhiteSpace(objSLOTransporteSolicitud.CAAT))
            //    Validaciones.Add("No se ha agregado información en CAAT");
            ////else if (ContieneCaracteresInvalidos(objSLOTransporteSolicitud.CAAT))
            ////    Validaciones.Add("CAAT contiene caracteres no permitidos");

            //if (string.IsNullOrWhiteSpace(objSLOTransporteDetalle.FolioUUID))
            //    Validaciones.Add("No se asignó un FolioUUID");
            //else if (ContieneCaracteresInvalidos(objSLOTransporteDetalle.FolioUUID))
            //    Validaciones.Add("FolioUUID contiene caracteres no permitidos");

            if (Validaciones.Any())
                return await MostrarValidacion();

            if (objSolicitudes.IdSLOSolicitud == 0)
                Validaciones.Add("No se cargó correctamenta la información de la Solicitud de Servicio");


            //Cargar en transporte Detalle los items seleccionados
            objSLOTransporteSolicitud.sloTransporteDetalle = new List<SLOTransporteDetalle>();
            var folioUUID = objSLOTransporteDetalle.FolioUUID;
            foreach (var mercancia in selectedItems)
            {
                var detalle = new SLOTransporteDetalle
                {
                    IdCatTipoOperTransportes = objSLOTransporteSolicitud.IdCatTipoOperTransportes,
                    IdCatTipoEstados = 1,
                    FechaRegistro = DateTime.Now,
                    Activo = true,
                    IdCatUsuarios = UsuarioToken.IdCatUsuario,
                    IdSLOSolicitudDet = mercancia.IdSLOSolicitudDet,
                    IdSLOSolicitud = objSolicitudes.IdSLOSolicitud,
                    FolioUUID = folioUUID
                };
                objSLOTransporteSolicitud.sloTransporteDetalle.Add(detalle);
            }
            //Asignar Transporte Detalle
            //objSLOTransporteSolicitud.sloTransporteDetalle = lstTransporteDetalle;

            if (!objSLOTransporteSolicitud.sloTransporteDetalle.Any())
                Validaciones.Add("No se asignó correctamente la carga a la Solicitud de Transporte");

            if (Validaciones.Any())
            {
                return await MostrarValidacion();
            }
            return true;
        }

        private async Task CrearSolicitudTransporte()
        {
            try
            {
                //objSLOTransporteSolicitud.IdCatTipoTransporte = 1;
                objSLOTransporteSolicitud.IdCatUsuarios = UsuarioToken.IdCatUsuario;
                objSLOTransporteSolicitud.IdSLOSolicitud = objSolicitudes.IdSLOSolicitud;
                objSLOTransporteSolicitud.Activo = true;

                // Normalizar (Trim) antes de validar
                objTransporteAsignado.Placas = string.IsNullOrWhiteSpace(objTransporteAsignado.Placas) ? null : objTransporteAsignado.Placas.Trim();
                objTransporteAsignado.Economico = string.IsNullOrWhiteSpace(objTransporteAsignado.Economico) ? null : objTransporteAsignado.Economico.Trim();
                objTransporteAsignado.Color = string.IsNullOrWhiteSpace(objTransporteAsignado.Color) ? null : objTransporteAsignado.Color.Trim();
                objTransporteAsignado.Marca = string.IsNullOrWhiteSpace(objTransporteAsignado.Marca) ? null : objTransporteAsignado.Marca.Trim();
                objTransporteAsignado.Operador = string.IsNullOrWhiteSpace(objTransporteAsignado.Operador) ? null : objTransporteAsignado.Operador.Trim();
                objSLOTransporteSolicitud.CAAT = string.IsNullOrWhiteSpace(objSLOTransporteSolicitud.CAAT) ? null : objSLOTransporteSolicitud.CAAT.Trim();
                objSLOTransporteDetalle.FolioUUID = string.IsNullOrWhiteSpace(objSLOTransporteDetalle.FolioUUID) ? null : objSLOTransporteDetalle.FolioUUID.Trim();




                var validacion = await ValidarSolicitud();

                if (!validacion)
                {
                    return;
                }
                //Hacer la carga de la información una vez validada la información
                #region CARGAR INFORMACION
                try
                {

                    RespuestaGenericaDTO TransporteSolicitudResponse = await SLOTransporteSolicitudService.SLOTransporteSolicitudCrear(objSLOTransporteSolicitud);

                    if (TransporteSolicitudResponse.IsSuccess)
                    {
                        //SLOTransporteSolicitud solicitudCreada = new SLOTransporteSolicitud();
                        //solicitudCreada = (SLOTransporteSolicitud)TransporteSolicitudResponse.Entidad;

                        objSLOTransporteSolicitud = (TransporteSolicitudResponse.Entidad as JObject)?.ToObject<SLOTransporteSolicitud>();
                        //objSLOTransporteSolicitud = solicitudCreada;

                        //var objSLOSolicitudTransporteCreada = solicitudCreada;
                        lstTransporteDetalle = new();
                        selectedItems = new();
                        objTransporteAsignado.IdSLOTransporteSolicitud = objSLOTransporteSolicitud.IdSLOTransporteSolicitud;
                        objTransporteAsignado.IdCatUsuarios = UsuarioToken.IdCatUsuario;
                        objTransporteAsignado.FechaRegistro = DateTime.Now;
                        objTransporteAsignado.Activo = true;
                        objTransporteAsignado.IdCatTipoOperTransportes = objSLOTransporteSolicitud.IdCatTipoOperTransportes;
                        //objTransporteAsignado.IdCatTipoTransporte = 1;
                        //objTransporteAsignado.sloTransporteAsignadoDetalle = new List<SLOTransporteAsignadoDetalle>();

                        //foreach (var item in objSLOTransporteSolicitud.sloTransporteDetalle)
                        //{
                        //    var detalle = new SLOTransporteAsignadoDetalle
                        //    {
                        //        IdSLOTransporteDetalle = item.IdSLOTransporteDetalle,
                        //        IdCatUsuarioRegistro = UsuarioToken.IdCatUsuario,
                        //        Activo = true,
                        //        FechaRegistro = DateTime.Now,

                        //    };
                        //    objTransporteAsignado.sloTransporteAsignadoDetalle.Add(detalle);
                        //}

                        try
                        {

                            RespuestaGenericaDTO TransporteAsignadoResponse = await SLOTransporteAsignadoService.SLOTransporteAsignadoCrear(objTransporteAsignado);

                            if (TransporteAsignadoResponse.IsSuccess)
                            {
                                await ProcesarDocumentosAsync();

                                SLOTransporteAsignado objTransporteAsignacionCreada = (SLOTransporteAsignado)TransporteAsignadoResponse.Entidad;

                                SLOTControlTerrestre objControlTerrestre = new();

                                objControlTerrestre.FechaRegistro = DateTime.Now;
                                objControlTerrestre.Activo = true;
                                objControlTerrestre.IdCatUsuarios = UsuarioToken.IdCatUsuario;
                                objControlTerrestre.IdSLOTransporteAsignado = objTransporteAsignacionCreada.IdSLOTransporteAsignado;
                                objControlTerrestre.IdSLOSolicitud = objSolicitudes.IdSLOSolicitud;

                                RespuestaGenericaDTO SLOControlTerrestreResponse = await SLOTControlTerrestreService.CrearTControlTerrestre(objControlTerrestre);

                                await SweetAlertService.FireAsync("Creado", "Solicitud de Servicio de Transporte y seguimiento creado", SweetAlertIcon.Success);
                                lstCargamento = new();
                                lstCargamentoDto = new();
                                lstCargarArchivos = new();
                                objSolicitudes = new();
                                objTransporteAsignado = new();
                                objSLOTransporteSolicitud = new();
                                objSLOTransporteDetalle = new();

                                DialogService.Close(true);

                            }
                            if (TransporteAsignadoResponse.IsSuccess == false)
                            {
                                await SweetAlertService.FireAsync("Error", "Error al crear el Transporte Asignado", SweetAlertIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex}");
                        }

                    }
                    if (TransporteSolicitudResponse.IsSuccess == false)
                    {
                        await SweetAlertService.FireAsync("Error", "Error al crear la Solicitud de Transporte", SweetAlertIcon.Error);
                        lstTransporteDetalle = new();
                        selectedItems = new();
                        objSLOTransporteSolicitud = new();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        private async Task<RespuestaGenericaDTO> CrearTransporteAsigancion()
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new();
            respuestaGenericaDTO.IsSuccess = false;
            //objTransporteAsignado.IdCatUsuarios = UsuarioToken.IdCatUsuario;
            objTransporteAsignado.IdSLOTransporteSolicitud = objSLOTransporteSolicitud.IdSLOTransporteSolicitud;
            objTransporteAsignado.IdCatTipoOperTransportes = objSLOTransporteSolicitud.IdCatTipoOperTransportes;
            objSLOTransporteSolicitud.IdCatTipoEstados = 2;
            var nuevoTransporteAsignado = new SLOTransporteAsignado()
            {
                IdCatUsuarios = UsuarioToken.IdCatUsuario,
                IdCatTipoOperTransportes = objSLOTransporteSolicitud.IdCatTipoOperTransportes,
                Placas = objTransporteAsignado.Placas,
                Economico = objTransporteAsignado.Economico,
                Color = objTransporteAsignado.Color,
                Operador = objTransporteAsignado.Operador,
                Marca = objTransporteAsignado.Marca,
                FechaRegistro = DateTime.Now,
                IdSLOTransporteSolicitud = objSLOTransporteSolicitud.IdSLOTransporteSolicitud
            };

            respuestaGenericaDTO = await SLOTransporteAsignadoService.SLOTransporteAsignadoCrear(/*objTransporteAsignado*/nuevoTransporteAsignado);

            if (respuestaGenericaDTO.IsSuccess)
            {
                //await SweetAlertService.FireAsync(
                //    "Exito",
                //    "El transporte fue asignado correctamente",
                //    SweetAlertIcon.Success);



                DialogService.Close(true);
                return respuestaGenericaDTO;
            }
            else if (!respuestaGenericaDTO.IsSuccess)
            {
                await SweetAlertService.FireAsync(
                    "Error",
                    "La asignación no pudo ser creada correctamente debido a un error inesperado",
                    SweetAlertIcon.Error);
                return respuestaGenericaDTO;
            }
            else
            {
                return respuestaGenericaDTO;
            }
        }

        private bool IsSelected(SLOSolicitudesDetalle item)
        {
            return selectedItems.Contains(item);
        }

        private void OnSelectChanged(SLOSolicitudesDetalle item, bool isSelected)
        {
            if (isSelected)
            {
                selectedItems.Add(item);
            }
            else
            {
                selectedItems.Remove(item);
            }
        }

        //Control de Agregado de Documentos de manera dínamica
        // Control de agregado de Documentos de manera dinámica
        //private async Task AgregarDocumento()
        //{
        //    // Abrir modal y obtener la lista de documentos
        //    var result = await DialogService.OpenAsync<UltimaMillaModalDocumentos>("Cargar Documentos",
        //        null,
        //        new DialogOptions() { Width = "30%", Height = "30%", Resizable = true, Draggable = true, ShowClose = false });

        //    // Validar que se haya retornado algo
        //    if (result == null)
        //        return;

        //    // Intentar convertir a la lista de documentos
        //    if (result is List<UltimaMillaModalDocumentos.DocumentoUploadItem> documentosModal && documentosModal.Any())
        //    {
        //        int procesados = 0;

        //        foreach (var documento in documentosModal)
        //        {
        //            if (documento.FileInfo != null && !string.IsNullOrEmpty(documento.AcronimoDocumento))
        //            {
        //                // Evitar duplicados (ejemplo: mismo nombre de archivo)
        //                var existe = lstCargarArchivos.Any(d => d.File.Name == documento.FileInfo.Name &&
        //                                                        d.TipoDocumento == documento.AcronimoDocumento);
        //                if (existe)
        //                    continue;

        //                lstCargarArchivos.Add(new SLOCargarArchivo
        //                {
        //                    IdOrden = objSolicitudes.IdOrden,
        //                    IdUsuario = UsuarioToken.IdCatUsuario,
        //                    TipoDocumento = documento.AcronimoDocumento,
        //                    Identificador = objTransporteAsignado?.Placas, // opcional si existe
        //                    File = documento.FileInfo
        //                });

        //                procesados++;
        //            }
        //        }

        //        if (procesados == 0)
        //        {
        //            IJsHelper.MostrarNotificacion(
        //                NotificationService,
        //                "Validación",
        //                "No se seleccionó ningún documento válido.",
        //                NotificationSeverity.Warning,
        //                4000
        //            );
        //        }
        //        else
        //        {
        //            // Refrescar el grid solo si hubo documentos agregados
        //            await documentosGrid.Reload();
        //            StateHasChanged();

        //            IJsHelper.MostrarNotificacion(
        //                NotificationService,
        //                "Éxito",
        //                $"{procesados} documento(s) agregado(s) correctamente.",
        //                NotificationSeverity.Success,
        //                3000
        //            );
        //        }
        //    }
        //    else
        //    {
        //        IJsHelper.MostrarNotificacion(
        //            NotificationService,
        //            "Validación",
        //            "No se agregaron documentos a la carga.",
        //            NotificationSeverity.Warning,
        //            4000
        //        );
        //    }
        //}




        //private void AgregarDocumentos()
        //{
        //    lstCargarArchivos.Add(new SLOCargarArchivo
        //    {
        //        TipoDocumento = null
        //    });

        //}

        //private async Task SelectDocumentoTransporte(UploadChangeEventArgs args)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //private async Task OnUploadChange(UploadChangeEventArgs args)
        //{
        //    var files = args.Files?.ToList();
        //    if (files == null || !files.Any())
        //        return;

        //    // Abrir modal para clasificar los archivos
        //    var result = await DialogService.OpenAsync<UltimaMillaModalDocumentos>(
        //        "Clasificar Documentos",
        //        new Dictionary<string, object>() { { "Archivos", files } },
        //        new DialogOptions() { Width = "40%", Height = "40%", Resizable = true, Draggable = true, ShowClose = false }
        //    );

        //    // Procesar resultados del modal
        //    //if (result is List<SLOCargarArchivo> listaFinal && listaFinal.Any())
        //    //{
        //    //    lstCargarArchivos.AddRange(listaFinal);
        //    //}
        //    if (result is List<UltimaMillaModalDocumentos.DocumentoUploadItem> listaFinal && listaFinal.Any())
        //    {
        //        foreach (var doc in listaFinal)
        //        {
        //            lstCargarArchivos.Add(new SLOCargarArchivo()
        //            {
        //                TipoDocumento = doc.AcronimoDocumento,
        //                Identificador = objTransporteAsignado.Placas,
        //                File = doc.FileInfo,
        //                IdOrden = objSolicitudes.IdOrden,
        //                IdUsuario = UsuarioToken.IdCatUsuario
        //            });
        //        }

        //        await uploadFiles.ClearFiles();

        //    }
        //}


        //private async Task OnUploadChange(UploadChangeEventArgs args)
        //{
        //    long MaxFilesSize = 2 * 1024 * 1024; //2 MB
        //    int MaxCountFiles = 6;
        //    var files = args.Files?.ToList() ?? new List<Radzen.FileInfo>();

        //    if (files.Count > 6)
        //    {
        //        NotificationService.Notify(new NotificationMessage
        //        {
        //            Severity = NotificationSeverity.Warning,
        //            Summary = "Cantidad máxima superada",
        //            Detail = $"No puedes cargar más de 6 archivos a la vez.",
        //            Duration = 4000
        //        });

        //        // Limpia la selección para evitar el crash
        //        await uploadFiles.ClearFiles();
        //        return;
        //    }

        //    //Validar que los archivos sean del tipo valido
        //    var filesInvalidos = args.Files?.ToList()
        //.Where(f => f.ContentType != "application/pdf")
        //.ToList();

        //    foreach (var file in filesInvalidos)
        //    {
        //        NotificationService.Notify(new NotificationMessage
        //        {
        //            Severity = NotificationSeverity.Error,
        //            Summary = "Tipo de archivo no permitido",
        //            Detail = $"{file.Name} no es un PDF",
        //            Duration = 4000
        //        });

        //        return;                ; // elimina archivos inválidos
        //    }

        //    List<string> validacionesDocumento = new();


        //    if (files == null || !files.Any())
        //    {
        //        validacionesDocumento.Add("No se cargaron documentos");
        //    }

        //    if (files.Count >= MaxCountFiles)
        //    {
        //        validacionesDocumento.Add("Solo se pueden cargar 6 documentos simultaneamente");
        //    }



        //    foreach (var file in files)
        //    {
        //        if(file.Size > MaxFilesSize)
        //        {
        //            validacionesDocumento.Add($"{file.Name} excede el tamaño máximo permitido de 2 MB");
        //            break;
        //        }
        //    }

        //    if (validacionesDocumento.Any())
        //    {
        //        string validacionMensaje = string.Join(", ", validacionesDocumento);
        //        NotificationService.Notify(new NotificationMessage
        //        {
        //            Severity = NotificationSeverity.Error,
        //            Summary = "No hay archivos",
        //            Detail = $"{validacionMensaje}",
        //            Duration = 4000
        //        });
        //        //await uploadFiles.RemoveFile(file.Name);
        //        //await uploadFiles.ClearFiles();                
        //        return;
        //    }



        //    //if (files.Count > 6)
        //    //{
        //    //    await SweetAlertService.FireAsync("Cantidad Máxima Superada", "No se puede cargar más de 6 archivos de forma simultanea.", SweetAlertIcon.Warning);
        //    //    await uploadFiles.ClearFiles();
        //    //    return;
        //    //}


        //    // Abrir modal para clasificar los archivos
        //    var result = await DialogService.OpenAsync<UltimaMillaModalDocumentos>(
        //        "Clasificar Documentos",
        //        new Dictionary<string, object>() { { "Archivos", files }, { "Modo", "TRANSPORTE" } },
        //        new DialogOptions() { Width = "60%", Height = "55%", Resizable = true, Draggable = true, ShowClose = false, Style = "border-radius: 12px;" }
        //    );

        //    // Procesar resultados del modal
        //    if (result is List<UltimaMillaModalDocumentos.DocumentoUploadItem> listaFinal && listaFinal.Any())
        //    {
        //        foreach (var doc in listaFinal)
        //        {
        //            // Convertir IBrowserFile a byte[]
        //            byte[] fileBytes = null;
        //            if (doc.FileInfo != null)
        //            {
        //                using var ms = new MemoryStream();
        //                await doc.FileInfo.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024).CopyToAsync(ms);
        //                fileBytes = ms.ToArray();
        //            }

        //            lstCargarArchivos.Add(new SLOCargarArchivo()
        //            {
        //                TipoDocumento = doc.AcronimoDocumento,
        //                Identificador = objTransporteAsignado.Placas,
        //                NombreArchivo = doc.FileInfo?.Name,
        //                SizeFile = doc.FileInfo?.Size ?? 0,
        //                ContentType = doc.FileInfo?.ContentType,
        //                FileBytes = fileBytes,
        //                IdOrden = objSolicitudes.IdOrden,
        //                IdUsuario = UsuarioToken.IdCatUsuario
        //            });
        //        }

        //        // Limpiar selección del upload
        //        await uploadFiles.ClearFiles();
        //    }
        //    await uploadFiles.ClearFiles();
        //}


        private async Task OnUploadChange(UploadChangeEventArgs args)
        {
            // Si estamos limpiando por código, ignoramos el evento
            if (limpiarPendiente)
            {
                return;
            }

            var files = args.Files?.ToList() ?? new List<Radzen.FileInfo>();

            // Si no hay archivos (por ejemplo, ClearFiles disparó OnChange), salir sin mensajes
            if (!files.Any())
                return;

            // 1) Validaciones rápidas: cantidad
            if (files.Count > MaxCountFiles)
            {
                await MostrarAlertaLimiteCantidad();
                // marcar limpieza para hacerla fuera del evento
                limpiarPendiente = true;
                return;
            }

            // 2) Validar tipos y tamaños (agrupar errores)
            var errores = new List<string>();

            // Tipos inválidos (solo permitimos PDF)
            var tiposInvalidos = files
                .Where(f =>
                {
                    var contentType = (f.ContentType ?? "").ToLowerInvariant();
                    var ext = System.IO.Path.GetExtension(f.Name ?? "").ToLowerInvariant();
                    return !(contentType == "application/pdf" || ext == ".pdf");
                })
                .ToList();

            if (tiposInvalidos.Any())
                errores.Add($"Los siguientes archivos no son PDF: {string.Join(", ", tiposInvalidos.Select(x => x.Name))}");

            // Tamaño por archivo
            var grandes = files.Where(f => f.Size > MaxFileSize).ToList();
            if (grandes.Any())
                errores.Add($"Los siguientes archivos exceden {MaxFileSize / 1024 / 1024} MB: {string.Join(", ", grandes.Select(x => x.Name))}");

            if (errores.Any())
            {
                // Mostrar todos los errores juntos (puedes usar SweetAlert o NotificationService)
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Validación fallida",
                    Detail = string.Join(" / ", errores),
                    Duration = 6000
                });

                limpiarPendiente = true; // limpiar fuera del evento
                return;
            }

            // 3) Si pasa validación, abrir modal para clasificar los archivos
            var result = await DialogService.OpenAsync<UltimaMillaModalDocumentos>(
                "Clasificar Documentos",
                new Dictionary<string, object>() { { "Archivos", files }, { "Modo", "TRANSPORTE" } },
                new DialogOptions() { Width = "60%", Height = "55%", Resizable = true, Draggable = true, ShowClose = false, Style = "border-radius: 12px;" }
            );

            // 4) Procesar resultado del modal
            if (result is List<UltimaMillaModalDocumentos.DocumentoUploadItem> listaFinal && listaFinal.Any())
            {
                foreach (var doc in listaFinal)
                {
                    byte[] fileBytes = null;
                    if (doc.FileInfo != null)
                    {
                        using var ms = new MemoryStream();
                        await doc.FileInfo.OpenReadStream(maxAllowedSize: MaxFileSize).CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }

                    lstCargarArchivos.Add(new SLOCargarArchivo()
                    {
                        TipoDocumento = doc.AcronimoDocumento,
                        Identificador = objTransporteAsignado.Placas,
                        NombreArchivo = doc.FileInfo?.Name,
                        SizeFile = doc.FileInfo?.Size ?? 0,
                        ContentType = doc.FileInfo?.ContentType,
                        FileBytes = fileBytes,
                        IdOrden = objSolicitudes.IdOrden,
                        IdUsuario = UsuarioToken.IdCatUsuario
                    });
                }

                // programar limpieza del componente después del render
                limpiarPendiente = true;
            }
            else
            {
                limpiarPendiente = true;
            }
        }

        // Método auxiliar para notificar límite (puedes usar SweetAlertService en vez de NotificationService)
        private async Task MostrarAlertaLimiteCantidad()
        {
            // Si usas SweetAlertService:
            // await SweetAlertService.FireAsync("Límite superado", $"No puedes cargar más de {MaxCountFiles} archivos.", SweetAlertIcon.Warning);

            // O usar NotificationService:
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Warning,
                Summary = "Cantidad máxima superada",
                Detail = $"No puedes cargar más de {MaxCountFiles} archivos a la vez.",
                Duration = 4000
            });
        }

        private async Task ProcesarDocumentosAsync()
        {
            if (lstCargarArchivos == null || !lstCargarArchivos.Any())
            {
                //await SweetAlertService.FireAsync("Validación", "No hay documentos para procesar.", SweetAlertIcon.Warning);
                return;
            }

            int total = lstCargarArchivos.Count;
            int procesados = 0;
            bool todosCorrectos = true;

            // 🔹 Mostrar modal inicial sin bloquear
            //_ = SweetAlertService.FireAsync(new SweetAlertOptions
            //{
            //    Title = "Subiendo documentos...",
            //    Text = $"0 de {total}",
            //    Icon = SweetAlertIcon.Info,
            //    AllowOutsideClick = false,
            //    ShowConfirmButton = false
            //});

            //await Task.Yield(); // asegura que el modal se monte
            //await SweetAlertService.ShowLoadingAsync();

            foreach (var archiCarga in lstCargarArchivos)
            {
                try
                {
                    archiCarga.IdOrden = objSolicitudes.IdOrden;
                    archiCarga.IdUsuario = UsuarioToken.IdCatUsuario;
                    archiCarga.Identificador = objSLOTransporteSolicitud.IdSLOTransporteSolicitud.ToString();

                    // 🔹 Actualizar progreso en el mismo modal
                    //await SweetAlertService.UpdateAsync(new SweetAlertOptions
                    //{
                    //    Title = "Subiendo documentos...",
                    //    Text = $"{procesados + 1} de {total}"
                    //});

                    RespuestaGenericaDTO respon = await sloDocumentosService.SLOUploadFile(archiCarga);

                    if (!respon.IsSuccess)
                    {
                        todosCorrectos = false;
                        await SweetAlertService.CloseAsync();
                        await SweetAlertService.FireAsync("Error", $"No se pudo cargar el archivo {archiCarga.NombreArchivo}.", SweetAlertIcon.Error);
                        break; // detener en el primer error
                    }
                }
                catch (Exception ex)
                {
                    todosCorrectos = false;
                    await SweetAlertService.CloseAsync();
                    await SweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                    break;
                }

                procesados++;
            }

            // 🔹 Al finalizar
            await SweetAlertService.CloseAsync();

            if (todosCorrectos)
            {
                //await SweetAlertService.FireAsync("Éxito", "Todos los documentos se cargaron correctamente.", SweetAlertIcon.Success);
            }
            else
            {
                await SweetAlertService.FireAsync("Proceso incompleto", "Algunos documentos no se pudieron cargar.", SweetAlertIcon.Warning);
            }
        }

        private async Task EliminarDocumento(SLOCargarArchivo doc)
        {
            lstCargarArchivos.Remove(doc);
            await documentosGrid.Reload();
        }

        #region VALIDAR INFORMACION TRANSPORTE
        private void RegularizarPlacas(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(objTransporteAsignado.Placas))
            {
                objTransporteAsignado.Placas = new string(objTransporteAsignado.Placas
                    .ToUpper()
                    .Where(c => char.IsLetterOrDigit(c) || c == '-')
                    .ToArray());
            }
        }


        #endregion
        #endregion


        #endregion
    }
}
