using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaTimeLineAgregarTraking
    {
        [Parameter] public SLOTControlTerrestre TControlTerrestre { get; set; }
        [Parameter] public UsuarioTokenDTO UsuarioDTO { get; set; }
        [Parameter] public DateTime? Booking { get; set; }
        [Inject] private ISLOTControlTerrestreService servicioControl { get; set; }
        [Inject] private ICatTipoEventosCronService catTipoEventosCronService { get; set; }
        [Inject] private DialogService dialogService { get; set; }
        [Inject] private ISLOTransporteCronService TransporteCronService { get; set; }
        [Inject] private SweetAlertService sweetAlertService { get; set; }
        [Inject] ISLODocumentosService sloDocumentosService { get; set; }




        private SLOTControlTerrestre objTControlTerrestre = new();
        private SLOTransportesCron transporteCron = new SLOTransportesCron();
        private ICollection<CatTipoEventosCron> listaTiposEventos;
        private RadzenUpload uploadFiles;
        private List<SLOCargarArchivo> lstCargarArchivos = new List<SLOCargarArchivo>();
        private RadzenDataGrid<SLOCargarArchivo> documentosGrid;
        public DateTime? FechaMinimaBooking;
        protected override async Task OnInitializedAsync()
        {
            objTControlTerrestre = TControlTerrestre;
            listaTiposEventos = await catTipoEventosCronService.CatTipoEventosCronListar();
            transporteCron.IdSLOTransporteAsignado = objTControlTerrestre.IdSLOTransporteAsignado;
            FechaMinimaBooking = Booking;
        }

        private async Task Guardar()
        {
            List<string> Validaciones = new();

            transporteCron.Activo = true;
            transporteCron.FechaRegistro = DateTime.Now;
            transporteCron.IdCatUsuarios = UsuarioDTO.IdCatUsuario;

            if (transporteCron.FechaEvento < FechaMinimaBooking)
                Validaciones.Add("Seleccionar <strong>Fecha Evento</strong> no menor a <strong>Confirmación de Booking</strong>");

            if (transporteCron.IdCatTipoEventoCron == 0)
                Validaciones.Add("Establecer un <strong>Tipo de Evento</strong>");

            if (transporteCron.Comentarios == null || transporteCron.Comentarios == "")
                Validaciones.Add("Agregar <strong>Comentarios</strong>");

            if (transporteCron.URLMaps == null || transporteCron.URLMaps == "")
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

            var response = await TransporteCronService.SLOTransporteCronCrear(transporteCron);

            //foreach (var doc in lstCargarArchivos)
            //{
            //    doc.TipoDocumento = "INCIDENCIA";

            //}

            await ProcesarDocumentosAsync();
            if (response.IsSuccess)
            {
                sweetAlertService.FireAsync(
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

        private void OnCancel()
        {
            dialogService.Close(false);
        }

        private async Task OnUploadChange(UploadChangeEventArgs args)
        {
            var files = args.Files?.ToList();
            if (files == null || !files.Any())
                return;

            // Abrir modal para clasificar los archivos
            var result = await dialogService.OpenAsync<UltimaMillaModalDocumentos>(
                "Clasificar Documentos",
                new Dictionary<string, object>() { { "Archivos", files }, {"Modo", "INCIDENCIA"} },
                new DialogOptions()
                    { Width = "40%", Height = "30%", Resizable = true, Draggable = true, ShowClose = false }
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
               

                // Limpiar selección del upload
                await uploadFiles.ClearFiles();
            }
        }

        private async Task ProcesarDocumentosAsync()
        {

            foreach (var archiCarga in lstCargarArchivos)
            {
                try
                {
                    //archiCarga.TipoDocumento = "INCIDENCIA";

                    bool respon = await sloDocumentosService.SLOUploadFile(archiCarga);

                    if (!respon)
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
                    break;
                }

            }
        }

        private async Task EliminarDocumento(SLOCargarArchivo doc)
        {
            lstCargarArchivos.Remove(doc);
            await documentosGrid.Reload();
        }
    }
}
