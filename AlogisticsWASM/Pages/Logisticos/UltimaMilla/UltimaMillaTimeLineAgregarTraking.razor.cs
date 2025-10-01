using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using ClienteBlazorWASM.Helpers;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using System.Drawing;
using System.Linq.Dynamic.Core;
using static System.Net.WebRequestMethods;
using ALOG.Modelos.Modelos.DTO.Consultas;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaTimeLineAgregarTraking
    {
        #region PARAMETROS
        [Parameter] public SLOTControlTerrestre TControlTerrestre { get; set; }
        [Parameter] public UsuarioTokenDTO UsuarioDTO { get; set; }
        [Parameter] public DateTime? Booking { get; set; }
        [Parameter] public string Modo { get; set; }
        [Parameter] public int? IdCron { get; set; }

        #endregion

        #region SERVICIOS
        [Inject] private ISLOTControlTerrestreService servicioControl { get; set; }
        [Inject] private ICatTipoEventosCronService catTipoEventosCronService { get; set; }
        [Inject] private DialogService dialogService { get; set; }
        [Inject] private ISLOTransporteCronService TransporteCronService { get; set; }
        [Inject] private SweetAlertService sweetAlertService { get; set; }
        [Inject] ISLODocumentosService sloDocumentosService { get; set; }
        [Inject] ISLOTransporteCronDocumentosService SLOTransporteCronDocumentosService { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        [Inject] private HttpClient Http { get; set; }
        #endregion

        #region OBJETOS Y LISTAS
        private SLOTControlTerrestre objTControlTerrestre = new();
        private SLOTransportesCron objTransporteCron = new SLOTransportesCron();
        private ICollection<CatTipoEventosCron> listaTiposEventos;
        private RadzenUpload uploadFiles;
        private List<SLOCargarArchivo> lstCargarArchivos = new List<SLOCargarArchivo>();
        private RadzenDataGrid<SLOCargarArchivo> documentosGrid;
        public DateTime? FechaMinimaBooking;
        private bool boolEditable = false;
        private List<SLOSolicitudesDocumentos> lstDocumentosGuardados = new List<SLOSolicitudesDocumentos>();
        private List<SLOTransporteCronDocumentos> lstTransporteCronDocumentos = new List<SLOTransporteCronDocumentos>();
        private List<SLOSolicitudesDocumentos> lstPruebasIncidencia = new List<SLOSolicitudesDocumentos>();
        private int idCron;
        private RadzenDataGrid<SLOSolicitudesDocumentos> gridArchivosIncidencias;

        //variables de RadzenUpload
        private int uploadKey = 0;
        private bool limpiarPendiente = false;
        private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
        private const int MaxCountFiles = 6;

        //FiltroDocumentos
        FiltroGenericoDTO filtroDocumentos = new FiltroGenericoDTO();
        #endregion

        #region INICIALIZAR
        protected override async Task OnInitializedAsync()
        {
            if(Modo == "E")
            {
                idCron = IdCron.Value;
                RespuestaGenericaDTO respuestaGenericaDTO = await TransporteCronService.ObtenerPorIDTransporteCron(idCron);                
                if (respuestaGenericaDTO.IsSuccess)
                {
                    objTransporteCron = (SLOTransportesCron)respuestaGenericaDTO.Entidad;

                    filtroDocumentos.Id = TControlTerrestre.sloSolicitudes.IdSLOSolicitud;
                    filtroDocumentos.IdTipoDocumento = TControlTerrestre.sloTransporteAsignado.IdSLOTransporteSolicitud;
                    
                    lstDocumentosGuardados = await sloDocumentosService.sloGetFilesTask(filtroDocumentos);
                    lstTransporteCronDocumentos = await SLOTransporteCronDocumentosService.ObtenerSLOTranporteCronDocumentos(idCron);

                    lstPruebasIncidencia = lstDocumentosGuardados.Where(doc => lstTransporteCronDocumentos
                    .Any(cronDoc => cronDoc.IdSLOSolicitudDocumentos == doc.IdSLOSolicitudDocumentos)).ToList();



                }
                boolEditable = true;
            }
            if(Modo == "C")
            {
                boolEditable = false;
            }

            objTControlTerrestre = TControlTerrestre;
            objTransporteCron.IdSLOTransporteAsignado = objTControlTerrestre.IdSLOTransporteAsignado;
            FechaMinimaBooking = Booking;
            listaTiposEventos = await catTipoEventosCronService.CatTipoEventosCronListar();            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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

        #region FUNCIONES UI       
        private async Task GuardarEventualidad()
        {
            List<string> Validaciones = new();

            objTransporteCron.Activo = true;
            objTransporteCron.FechaRegistro = DateTime.Now;
            objTransporteCron.IdCatUsuarios = UsuarioDTO.IdCatUsuario;

            if (objTransporteCron.FechaEvento <= FechaMinimaBooking)
                Validaciones.Add("Seleccionar <strong>Fecha Evento</strong> no menor o igual a <strong>Confirmación de Booking</strong>");

            if (objTransporteCron.IdCatTipoEventoCron == 0)
                Validaciones.Add("Establecer un <strong>Tipo de Evento</strong>");

            if (objTransporteCron.Comentarios == null || objTransporteCron.Comentarios == "")
                Validaciones.Add("Agregar <strong>Comentarios</strong>");

            if (objTransporteCron.URLMaps == null || objTransporteCron.URLMaps == "")
                Validaciones.Add("Agregar <strong>URLMaps</strong>");


            if (Validaciones.Any())
            {
                // Unimos los errores en un string con saltos de línea
                string mensaje = "<ul style='padding-left: 20px; line-height: 1.6;'>" +
                                 string.Join("", Validaciones.Select(e => $"<li>{e}</li>")) +
                                 "</ul>";
                await sweetAlertService.FireAsync("Datos incompletos o no válidos", mensaje, SweetAlertIcon.Warning);
                return;
            }
            //{
            //    await sweetAlertService.FireAsync("Fecha fuera de operación","La fecha de la eventualidad no puede ser menor a Booking", SweetAlertIcon.Warning);
            //    return;
            //}

            RespuestaGenericaDTO response = await TransporteCronService.SLOTransporteCronCrear(objTransporteCron);

            //foreach (var doc in lstCargarArchivos)
            //{
            //    doc.TipoDocumento = "INCIDENCIA";

            //}

            var json = JsonConvert.SerializeObject(response.Entidad);
            var eventualidad = JsonConvert.DeserializeObject<SLOTransportesCron>(json);

            var lstDocumentosGuardados = await ProcesarDocumentosAsync();

            var lstTransporteCronDocumentos = new List<SLOTransporteCronDocumentos>();
            
            if(lstDocumentosGuardados.Count > 0)
            {
                foreach(var documento in lstDocumentosGuardados)
                {
                    lstTransporteCronDocumentos.Add(new SLOTransporteCronDocumentos
                    {
                        IdSLOTransporteCron = eventualidad.IdSLOTransporteCron,
                        IdSLOSolicitudDocumentos = documento.IdSLOSolicitudDocumentos,
                        Activo = true,
                        IdUsuarioRegistro = UsuarioDTO.IdCatUsuario,
                        FechaRegistro = DateTime.Now
                    });
                }
            }

            var resultado = await SLOTransporteCronDocumentosService.CrearSLOTransporteCronDocumeto(lstTransporteCronDocumentos);

            if (response.IsSuccess)
            {
               await sweetAlertService.FireAsync(
                    "Creado",
                    "La eventualidad ha sido agregada de manera correcta",
                    SweetAlertIcon.Success);
                dialogService.Close(true);
                //NavigationManager.NavigateTo("/listado"); // o a donde necesites
            }
            else
            {
                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = $"No se pudo registrar la eventualidad de forma correcta: {response.lstrErrorMessages}",
                    Duration = 3000
                });
            }
        }

        /// <summary>
        /// Actualiza la eventualidad con los nuevos datos
        /// </summary>
        private async Task ActualizarEventualidad()
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new RespuestaGenericaDTO();

            List<string> Validaciones = new();

            objTransporteCron.Activo = true;
            objTransporteCron.FechaRegistro = DateTime.Now;
            objTransporteCron.IdCatUsuarios = UsuarioDTO.IdCatUsuario;

            if (objTransporteCron.FechaEvento <= FechaMinimaBooking)
                Validaciones.Add("Seleccionar <strong>Fecha Evento</strong> no menor o igual a <strong>Confirmación de Booking</strong>");

            if (objTransporteCron.IdCatTipoEventoCron == 0)
                Validaciones.Add("Establecer un <strong>Tipo de Evento</strong>");

            if (objTransporteCron.Comentarios == null || objTransporteCron.Comentarios == "")
                Validaciones.Add("Agregar <strong>Comentarios</strong>");

            if (objTransporteCron.URLMaps == null || objTransporteCron.URLMaps == "")
                Validaciones.Add("Agregar <strong>URLMaps</strong>");


            if (Validaciones.Any())
            {
                // Unimos los errores en un string con saltos de línea
                string mensaje = "<ul style='padding-left: 20px; line-height: 1.6;'>" +
                                 string.Join("", Validaciones.Select(e => $"<li>{e}</li>")) +
                                 "</ul>";
                await sweetAlertService.FireAsync("Datos incompletos o no válidos", mensaje, SweetAlertIcon.Warning);
                return;
            }

            try
            {
                respuestaGenericaDTO = await TransporteCronService.ActualizarSLOTransporteCron(objTransporteCron);
                
                if (respuestaGenericaDTO.IsSuccess)
                {
                    await sweetAlertService.FireAsync("Exito", "Eventualidad Actualizada Correctamente", SweetAlertIcon.Success);
                    dialogService.Close(true);
                }
                else
                {
                    await sweetAlertService.FireAsync("Error", "Ocurrió un error al actualizar la Eventudalidad, valide la información.", SweetAlertIcon.Error);
                }
            }
            catch (Exception ex) 
            {
                await sweetAlertService.FireAsync("Error", "Ocurrió un error al actualizar la Eventualidad", SweetAlertIcon.Error);
            }

        }

        private void OnCancel()
        {
            dialogService.Close(false);
        }

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
                    return !(contentType == "application/pdf" || ext == ".pdf" ||
         contentType.StartsWith("image/") ||
         ext == ".jpg" || ext == ".jpeg" || ext == ".png");

                })
                .ToList();

            if (tiposInvalidos.Any())
                errores.Add($"Los siguientes archivos no son validos: {string.Join(", ", tiposInvalidos.Select(x => x.Name))}");

            // Tamaño por archivo
            var grandes = files.Where(f => f.Size > MaxFileSize).ToList();
            if (grandes.Any())
                errores.Add($"Los siguientes archivos exceden {MaxFileSize / 1024 / 1024} MB: {string.Join(", ", grandes.Select(x => x.Name))}");

            if (errores.Any())
            {
                // Mostrar todos los errores juntos (puedes usar SweetAlert o NotificationService)
                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Validación fallida",
                    Detail = string.Join(" / ", errores),
                    Duration = 6000
                });

                limpiarPendiente = true; // limpiar fuera del evento
                return;
            }

            // Abrir modal para clasificar los archivos
            var result = await dialogService.OpenAsync<UltimaMillaModalDocumentos>(
                "Clasificar Documentos",
                new Dictionary<string, object>() { { "Archivos", files }, { "Modo", "INCIDENCIA" } },
                new DialogOptions()
                { Width = "60%", Height = "55%", Resizable = true, Draggable = true, ShowClose = false, Style = "border-radius: 12px;"}
            );

            // Procesar resultados del modal
            if (result is List<UltimaMillaModalDocumentos.DocumentoUploadItem> listaFinal && listaFinal.Any())
            {
                try
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
                            //Identificador = objTControlTerrestre.sloTransporteAsignado.IdSLOTransporteAsignado.ToString(),
                            Identificador = objTControlTerrestre.sloTransporteAsignado.IdSLOTransporteSolicitud.ToString(),
                            NombreArchivo = doc.FileInfo?.Name,
                            SizeFile = doc.FileInfo?.Size ?? 0,
                            ContentType = doc.FileInfo?.ContentType,
                            FileBytes = fileBytes,
                            IdOrden = objTControlTerrestre.sloSolicitudes.IdOrden,
                            IdUsuario = UsuarioDTO.IdCatUsuario
                        });
                    }

                }
                catch (Exception ex)
                {
                    return;
                }

                if(Modo == "E")
                {
                    var lstDocumentosGuardados = await ProcesarDocumentosAsync();

                    var lstTransporteCronDocumentos = new List<SLOTransporteCronDocumentos>();

                    if (lstDocumentosGuardados.Count > 0)
                    {
                        foreach (var documento in lstDocumentosGuardados)
                        {
                            lstTransporteCronDocumentos.Add(new SLOTransporteCronDocumentos
                            {
                                IdSLOTransporteCron = idCron,
                                IdSLOSolicitudDocumentos = documento.IdSLOSolicitudDocumentos,
                                Activo = true,
                                IdUsuarioRegistro = UsuarioDTO.IdCatUsuario,
                                FechaRegistro = DateTime.Now
                            });
                        }
                    }

                    var resultado = await SLOTransporteCronDocumentosService.CrearSLOTransporteCronDocumeto(lstTransporteCronDocumentos);

                    lstDocumentosGuardados = await sloDocumentosService.sloGetFilesTask(filtroDocumentos);
                    lstTransporteCronDocumentos = await SLOTransporteCronDocumentosService.ObtenerSLOTranporteCronDocumentos(idCron);
                    lstPruebasIncidencia = new();
                    lstPruebasIncidencia = lstDocumentosGuardados.Where(doc => lstTransporteCronDocumentos
                    .Any(cronDoc => cronDoc.IdSLOSolicitudDocumentos == doc.IdSLOSolicitudDocumentos)).ToList();
                    await gridArchivosIncidencias.Reload();

                }
                // Limpiar selección del upload
                limpiarPendiente = true;
            }
            limpiarPendiente = true;
        }

        private async Task MostrarAlertaLimiteCantidad()
        {
            // Si usas SweetAlertService:
            // await SweetAlertService.FireAsync("Límite superado", $"No puedes cargar más de {MaxCountFiles} archivos.", SweetAlertIcon.Warning);

            // O usar NotificationService:
            notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Warning,
                Summary = "Cantidad máxima superada",
                Detail = $"No puedes cargar más de {MaxCountFiles} archivos a la vez.",
                Duration = 4000
            });
        }

        private async Task<List<SLOSolicitudesDocumentos>> ProcesarDocumentosAsync()
        {
            lstDocumentosGuardados = new();     

            foreach (var archiCarga in lstCargarArchivos)
            {
                try
                {
                    //archiCarga.TipoDocumento = "INCIDENCIA";

                    RespuestaGenericaDTO respon = await sloDocumentosService.SLOUploadFile(archiCarga);

                    if (respon.IsSuccess)
                    {
                        var json = JsonConvert.SerializeObject(respon.Entidad);
                        var documento = JsonConvert.DeserializeObject<SLOSolicitudesDocumentos>(json);

                        lstDocumentosGuardados.Add(documento);
                    }
                        
                    
                    if (!respon.IsSuccess)
                    {
                        await sweetAlertService.FireAsync("Error",
                            $"No se pudo cargar el archivo {archiCarga.NombreArchivo}.", SweetAlertIcon.Error);
                        break; // detener en el primer error
                    }

                    
                }
                catch (Exception ex)
                {
                    await sweetAlertService.CloseAsync();
                    await sweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                    return lstDocumentosGuardados;
                }                
            }

            return lstDocumentosGuardados;
        }

        private async Task EliminarDocumento(SLOCargarArchivo doc)
        {
            lstCargarArchivos.Remove(doc);
            await documentosGrid.Reload();
        }

        private async Task AbrirDocumentoCont(SLOSolicitudesDocumentos doc)
        {
            var response = await Http.GetAsync($"{Inicializar.UrlApiLogistico}SLODocumentos/obtenerArchivo/{doc.DocumentoUUID}");

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var base64 = Convert.ToBase64String(fileBytes);
                //var fileUrl = $"data:application/pdf;base64,{base64}";

                // 👇 Obtenemos el tipo real del archivo (ej: application/pdf, image/png, image/jpeg)
                //var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

                //await JS.InvokeVoidAsync("abrirPDFenNuevaPestana", base64, "application/pdf");
                await JS.InvokeVoidAsync("abrirPDFenNuevaPestana", base64, doc.TipoArchivo.ToString());
                //await _jsRuntime.InvokeVoidAsync("saveAsFile", nombreArchivo, Convert.ToBase64String(contenido));


                // Abre el PDF en una nueva pestaña
                //await JS.InvokeVoidAsync("open", fileUrl, "_blank");
            }
            else
            {
                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "No se pudo abrir el documento.",
                    Duration = 4000
                });
            }
        }

        private async Task BajaDocumento(SLOSolicitudesDocumentos item)
        {
            // Mostrar confirmación
            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmar",
                Text = $"¿Seguro que desea dar de baja el documento '{item.NombreDocumento}'? Los cambios serán guardados.",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí, eliminar",
                CancelButtonText = "Cancelar"
            });

            // Solo continuar si el usuario confirmó
            if (result.IsConfirmed)
            {
                var respuestaGenericaDto = await sloDocumentosService.BajaDocumento(item);

                if (respuestaGenericaDto.IsSuccess)
                {
                    await sweetAlertService.FireAsync("Baja", "Documento dado de baja correctamente", SweetAlertIcon.Success);
                    // Opcional: refrescar lista de documentos
                    var filtro =  new FiltroGenericoDTO
                    {
                        Id = TControlTerrestre.sloSolicitudes.IdSLOSolicitud,
                        IdTipoDocumento = TControlTerrestre.sloTransporteAsignado.IdSLOTransporteSolicitud
                    };
                    lstDocumentosGuardados = await sloDocumentosService.sloGetFilesTask(filtro);
                    lstTransporteCronDocumentos = await SLOTransporteCronDocumentosService.ObtenerSLOTranporteCronDocumentos(idCron);
                    
                    lstPruebasIncidencia = lstDocumentosGuardados.Where(doc => lstTransporteCronDocumentos
                    .Any(cronDoc => cronDoc.IdSLOSolicitudDocumentos == doc.IdSLOSolicitudDocumentos)).ToList();

                    await gridArchivosIncidencias.Reload();
                }
                else
                {
                    await sweetAlertService.FireAsync("Error", "Error al dar de baja el documento.", SweetAlertIcon.Error);
                }
            }
            else
            {
                // El usuario canceló
                //await sweetAlertService.FireAsync("Cancelado", "No se eliminó el documento.", SweetAlertIcon.Info);
            }
        }
        private void HacerEditableEventualidad(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            boolEditable = !boolEditable;
            StateHasChanged();
        }
        #endregion

    }
}
