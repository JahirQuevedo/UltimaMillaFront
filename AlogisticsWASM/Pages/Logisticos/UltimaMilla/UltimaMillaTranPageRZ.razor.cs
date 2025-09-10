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
using Newtonsoft.Json.Linq;
using Radzen;
using Radzen.Blazor;
using System.Drawing.Text;
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
        #endregion



        #region DTO's
        public class CargamentoDTO
        {
            public int Id { get; set; }
            public SLOTransporteDetalle? TransporteAsignado { get; set; }
            public SLOSolicitudesDetalle? SolicitudesDet { get; set; }
        }
        #endregion

        #region Objetos y listas
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

        private bool busy;

        private RadzenUpload uploadFiles;
        List<string> Validaciones = new();
        #endregion

        #region Funciones

        #region Inicializar

        protected override async Task OnInitializedAsync()
        {
            UsuarioToken = await _loginService.ObtenerdatosToken();
            lstCatTransportistas = await CatTransportistasService.GetTransportistas();
            lstCatOperacionComercio = await CatTipoOperacionesComercioService.GetCatTipoOperacionesComercio();
            lstCatTipoEstados = await CatTipoEstadoService.GetTiposEstado();
            lstCatClientesUbicaciones = await CatClientesUbicacionesService.CatClientesUbicacionesListar();
            lstCatTipoTransporte = await CatTipoTransporteService.GetTipoTransporte();
            objSLOTransporteSolicitud.IdCatTipoEstados = Solicitud.IdCatTipoEstado;

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


            lstCatDocumentos = await documentoService.GetTiposDocumento();
            lstTransporteDocumentos = lstCatDocumentos
                .Where(s => s.Acronimo == "CARTAPORTE" || s.Acronimo == "POD")
                .ToList();


            if (Modo == "R")
            {
                objSolicitudes = Solicitud;
                objSLOTransporteSolicitud = SolicitudTransporte;
                objTransporteAsignado = TransporteAsignado;

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
        #endregion

        #region Funciones de UI
        private async Task CrearSolicitudTransporte()
        {
            try
            {
                objSLOTransporteSolicitud.IdCatTipoTransporte = 1;
                objSLOTransporteSolicitud.IdCatUsuarios = UsuarioToken.IdCatUsuario;
                objSLOTransporteSolicitud.IdSLOSolicitud = objSolicitudes.IdSLOSolicitud;
                objSLOTransporteSolicitud.Activo = true;


                if (objSLOTransporteSolicitud.IdCatTransportista == 0)
                    Validaciones.Add("Se debe asignar un Transportista");

                if (selectedItems == null || !selectedItems.Any())
                    Validaciones.Add("No hay mercancía seleccionada para su transporte");

                if (objTransporteAsignado.Placas == null || objTransporteAsignado.Placas == "")
                    Validaciones.Add("No se han agregado Placas de Transporte");

                if (objTransporteAsignado.Economico == null || objTransporteAsignado.Economico == "")
                    Validaciones.Add("No se ha agregado un valor Economico");

                if (objTransporteAsignado.Color == null || objTransporteAsignado.Color == "")
                    Validaciones.Add("No se ha agregado un color de Transporte");

                if (objTransporteAsignado.Marca == null || objTransporteAsignado.Marca == "")
                    Validaciones.Add("No se ha agregado Marca de transporte");

                if (objTransporteAsignado.Operador == null || objTransporteAsignado.Operador == "")
                    Validaciones.Add("No se ha agregado información de Conductor");

                if (objSLOTransporteSolicitud.CAAT == null || objSLOTransporteSolicitud.CAAT == "")
                    Validaciones.Add("No se ha agregado información en CAAT");

                if (objSLOTransporteDetalle.FolioUUID == null || objSLOTransporteDetalle.FolioUUID == "")
                    Validaciones.Add("No se asignó un FolioUUID");

                if (objSolicitudes.IdSLOSolicitud == 0)
                    Validaciones.Add("No se cargó correctamenta la información de la Solicitud de Servicio");


                //Cargar en transporte Detalle los items seleccionados
                objSLOTransporteSolicitud.sloTransporteDetalle = new List<SLOTransporteDetalle>();

                foreach (var mercancia in selectedItems)
                {
                    var detalle = new SLOTransporteDetalle
                    {
                        IdCatTipoTransporte = 1,
                        IdCatTipoEstados = 1,
                        FechaRegistro = DateTime.Now,
                        Activo = true,
                        IdCatUsuarios = UsuarioToken.IdCatUsuario,
                        IdSLOSolicitudDet = mercancia.IdSLOSolicitudDet,
                        IdSLOSolicitud = objSolicitudes.IdSLOSolicitud,
                        FolioUUID = objSLOTransporteDetalle.FolioUUID
                    };
                    objSLOTransporteSolicitud.sloTransporteDetalle.Add(detalle);
                }
                //Asignar Transporte Detalle
                //objSLOTransporteSolicitud.sloTransporteDetalle = lstTransporteDetalle;

                if (!objSLOTransporteSolicitud.sloTransporteDetalle.Any())
                    Validaciones.Add("No se asignó correctamente la carga a la Solicitud de Transporte");

                if (Validaciones.Any())
                {
                    // Construir un mensaje HTML para SweetAlert
                    string mensaje = "<ul style='padding-left:20px; line-height:1.5;'>" +
                                     string.Join("", Validaciones.Select(e => $"<li>{e}</li>")) +
                                     "</ul>";


                    await SweetAlertService.FireAsync("Falta Información", mensaje, SweetAlertIcon.Warning);
                    Validaciones = new();
                    return; // No continuar si hay errores
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
                        objTransporteAsignado.IdCatTipoTransporte = 1;
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
            objTransporteAsignado.IdCatTipoTransporte = objSLOTransporteSolicitud.IdCatTipoTransporte;
            objSLOTransporteSolicitud.IdCatTipoEstados = 2;
            var nuevoTransporteAsignado = new SLOTransporteAsignado()
            {
                IdCatUsuarios = UsuarioToken.IdCatUsuario,
                IdCatTipoTransporte = objSLOTransporteSolicitud.IdCatTipoTransporte,
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
        private async Task OnUploadChange(UploadChangeEventArgs args)
        {
            var files = args.Files?.ToList();
            if (files == null || !files.Any())
                return;

            // Abrir modal para clasificar los archivos
            var result = await DialogService.OpenAsync<UltimaMillaModalDocumentos>(
                "Clasificar Documentos",
                new Dictionary<string, object>() { { "Archivos", files }, { "Modo", "TRANSPORTE" } },
                new DialogOptions() { Width = "40%", Height = "40%", Resizable = true, Draggable = true, ShowClose = false }
            );

            // Procesar resultados del modal
            if (result is List<UltimaMillaModalDocumentos.DocumentoUploadItem> listaFinal && listaFinal.Any())
            {
                foreach (var doc in listaFinal)
                {
                    // Convertir IBrowserFile a byte[]
                    byte[] fileBytes = null;
                    if (doc.FileInfo != null)
                    {
                        using var ms = new MemoryStream();
                        await doc.FileInfo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(ms);
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

                // Limpiar selección del upload
                await uploadFiles.ClearFiles();
            }
        }


        private async Task ProcesarDocumentosAsync()
        {
            if (lstCargarArchivos == null || !lstCargarArchivos.Any())
            {
                await SweetAlertService.FireAsync("Validación", "No hay documentos para procesar.", SweetAlertIcon.Warning);
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

                    bool respon = await sloDocumentosService.SLOUploadFile(archiCarga);

                    if (!respon)
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

        #endregion


        #endregion



    }
}
