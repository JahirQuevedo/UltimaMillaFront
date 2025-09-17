using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using ClienteBlazorWASM.Helpers;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AlogisticsWASM.Pages.Vacios.Solicitudes.SolicitudesCRUDCMP;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaTimeLineControlPageRZ
    {
        #region PARAMETROS
        [Parameter] public List<SLOTControlTerrestre> TControlTerrestre { get; set; }
        [Parameter] public UsuarioTokenDTO UsuarioDTO { get; set; }
        #endregion

        #region SERVICIOS
        [Inject] private SweetAlertService sweetAlertService { get; set; }
        [Inject] private ISLOTControlTerrestreService sloTorreControlTerrestreService { get; set; }
        [Inject] private ISLOTransporteCronService sloTransporteCronService { get; set; }
        [Inject] private ISLOSolicitudesService SLOSolicitudesService { get; set; }
        [Inject] private ISLOTransporteSolicitudService SLOTransporteSolicitudService { get; set; }
        [Inject] private ILoginService _loginService { get; set; }
        [Inject] private ICatTipoEstadoService tipoEstadoService { get; set; }
        [Inject] private NotificationService notificationService { get; set; }
        [Inject] private ISLODocumentosService sloDocumentosService { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private HttpClient Http { get; set; }
        [Inject] private DialogService dialogService { get; set; }        
        #endregion

        #region MODELOS DTO
        public class Evento
        {
            public string Titulo { get; set; }
            public string Detalle { get; set; }
            public string Ubicacion { get; set; }
            public DateTime Fecha { get; set; }
            public bool EsEventual { get; set; }
            public string RecibidoPor { get; set; }
            public string Comentarios { get; set; }
            public string EstadoUnidad { get; set; }
            public string ClaveEvento { get; set; }
            public string ResponsableRegistro { get; set; }
            public DateTime FechaRegistro { get; set; } = DateTime.Now;
            public SegmentoViaje Segmento { get; set; }
        }
        public class CardModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public bool MostrarGrid { get; set; }
        }
        public class TimelineEvent
        {
            public DateTime Fecha { get; set; }
            public string Titulo { get; set; }
            public string Tipo { get; set; }
        }
        public enum SegmentoViaje
        {
            Origen,
            TransitoMaritimoAereo,
            UltimaMilla
        }

        public class ScreenSize
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
        #endregion

        #region OBJETOS Y LISTAS
        private SLOTControlTerrestre objCotControlTerrestre { get; set; } = new SLOTControlTerrestre();
        private SLOTControlTerrestre objTControlTerrestre { get; set; } = new();
        private SLOTControlTerrestre objTControlTerrestreModificable;
        private List<SLOTransporteDetalle> lstDetallesAsignados = new List<SLOTransporteDetalle>();
        private List<SLOTransportesCron> lstTransportesCrons = new List<SLOTransportesCron>();
        private List<TimelineEvent> lstEventos = new();
        private ICollection<CatTipoEstados> lstTipoEstados;
        private Orientation orientation = Orientation.Vertical;
        private LinePosition position = LinePosition.Center;
        private bool reverse;

        private SegmentoViaje segmentoActual = SegmentoViaje.Origen;
        private int? selectedCardId = null;
        private Evento? eventualidadSeleccionada;
        private List<CardModel> cards = new();
        private List<SLOSolicitudesDocumentos> lstDocumentos = null;
        private RadzenDataGrid<SLOSolicitudesDocumentos> gridDocumentos;
        private RadzenDataGrid<SLOSolicitudesDocumentos> gridArchivos;
        private RadzenUpload uploadFiles;
        private List<SLOCargarArchivo> lstCargarArchivos = new List<SLOCargarArchivo>();
        private RadzenDataGrid<SLOCargarArchivo> documentosGrid;

        private int idCatEstadoTerminado;
        private int? idEstadoActual;

        private int ancho;
        private int alto;
        private string zoomType;
        #endregion

        #region INICIALIZAR
        protected override async Task OnInitializedAsync()
        {

            InicializarCards();

            if (cards?.Any() == true)
            {
                selectedCardId = cards[0].Id;
            }

            lstTipoEstados = await tipoEstadoService.GetTiposEstado();
            
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
        }

        private void InicializarCards()
        {
            cards = new List<CardModel>
            {
                new CardModel
                {
                    Id = 1,
                    Title = "Transporte Solicitud: " + TControlTerrestre.Select(a => a.sloSolicitudes?.Orden.ReferenciaALO).FirstOrDefault(),
                    MostrarGrid = true
                }
            };
        }
        #endregion

        #region FUNCIONES UI

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

        #region CARD IZQUIERDA
        private async Task BotonClickeado(SLOTControlTerrestre item)
        {

            idCatEstadoTerminado = lstTipoEstados
            .Where(d => d.TipoEstado == "T")
            .Select(d => d.IdCatTipoEstados)
            .FirstOrDefault();
            
            Console.WriteLine($"Botón clickeado para placa: {item.sloTransporteAsignado}");
            objCotControlTerrestre = item;
            try
            {
                if (objCotControlTerrestre != null)
                {
                                        

                    objTControlTerrestreModificable = new()
                    {
                        FConfirmaBooking = objCotControlTerrestre.FConfirmaBooking,
                        FUnidensitiocarga = objCotControlTerrestre.FUnidensitiocarga,
                        FUnidadencarga = objCotControlTerrestre.FUnidadencarga,
                        FFinalizaCarga = objCotControlTerrestre.FFinalizaCarga,
                        FIniciaTransito = objCotControlTerrestre.FIniciaTransito,
                        FEnFrontera = objCotControlTerrestre.FEnFrontera,
                        FInicioDespachoAA = objCotControlTerrestre.FInicioDespachoAA,
                        FFinDespachoAA = objCotControlTerrestre.FFinDespachoAA,
                        FIniciaTransitoEXPO = objCotControlTerrestre.FIniciaTransitoEXPO,
                        FPuntoDescarga = objCotControlTerrestre.FPuntoDescarga,
                        FEnProcesoDescarga = objCotControlTerrestre.FEnProcesoDescarga,
                        FFinDescarga = objCotControlTerrestre.FFinDescarga,
                        FRecepcionPOD = objCotControlTerrestre.FRecepcionPOD,
                        FFinOperacion = objCotControlTerrestre.FFinOperacion
                    };

                    objTControlTerrestre = objCotControlTerrestre;
                    //objTControlTerrestreModificable = objTControlTerrestre;
                    lstDetallesAsignados = item.sloTransporteAsignado
                        .sloTransporteSolicitud
                        .sloTransporteDetalle
                        .ToList();

                    lstDocumentos = await sloDocumentosService.sloGetFilesTask(item.sloTransporteAsignado.sloTransporteSolicitud.IdSLOTransporteSolicitud);
                    Console.WriteLine($"Registros cargados: {lstDocumentos?.Count ?? 0}");
                    await ConstruirEventos(); // Separa la lógica en un método
                    InvokeAsync(StateHasChanged); // Fuerza la actualización del UI
                    //await gridDocumentos.Reload();
                    idEstadoActual = objCotControlTerrestre.sloTransporteAsignado.sloTransporteSolicitud.IdCatTipoEstados;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            
        }
        private void SelectCard(int cardId)
        {
            selectedCardId = cardId;
            segmentoActual = cardId switch
            {
                1 => SegmentoViaje.Origen,
                2 => SegmentoViaje.TransitoMaritimoAereo,
                3 => SegmentoViaje.UltimaMilla,
                _ => SegmentoViaje.Origen
            };
        }

        private int indiceActual = 0;
        private Timer? timer;

        public void Dispose()
        {
            timer?.Dispose();
        }
        private async Task VerDetalle(string modo, SLOTControlTerrestre data)
        {
            var response = await DialogService.OpenAsync<UltimaMillaTranPageRZ>(
                $"Detalles de solicitud de Transporte {data.sloSolicitudes.Orden.ReferenciaALO}",
                new Dictionary<string, object>
                {
                    {"SolicitudTransporte", data.sloTransporteAsignado.sloTransporteSolicitud},
                    {"Modo",modo},
                    {"Solicitud",data.sloSolicitudes},
                    {"TransporteAsignado", data.sloTransporteAsignado}
                    //{" UsuarioToken", await _loginService.ObtenerdatosToken()}

                }, new DialogOptions
                {
                    Width = "70%",
                    Height = "90%",
                    Resizable = true,
                    Draggable = true,
                    Style = "border-radius: 12px;"
                });
        }
        #endregion

        #region CARD CENTRAL

        /// <summary>
        /// Formatea una cadena de fecha en español (ej: "lunes 1 de enero de 2024").
        /// </summary>
        private string FormatearFecha(string fechaString)
        {
            if (DateTime.TryParse(fechaString, out DateTime fecha))
            {
                var culture = new System.Globalization.CultureInfo("es-ES");
                return culture.TextInfo.ToTitleCase(
                    fecha.ToString("dddd d 'de' MMMM 'de' yyyy", culture));
            }
            return "Fecha inválida";
        }

        /// <summary>
        /// Construye la lista de eventos del timeline (fechas fijas + eventualidades).
        /// </summary>
        private async Task ConstruirEventos()
        {
            lstTransportesCrons = await sloTransporteCronService
                .SLOTransporteCronListar(objCotControlTerrestre.IdSLOTransporteAsignado ?? 0);
            //lstTransportesCrons = new List<SLOTransportesCron>();
            var eventos = new List<TimelineEvent>();

            void AgregarEvento(DateTime? fecha, string titulo)
            {
                if (fecha.HasValue)
                {
                    eventos.Add(new TimelineEvent
                    {
                        Fecha = fecha.Value,
                        Titulo = titulo,
                        Tipo = "Fijo"
                    });
                }
            }

            // Fechas fijas
            AgregarEvento(objTControlTerrestre?.FConfirmaBooking, "Confirmación de Booking");
            AgregarEvento(objTControlTerrestre?.FUnidensitiocarga, "Unidad en sitio de carga");
            AgregarEvento(objTControlTerrestre?.FUnidadencarga, "Unidad en proceso de carga");
            AgregarEvento(objTControlTerrestre?.FFinalizaCarga, "Finalización de carga");
            AgregarEvento(objTControlTerrestre?.FIniciaTransito, "Inicia tránsito a destino");
            AgregarEvento(objTControlTerrestre?.FEnFrontera, "Unidad en frontera Mexicana");
            AgregarEvento(objTControlTerrestre?.FInicioDespachoAA, "Inicio del despacho Aduanal");
            AgregarEvento(objTControlTerrestre?.FFinDespachoAA, "Fin del despacho Aduanal");
            AgregarEvento(objTControlTerrestre?.FIniciaTransitoEXPO, "Inicia tránsito de Exportación");
            AgregarEvento(objTControlTerrestre?.FPuntoDescarga, "Llegada a punto de descarga");
            AgregarEvento(objTControlTerrestre?.FEnProcesoDescarga, "En proceso de descarga");
            AgregarEvento(objTControlTerrestre?.FFinDescarga, "Fin de descarga");
            AgregarEvento(objTControlTerrestre?.FRecepcionPOD, "Recepción POD");
            AgregarEvento(objTControlTerrestre?.FFinOperacion, "Fin de la operación");

            // Eventualidades
            if (lstTransportesCrons != null)
            {
                eventos.AddRange(lstTransportesCrons.Select(e => new TimelineEvent
                {
                    Fecha = e.FechaEvento,
                    Titulo = e.catTipoEventosCron?.Nombre ?? "Evento",
                    Tipo = "Eventualidad"
                }));
            }

            lstEventos = eventos.OrderBy(e => e.Fecha).ToList();
        }

        /// <summary>
        /// Valida las fechas de control terrestre.
        /// - Se permite que cualquier fecha esté vacía.
        /// - Si dos fechas consecutivas están rellenas, la segunda debe ser al menos 1 minuto mayor que la primera.
        /// - Los eventos “eventualidades” deben estar dentro del rango mínimo-máximo de las fechas definidas.
        /// </summary>
        private IEnumerable<string> ValidateControlTerrestre()
        {
            var errores = new List<string>();

            // 1) Declara tu arreglo de tuplas CON NOMBRES (Nombre, Fecha)
            var fechasOrdenadas = new (string Nombre, DateTime? Fecha)[]
            {
        ("Confirmación de Booking",       objTControlTerrestre?.FConfirmaBooking),
        ("Unidad en sitio de carga",      objTControlTerrestre?.FUnidensitiocarga),
        ("Unidad en proceso de carga",    objTControlTerrestre?.FUnidadencarga),
        ("Finalización de carga",         objTControlTerrestre?.FFinalizaCarga),
        ("Inicia tránsito a destino (mex)",   objTControlTerrestre?.FIniciaTransito),
        ("Unidad en frontera mexicana para exportación", objTControlTerrestre?.FEnFrontera),
        ("Inicio de despacho aduanal",    objTControlTerrestre?.FInicioDespachoAA),
        ("Fin de proceso de despacho aduanal",   objTControlTerrestre?.FFinDespachoAA),
        ("Inicia tránsito a destino (usa)",     objTControlTerrestre?.FIniciaTransitoEXPO),
        ("Punto de descarga",            objTControlTerrestre?.FPuntoDescarga),
        ("En proceso de descarga",       objTControlTerrestre?.FEnProcesoDescarga),
        ("Finalización de descarga",     objTControlTerrestre?.FFinDescarga),
        ("Recepción POD",                objTControlTerrestre?.FRecepcionPOD),
        ("Fin de la operación",          objTControlTerrestre?.FFinOperacion)
            };

            // 2) Validación secuencial: cada fecha definida debe ser >= (al menos 1 min mayor) que la anterior definida
            DateTime? ultimaFecha = null;
            string nombreUltima = null;

            foreach (var (nombre, fecha) in fechasOrdenadas)
            {
                if (!fecha.HasValue) continue;

                // Regla: fecha actual debe ser >= última + 1 minuto
                // Error si es menor que (última + 1 minuto). Igual a +1 minuto SÍ es válido.
                if (ultimaFecha.HasValue && fecha.Value < ultimaFecha.Value.AddMinutes(1))
                {
                    errores.Add($"La fecha '{nombre}' debe ser mayor a '{nombreUltima}'.");
                }

                ultimaFecha = fecha;
                nombreUltima = nombre;
            }

            // 3) Validar eventualidades dentro del rango [min, max] de las fechas definidas
            if (lstTransportesCrons != null && lstTransportesCrons.Any())
            {
                var fechasDefinidas = fechasOrdenadas
                    .Where(f => f.Fecha.HasValue)
                    .Select(f => f.Fecha!.Value)   // ya filtramos HasValue, se puede usar !
                    .ToList();

                if (fechasDefinidas.Any())
                {
                    var minFecha = fechasDefinidas.Min();
                    var maxFecha = fechasDefinidas.Max();

                    foreach (var cron in lstTransportesCrons)
                    {
                        // Si tu regla es que un evento sin fecha viene como DateTime.MinValue
                        if (cron.FechaEvento == DateTime.MinValue) continue;

                        if (cron.FechaEvento < minFecha || cron.FechaEvento > maxFecha)
                        {
                            errores.Add(
                                $"El evento '{cron.catTipoEventosCron?.Nombre ?? "Evento"}' " +
                                $"con fecha {cron.FechaEvento:dd/MM/yyyy HH:mm} " +
                                $"está fuera del rango de fechas de la operación."
                            );
                        }                        
                    }

                }
            }

            return errores;
        }




        /// <summary>
        /// Guarda los cambios en el timeline. Si hay errores, muestra notificaciones y no guarda.
        /// </summary>
        private async Task GuardarCambiosTimeLine()
        {

            objTControlTerrestre.FConfirmaBooking = objTControlTerrestreModificable.FConfirmaBooking;
            objTControlTerrestre.FUnidensitiocarga = objTControlTerrestreModificable.FUnidensitiocarga;
            objTControlTerrestre.FUnidadencarga = objTControlTerrestreModificable.FUnidadencarga;
            objTControlTerrestre.FFinalizaCarga = objTControlTerrestreModificable.FFinalizaCarga;
            objTControlTerrestre.FIniciaTransito = objTControlTerrestreModificable.FIniciaTransito;
            objTControlTerrestre.FEnFrontera = objTControlTerrestreModificable.FEnFrontera;
            objTControlTerrestre.FInicioDespachoAA = objTControlTerrestreModificable.FInicioDespachoAA;
            objTControlTerrestre.FFinDespachoAA = objTControlTerrestreModificable.FFinDespachoAA;
            objTControlTerrestre.FIniciaTransitoEXPO = objTControlTerrestreModificable.FIniciaTransitoEXPO;
            objTControlTerrestre.FPuntoDescarga = objTControlTerrestreModificable.FPuntoDescarga;
            objTControlTerrestre.FEnProcesoDescarga = objTControlTerrestreModificable.FEnProcesoDescarga;
            objTControlTerrestre.FFinDescarga = objTControlTerrestreModificable.FFinDescarga;
            objTControlTerrestre.FRecepcionPOD = objTControlTerrestreModificable.FRecepcionPOD;
            objTControlTerrestre.FFinOperacion = objTControlTerrestreModificable.FFinOperacion;

            List<string> errores;
            errores = (List<string>)ValidateControlTerrestre();

            if (objCotControlTerrestre.FConfirmaBooking == null)
                errores.Add("No se ha establecido una fecha de <strong>Confirmación de Booking</strong>");

            if (errores.Any())
            {
                // Unimos los errores en un string con saltos de línea
                string mensaje = "<ul style='padding-left: 20px; line-height: 1.6;'>" +
                                 string.Join("", errores.Select(e => $"<li>{e}</li>")) +
                                 "</ul>";
                await sweetAlertService.FireAsync("Datos incompletos", mensaje, SweetAlertIcon.Warning);
                return;                
            }

            

            var response = await sloTorreControlTerrestreService.ActualizarTControlTerrestre(objTControlTerrestre);

            if (response.IsSuccess)
            {
                await sweetAlertService.FireAsync(
                    "Éxito",
                    "Los cambios han sido guardados de forma correcta.",
                    SweetAlertIcon.Success);
                await ConstruirEventos();
                StateHasChanged();
            }
            else
            {
                await sweetAlertService.FireAsync(
                    "Error",
                    "Los cambios no han sido guardados de forma correcta.",
                    SweetAlertIcon.Error);
            }
        }

        /// <summary>
        /// Se ejecuta cuando el usuario cambia una fecha en el formulario.
        /// Realiza la validación y muestra los errores (si existen).
        /// </summary>
        private void OnDateChanged()
        {
            var errores = ValidateControlTerrestre();

            foreach (var error in errores)
            {
                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Fecha inválida",
                    Detail = error,
                    Duration = 10000
                });
            }

            StateHasChanged();
        }

        /// <summary>
        /// Mapea el índice de la tabla a la propiedad correspondiente del modelo.
        /// </summary>
        private string GetPropertyName(int index)
        {
            return index switch
            {
                0 => nameof(objTControlTerrestre.FConfirmaBooking),
                1 => nameof(objTControlTerrestre.FUnidensitiocarga),
                2 => nameof(objTControlTerrestre.FUnidadencarga),
                3 => nameof(objTControlTerrestre.FFinalizaCarga),
                4 => nameof(objTControlTerrestre.FIniciaTransito),
                5 => nameof(objTControlTerrestre.FEnFrontera),
                6 => nameof(objTControlTerrestre.FInicioDespachoAA),
                7 => nameof(objTControlTerrestre.FFinDespachoAA),
                8 => nameof(objTControlTerrestre.FIniciaTransitoEXPO),
                9 => nameof(objTControlTerrestre.FPuntoDescarga),
                10 => nameof(objTControlTerrestre.FEnProcesoDescarga),
                11 => nameof(objTControlTerrestre.FFinDescarga),
                12 => nameof(objTControlTerrestre.FRecepcionPOD),
                13 => nameof(objTControlTerrestre.FFinOperacion),
                _ => string.Empty
            };
        }

        /// <summary>
        /// Abre la ventana modal para finalizar operación.
        /// </summary>
        private async Task FinalizarOperacion()
        {
            List<string> Validaciones = new();
            Validaciones = (List<string>)ValidateControlTerrestre();

            // Validaciones de campos de fecha
            if (objTControlTerrestre.FConfirmaBooking == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Confirmación de Booking</strong>");

            if (objTControlTerrestre.FFinDespachoAA == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Fin de proceso de despacho aduanal</strong>");

            if (objTControlTerrestre.FUnidensitiocarga == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Unidad en Sitio de Carga</strong>");

            if (objTControlTerrestre.FIniciaTransitoEXPO == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Unidad inicia tránsito a destino (lado estadounidense)</strong>");

            if (objTControlTerrestre.FUnidadencarga == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Unidad en Proceso de Carga</strong>");

            if (objTControlTerrestre.FPuntoDescarga == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Unidad en punto de descarga</strong>");

            if (objTControlTerrestre.FFinalizaCarga == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Finalización de Carga</strong>");

            if (objTControlTerrestre.FEnProcesoDescarga == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Unidad en proceso de descarga</strong>");

            if (objTControlTerrestre.FIniciaTransito == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Unidad inicia tránsito a destino (lado mexicano)</strong>");

            if (objTControlTerrestre.FFinDescarga == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Finalización de descarga</strong>");

            if (objTControlTerrestre.FEnFrontera == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Unidad en frontera mexicana para exportación</strong>");

            if (objTControlTerrestre.FRecepcionPOD == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Prueba de entrega</strong>");

            if (objTControlTerrestreModificable.FInicioDespachoAA == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Inicio de proceso de despacho aduanal</strong>");

            if (objTControlTerrestre.FFinOperacion == null)
                Validaciones.Add("No se ha establecido una fecha de <strong>Fin de la operación</strong>");

            var idPOD = lstDocumentos
            .Where(d => d.catDocumentos.Acronimo == "POD")
            .Select(d => d.catDocumentos.IdCatDocumento)
            .FirstOrDefault();

            if (!lstDocumentos.Any(d => d.IdCatDocumento == idPOD))
            {
                Validaciones.Add("Para finalizar la operación es necesario subir <strong>Prueba de Entrega</strong>");
            }



            if (Validaciones.Any())
            {
                // Unimos los errores en un string con saltos de línea
                string mensaje = "<ul style='padding-left: 20px; line-height: 1.6;'>" +
                                 string.Join("", Validaciones.Select(e => $"<li>{e}</li>")) +
                                 "</ul>";
                await sweetAlertService.FireAsync("Datos incompletos", mensaje, SweetAlertIcon.Warning);
                return;
            }

            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Finalizar proceso",
                Text = "Está a punto de finalizar el proceso",
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí, finalizar",
                CancelButtonText = "Cancelar"
            });

            bool decision = result.IsConfirmed;

            if (decision)
            {
                RespuestaGenericaDTO response = await SLOTransporteSolicitudService.SLOTransporteSolicitudFinalizar(objCotControlTerrestre.sloTransporteAsignado.IdSLOTransporteSolicitud);


                if (response.IsSuccess)
                {
                    await sweetAlertService.FireAsync("Solicitud Finalizada", "La Solicitud de Transporte ha sido finalizada correctamente", SweetAlertIcon.Success);

                    int idSolicitud = objCotControlTerrestre.IdSLOSolicitud;
                    dialogService.Close(true);
                                        
                }

            }

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
                Text = $"¿Seguro que desea dar de baja el documento '{item.NombreDocumento}'?",
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
                    lstDocumentos = await sloDocumentosService.sloGetFilesTask(objTControlTerrestre.sloTransporteAsignado.sloTransporteSolicitud.IdSLOTransporteSolicitud);
                    await gridArchivos.Reload();
                }
                else
                {
                    await sweetAlertService.FireAsync("Error", "Error al dar de baja el documento.", SweetAlertIcon.Error);
                }
            }
            else
            {
                // El usuario canceló
                await sweetAlertService.FireAsync("Cancelado", "No se eliminó el documento.", SweetAlertIcon.Info);
            }
        }

        private async Task OnUploadChange(UploadChangeEventArgs args)
        {
            var files = args.Files?.ToList();
            if (files == null || !files.Any())
                return;

            // Abrir modal para clasificar los archivos
            var result = await dialogService.OpenAsync<UltimaMillaModalDocumentos>(
                "Clasificar Documentos",
                new Dictionary<string, object>() { { "Archivos", files }, { "Modo", "TIMELINE" } },
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
                            Identificador = objTControlTerrestre.sloTransporteAsignado.Placas,
                            NombreArchivo = doc.FileInfo?.Name,
                            SizeFile = doc.FileInfo?.Size ?? 0,
                            ContentType = doc.FileInfo?.ContentType,
                            FileBytes = fileBytes,
                            IdOrden = objTControlTerrestre.sloSolicitudes.IdOrden,
                            IdUsuario = UsuarioDTO.IdCatUsuario
                        });
                    }
                    var lista = lstCargarArchivos;
                }
                catch (Exception ex)
                {
                    return;
                }


                // Limpiar selección del upload
                await uploadFiles.ClearFiles();
            }
        }

        private async Task EliminarDocumento(SLOCargarArchivo doc)
        {
            lstCargarArchivos.Remove(doc);
            await documentosGrid.Reload();
        }

        //Guardar Documentos
        private async Task ProcesarDocumentosAsync()
        {
            if (lstCargarArchivos == null || !lstCargarArchivos.Any())
            {
                await sweetAlertService.FireAsync("Validación", "No hay documentos para procesar.", SweetAlertIcon.Warning);
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
                    //archiCarga.IdOrden = objSolicitudes.IdOrden;
                    //archiCarga.IdUsuario = UsuarioToken.IdCatUsuario;
                    //archiCarga.Identificador = objSLOTransporteSolicitud.IdSLOTransporteSolicitud.ToString();
                    archiCarga.IdOrden = objCotControlTerrestre.sloSolicitudes.IdOrden;
                    archiCarga.IdUsuario = UsuarioDTO.IdCatUsuario;
                    archiCarga.Identificador = objCotControlTerrestre.sloTransporteAsignado.IdSLOTransporteSolicitud.ToString();

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
                        await sweetAlertService.CloseAsync();
                        await sweetAlertService.FireAsync("Error", $"No se pudo cargar el archivo {archiCarga.NombreArchivo}.", SweetAlertIcon.Error);
                        break; // detener en el primer error
                    }
                    else
                    {
                        todosCorrectos = true;
                        await sweetAlertService.FireAsync("Exito", "Documentos agregados con exito.", SweetAlertIcon.Success);
                        lstCargarArchivos = new List<SLOCargarArchivo>();
                        lstDocumentos = await sloDocumentosService.sloGetFilesTask(objCotControlTerrestre.sloTransporteAsignado.sloTransporteSolicitud.IdSLOTransporteSolicitud);
                        await gridArchivos.Reload();
                        StateHasChanged();
                    }
                }
                catch (Exception ex)
                {
                    todosCorrectos = false;
                    await sweetAlertService.CloseAsync();
                    await sweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                    break;
                }

                procesados++;
            }

            // 🔹 Al finalizar
            await sweetAlertService.CloseAsync();

            if (todosCorrectos)
            {
                //await SweetAlertService.FireAsync("Éxito", "Todos los documentos se cargaron correctamente.", SweetAlertIcon.Success);
            }
            else
            {
                await sweetAlertService.FireAsync("Proceso incompleto", "Algunos documentos no se pudieron cargar.", SweetAlertIcon.Warning);
            }
        }
        #endregion

        #region CARD DERECHA

        private void MostrarEventualidad(Evento evento)
        {
            if (evento.EsEventual)
            {
                eventualidadSeleccionada = evento;
            }
        }
        private async Task AgregarTracking()
        {
            var response = await DialogService.OpenAsync<UltimaMillaTimeLineAgregarTraking>(
                $"Registrar evento de tracking: {objTControlTerrestre.sloTransporteAsignado.Placas}",
                new Dictionary<string, object>
                {
                    { "TControlTerrestre", objCotControlTerrestre },
                    {"UsuarioDTO", UsuarioDTO},
                    {"Booking", objTControlTerrestre.FConfirmaBooking}
                },
                new DialogOptions
                {
                    Width = "900px",
                    Height = "600px",
                    Draggable = true,
                    Resizable = true,
                    Style = "border-radius: 12px;",
                    CloseDialogOnOverlayClick = false
                }
            );
            if (response == true)
            {
                await ConstruirEventos();
                lstDocumentos = await sloDocumentosService.sloGetFilesTask(objTControlTerrestre.sloTransporteAsignado.sloTransporteSolicitud.IdSLOTransporteSolicitud);
                await gridArchivos.Reload();
                StateHasChanged();
            }
        }
        #endregion

        #endregion        
    }
}