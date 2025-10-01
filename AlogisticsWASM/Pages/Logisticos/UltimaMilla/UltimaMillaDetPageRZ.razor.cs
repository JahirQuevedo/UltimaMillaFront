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
using Newtonsoft.Json.Linq;
using Radzen;
using Radzen.Blazor;
using static AlogisticsWASM.Pages.Vacios.Solicitudes.SolicitudesCRUDCMP;
using static System.Net.WebRequestMethods;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaDetPageRZ
    {
        #region PARAMETROS
        [Parameter] public UsuarioTokenDTO UsuarioToken { get; set; } // Ensure UsuarioToken is passed as a parameter
        #endregion

        #region SERVICIOS
        [Inject] protected NotificationService NotificationService { get; set; } // Ensure NotificationService is injected
        [Inject] SweetAlertService SweetAlertService { get; set; }
        [Inject] ICatTipoTransporteService TipoTransporteService { get; set; }
        [Inject] ICatMercanciasService MercanciasService { get; set; }
        [Inject] ICatClientesService ClientesService { get; set; }
        //[Inject] IUltimaMillaEncabezadoService UltimaMillaEncabezadoService { get; set; }
        [Inject] ISLOSolicitudesService SLOSolicitudesService { get; set; }
        [Inject] ISLOSolicitudesDetalleService SLOSolicitudesDetalleService { get; set; }
        [Inject] ICatPatiosServices PatiosService { get; set; }
        [Inject] ICatTipoOperacionService TipoOperacionService { get; set; }
        [Inject] ICatClientesUbicacionesService CatClientesUbicacionesService { get; set; }
        [Inject] ICatTipoCargaService CatTipoCargaService { get; set; }
        [Inject] ICatTipoOperacionesComercioService CatOperacionesComercioService { get; set; }
        [Inject] ICatTipoIMOService CatTipoIMOService { get; set; }
        [Inject] ICatTipoEstadoService CatTipoEstadoService { get; set; }
        [Inject] ICatPaisesService CatPaisesService { get; set; }
        [Inject] ICatPaisEstadosService CatPaisEstadosService { get; set; }
        [Inject] private ICatPaisMunicipiosService catPaisMunicipiosService { get; set; }
        [Parameter] public string Modo { get; set; } = "C";
        [Parameter] public SLOSolicitudes? Solicitud { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        [Inject] ISLODocumentosService sloDocumentosService { get; set; }
        [Inject] private HttpClient Http { get; set; }
        #endregion

        #region OBJETOS, LISTAS, VARIABLES
        SLOSolicitudes objSLOSolicitudes = new SLOSolicitudes();
        private CatClientesUbicaciones objClientesUbicaciones = new CatClientesUbicaciones();
        private DateTime? FechaSalida { get; set; } = DateTime.Today;
        private DateTime? FechaEntrega { get; set; } = DateTime.Today;
        //private DateTime HoraSalida { get; set; } = DateTime.Today;
        //private DateTime HoraEntrega { get; set; } = DateTime.Today;

        private ICollection<CatTipoTransporte> lstTipoTransporte;
        private ICollection<CatMercancias> lstCatMercancias;
        private ICollection<CatClientes> lstClientes;
        private ICollection<CatPatios> lstCatPatios;
        private List<CatTipoOperacion> lstCatTipoOperacion;
        private List<CatClientesUbicaciones> lstCatClientesUbicaciones;
        private List<CatTipoCarga> lstCatTipoCarga;
        private List<CatTipoOperacionComercio> lstCatTipoOperacionesComercio;
        private List<CatTipoOperacionComercio> lstCatTipoOperacionesComercioFiltrado = new List<CatTipoOperacionComercio>();
        private List<CatTipoIMO> lstCatTipoIMO;
        private ICollection<CatTipoEstados> lstCatEstados;
        private List<CatPaises> lstPaises;
        private List<CatPaisEstados> lstPaisEstados;
        private List<CatPaisMunicipios> lstMunicipios;

        //Control Tamaño Pantalla
        int alto;
        int ancho;        
        private string zoomType = "";

        //CONTROL DE TABS
        private string acronimoTipoCargaSeleccionado =>
        lstCatTipoCarga?.FirstOrDefault(x => x.IdCatTipoCarga == objSLOSolicitudes.IdCatTipoCarga)?.Acronimo ?? "";

        private int selectedTabIndex;

        //Grid SLOSolicitudesDetalle
        private RadzenDataGrid<SLOSolicitudesDetalle> gridSolicitudesDetalle;
        private List<SLOSolicitudesDetalle> Items = [];
        private bool inInsert = false;
        private SLOSolicitudesDetalle detalleEnEdicion = null;

        private RadzenAccordion accordionRef;

        //variables de RadzenUpload
        private int uploadKey = 0;
        private bool limpiarPendiente = false;
        private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
        private const int MaxCountFiles = 6;
        private RadzenUpload uploadFile;
        private List<SLOCargarArchivo> lstCargarArchivos = new List<SLOCargarArchivo>();
        private RadzenDataGrid<SLOCargarArchivo> documentosGrid;

        //Documentos
        private List<SLOSolicitudesDocumentos> lstDocumentos = new List<SLOSolicitudesDocumentos>();
        private RadzenDataGrid<SLOSolicitudesDocumentos> gridArchivos;

        #endregion

        #region DTO´s
        public class ScreenSize
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
        #endregion

        #region INICIALIZAR
        protected override async Task OnInitializedAsync()
        {
            if (Modo == "E")
            {
                objSLOSolicitudes = Solicitud;
                //Items = new List<SLOSolicitudesDetalle>(objSLOSolicitudes.sloSOlicitudesDetalle);
                Items = await SLOSolicitudesDetalleService.SLOSolicitudesDetalleObtener(objSLOSolicitudes.IdSLOSolicitud);
                FechaSalida = (DateTime)objSLOSolicitudes.FechaPosicionamiento;
                FechaEntrega = (DateTime)objSLOSolicitudes.FechaFin;

                lstCatClientesUbicaciones = await CatClientesUbicacionesService.CatClientesUbicacionesListar();
                lstCatEstados = await CatTipoEstadoService.GetTiposEstado();
                lstPaises = await CatPaisesService.CatPaisesListar();
                lstPaisEstados = await CatPaisEstadosService.CatPaisEstadosListar();
                lstMunicipios = await catPaisMunicipiosService.CatPaisMunicipiosListar();
                lstCatTipoCarga = await CatTipoCargaService.CatTipoCargaListar();                
                lstCatTipoIMO = await CatTipoIMOService.GetCatTipoIMO();
                //lstTipoTransporte = await TipoTransporteService.GetTipoTransporte();
                lstCatTipoCarga = await CatTipoCargaService.CatTipoCargaListar();
                lstCatMercancias = await MercanciasService.GetCatMercancias();
                lstDocumentos = await sloDocumentosService.SLOListarArchivosSolicitud(Solicitud.IdSLOSolicitud);
            }
            if (Modo == "C")
            {
                
                lstTipoTransporte = await TipoTransporteService.GetTipoTransporte();
                lstCatMercancias =  await MercanciasService.GetCatMercancias();
                lstClientes =  await ClientesService.GetClientes();
                lstCatPatios =  await PatiosService.GetPatios();
                lstCatTipoOperacion =  await TipoOperacionService.CatTipoOperacionListar();
                lstCatClientesUbicaciones =  await CatClientesUbicacionesService.CatClientesUbicacionesListar();
                lstCatTipoCarga = await CatTipoCargaService.CatTipoCargaListar();
                lstCatTipoOperacionesComercio =  await CatOperacionesComercioService.GetCatTipoOperacionesComercio();
                lstCatTipoIMO = await CatTipoIMOService.GetCatTipoIMO();
                lstCatEstados =  await CatTipoEstadoService.GetTiposEstado();
                lstPaises =  await CatPaisesService.CatPaisesListar();
                lstPaisEstados =  await CatPaisEstadosService.CatPaisEstadosListar();
                lstMunicipios =  await catPaisMunicipiosService.CatPaisMunicipiosListar();
                lstCatTipoOperacionesComercioFiltrado = lstCatTipoOperacionesComercio.Where(c => c.Acronimo == "EXPO" || c.Acronimo == "NACIONAL").ToList();

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
            }
                // Registrar listener de resize
                await JS.InvokeVoidAsync("registerResizeHandler", DotNetObjectReference.Create(this));
                
                if (limpiarPendiente && uploadFile != null)
                {
                    limpiarPendiente = false; // quitar flag antes de limpiar para evitar recursión
                    try
                    {
                        await uploadFile.ClearFiles();
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

        #region FUNCIONES DE MERCANCIA    
        //CONTROL DE TABS
        private void CambiarTabPorTipoCarga()
        {
            // Busca el acrónimo del tipo de carga seleccionado
            var acronimo = lstCatTipoCarga?.FirstOrDefault(x => x.IdCatTipoCarga == objSLOSolicitudes.IdCatTipoCarga)?.Acronimo;

            // Asigna el índice según el acrónimo
            selectedTabIndex = acronimo switch
            {
                "PAL" => 0,
                "CONT" => 1,
                "GEN" => 2,
                _ => 3
            };
            var seleccionado = selectedTabIndex;
        }

        //CONTROL DE GRID DE PALETIZADA
        // Eliminar detalle
        private async Task InsertarDetalle()
        {
            try
            {
                // Si la fila en edición ya fue eliminada, resetear referencias
                //if (detalleEnEdicion != null && !Items.Contains(detalleEnEdicion))
                //{
                //    detalleEnEdicion = null;
                //    inInsert = false;
                //}
                // Validar si ya hay un detalle pendiente incompleto
                var detallePendienteItems = Items.FirstOrDefault(d => ValidarDetalleLista(d).Any());
                var erroresGrid = detalleEnEdicion != null ? ValidarDetalleLista(detalleEnEdicion) : new List<string>();

                if (detallePendienteItems != null || erroresGrid.Any())
                {
                    var errores = detallePendienteItems != null
                                    ? ValidarDetalleLista(detallePendienteItems)
                                    : erroresGrid;

                    string mensaje = "<ul style='padding-left:20px; line-height:1.5;'>" +
                                     string.Join("", errores.Select(e => $"<li>{e}</li>")) +
                                     "</ul>";
                    
                    await SweetAlertService.FireAsync("Falta Información", "Complete la información de mercancía pendiente para proceguir"/* mensaje*/, SweetAlertIcon.Warning);

                    // Mantener fila en edición
                    //if (detallePendienteItems != null)
                    //    await gridSolicitudesDetalle.EditRow(detallePendienteItems);
                    //if (detalleEnEdicion != null)
                    //    await gridSolicitudesDetalle.InsertRow(detalleEnEdicion);

                    var det = detalleEnEdicion;
                    //await gridSolicitudesDetalle.Reload();
                    
                    return; // Bloquear inserción hasta que se complete
                }

                // Crear nuevo detalle
                inInsert = true;
                detalleEnEdicion = new SLOSolicitudesDetalle()
                {
                    MciaPeligrosa = false,
                    Cantidad = 0,
                    Peso = 0.0m,
                    IdCatMercancia = 0,
                    Piezas = 0,
                    Activo = true,
                    IdCatTipoEmbalaje = 1
                };

                await gridSolicitudesDetalle.InsertRow(detalleEnEdicion);
            }
            catch (Exception ex)
            {
                await SweetAlertService.FireAsync("Error", "Error al agregar mercancia: " + ex.Message, SweetAlertIcon.Error);
            }
        }

        private async Task EditarDetalle(SLOSolicitudesDetalle item)
        {
            try
            {
                detalleEnEdicion = item; // ← Registrar el item en edición
                await gridSolicitudesDetalle.EditRow(item);
            }
            catch (Exception ex)
            {
                MostrarError("Error al editar detalle: " + ex.Message);
            }
        }
        //private async Task EliminarDetalle(SLOSolicitudesDetalle item)
        //{
        //    try
        //    {
        //        if (Items.Contains(item))
        //        {                    
        //            Items.Remove(item);                             
        //            await gridSolicitudesDetalle.Reload();
        //            //MostrarExito("Detalle marcado como eliminado correctamente");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MostrarError("Error al eliminar detalle: " + ex.Message);
        //    }
        //}
        private async Task EliminarDetalle(SLOSolicitudesDetalle item)
        {
            if(Modo == "E")
            {
                if (Items.Count == 1)
                {
                    await SweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "Mercancía Obligatoria",
                        Text = "Debe existir al menos una mercancía registrada. Favor de agregar una nueva para avanzar.",
                        Icon = SweetAlertIcon.Warning
                    });

                    return;

                }

                var result = await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "¿Desea eliminar esta mercancía?",
                    Text = "La mercancía será eliminada definitivamente.",
                    Icon = SweetAlertIcon.Warning,
                    ShowCancelButton = true,
                    ConfirmButtonText = "Sí, eliminar",
                    CancelButtonText = "Cancelar"
                });
                

                if (result.IsConfirmed)
                {
                    try
                    {
                        // Cancelar edición si está en modo edición
                        gridSolicitudesDetalle.CancelEditRow(item);

                        // Si el item está en Items (ya guardado), removerlo
                        if (Items.Contains(item))
                        {
                            Items.Remove(item);
                        }

                        // Si era una fila nueva que aún no está en Items, resetear flags
                        if (inInsert && detalleEnEdicion == item)
                        {
                            inInsert = false;
                            detalleEnEdicion = null;
                        }

                        await ActualizarSolicitud(objSLOSolicitudes);

                        //objSLOSolicitudes = await SLOSolicitudesService.ObtenerSolicitudId(objSLOSolicitudes.IdSLOSolicitud);
                        Items = await SLOSolicitudesDetalleService.SLOSolicitudesDetalleObtener(objSLOSolicitudes.IdSLOSolicitud);
                        // Refrescar el grid
                        await gridSolicitudesDetalle.Reload();
                        StateHasChanged();

                        //objSLOSolicitudes.IdCatUsuario = UsuarioToken.IdCatUsuario;
                        //objSLOSolicitudes.catClienteUbicacionDestino = null;
                        //objSLOSolicitudes.catClienteUbicacionOrigen = null;
                        //objSLOSolicitudes.catTipoEstado = null;
                        //objSLOSolicitudes.catTipoOperacion = null;
                        //objSLOSolicitudes.catUsuario = null;
                        //objSLOSolicitudes.Cliente = null;
                        //objSLOSolicitudes.Orden = null;

                        //objSLOSolicitudes.sloSOlicitudesDetalle = Items;
                        //var respuestaGenericaDTO = await SLOSolicitudesService.ActualizarSolicitudSLO(objSLOSolicitudes);
                        //if (respuestaGenericaDTO.IsSuccess)
                        //{
                        //    NotificationService.Notify(new NotificationMessage
                        //    {
                        //        Severity = NotificationSeverity.Success,
                        //        Summary = "Eliminado",
                        //        Detail = "Mercancia eliminada correctamente",
                        //        Duration = 4000
                        //    });

                        
                        //}
                        //else
                        //{
                        //    await SweetAlertService.FireAsync("Error", "Ha ocurrido un error al dar de baja mercancía", SweetAlertIcon.Error);
                        //    return;
                        //}                        
                        // Mostrar mensaje (opcional)
                        //MostrarExito("Detalle eliminado correctamente");
                    }
                    catch (Exception ex)
                    {
                        MostrarError("Error al eliminar detalle: " + ex.Message);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                try
                {
                    // Cancelar edición si está en modo edición
                    gridSolicitudesDetalle.CancelEditRow(item);

                    // Si el item está en Items (ya guardado), removerlo
                    if (Items.Contains(item))
                    {
                        Items.Remove(item);
                    }

                    // Si era una fila nueva que aún no está en Items, resetear flags
                    if (inInsert && detalleEnEdicion == item)
                    {
                        inInsert = false;
                        detalleEnEdicion = null;
                    }

                    // Refrescar el grid
                    await gridSolicitudesDetalle.Reload();

                    // Mostrar mensaje (opcional)
                    //MostrarExito("Detalle eliminado correctamente");
                }
                catch (Exception ex)
                {
                    MostrarError("Error al eliminar detalle: " + ex.Message);
                }
            }
        }



        private async Task AgregarDetalle(SLOSolicitudesDetalle item)
        {
            try
            {
                var errores = ValidarDetalleLista(item);

                if (errores.Any())
                {
                    //// Construir un mensaje HTML para SweetAlert
                    //string mensaje = "<ul style='padding-left:20px; line-height:1.5;'>" +
                    //                 string.Join("", errores.Select(e => $"<li>{e}</li>")) +
                    //                 "</ul>";

                    await SweetAlertService.FireAsync("Falta Información", "Por favor complete la información faltante de mercancía", SweetAlertIcon.Warning);
                    detalleEnEdicion = item;
                    //await gridSolicitudesDetalle.EditRow(item);
                    return; // No continuar si hay errores
                }

                // GuardarEventualidad detalles si no hay errores
                //await GuardarTodosDetalles();

                if (inInsert)
                {
                    Items.Add(item);
                    inInsert = false;
                }
                else
                {
                    var existingItem = Items.FirstOrDefault(i => i == item);
                    if (existingItem != null)
                    {
                        existingItem.MciaPeligrosa = item.MciaPeligrosa;
                        existingItem.Cantidad = item.Cantidad;
                        existingItem.Peso = item.Peso;
                        existingItem.IdCatMercancia = item.IdCatMercancia;
                    }
                }

                await gridSolicitudesDetalle.UpdateRow(item);
                detalleEnEdicion = null;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await SweetAlertService.FireAsync("Error", "Error al guardar detalle: " + ex.Message, SweetAlertIcon.Error);
            }
        }

        private async Task CancelarEdicion(SLOSolicitudesDetalle item)
        {
            try
            {
                gridSolicitudesDetalle.CancelEditRow(item);
                if (inInsert)
                {
                    Items.Remove(item); // eliminar fila nueva si se cancela
                    inInsert = false;
                }
                detalleEnEdicion = null; // ← Limpiar estado
                StateHasChanged();
            }
            catch (Exception ex)
            {
                MostrarError("Error al cancelar edición: " + ex.Message);
            }
        }
              

        // Mostrar notificación de error
        private void MostrarError(string mensaje)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = mensaje,
                Duration = 4000
            });
        }

        private List<string> ValidarDetalleLista(SLOSolicitudesDetalle item)
        {
            var errores = new List<string>();

            if(string.IsNullOrWhiteSpace(item.NoParte))
                errores.Add("<strong>NoParte</strong> no puede estar vacío HECHO POR JAHIR");
            //if (item.NoParte == null || item.NoParte == "")
            //    errores.Add("<strong>NoParte</strong> no puede estar vacío");

            if (item.Cantidad == null || item.Cantidad <= 0)
                errores.Add("<strong>Cantidad Piezas</strong> debe ser mayor a 0");

            if (item.Peso == null || item.Peso <= 0)
                errores.Add("<strong>Peso</strong> debe ser mayor a 0");

            if (item.IdCatMercancia == 0)
                errores.Add("Debe seleccionar <strong>Tipo Mercancia</strong>");

            if (item.Piezas == null || item.Piezas <= 0)
                errores.Add("<strong>Cantidad de Piezas</strong> debe ser mayor a 0");         
            


            return errores;
        }

        private /*List<string>*/bool ValidarDetalleListaGuardado(SLOSolicitudesDetalle item)
        {
            //var errores = new List<string>();
            bool validado = true;

            if (string.IsNullOrWhiteSpace(item.NoParte))
                validado = false;
            //errores.Add("<strong>NoParte</strong> no puede estar vacío HECHO POR JAHIR");
            //if (item.NoParte == null || item.NoParte == "")
            //    errores.Add("<strong>NoParte</strong> no puede estar vacío");

            if (item.Cantidad == null || item.Cantidad <= 0)
                validado = false;
            //errores.Add("<strong>Cantidad Piezas</strong> debe ser mayor a 0");

            if (item.Peso == null || item.Peso <= 0)
                validado = false;
            //errores.Add("<strong>Peso</strong> debe ser mayor a 0");

            if (item.IdCatMercancia == 0)
                validado = false;
            //errores.Add("Debe seleccionar <strong>Tipo Mercancia</strong>");

            if (item.Piezas == null || item.Piezas <= 0)
                validado = false;
                //errores.Add("<strong>Cantidad de Piezas</strong> debe ser mayor a 0");



            return validado;
        }

        #endregion

        #region GUARDADO Y VALIDACIONES

        public async Task<bool> MostrarValidaciones(List<string> Validaciones)
        {
            if (Validaciones.Any())
            {
                // Unimos los errores en un string con saltos de línea
                string mensaje = "<ul style='padding-left: 20px; line-height: 1.6;'>" +
                                 string.Join("", Validaciones.Select(e => $"<li>{e}</li>")) +
                                 "</ul>";
                await SweetAlertService.FireAsync("Datos incompletos", mensaje, SweetAlertIcon.Warning);
                return false;
            }
            return true;
        }
        //GENERAR SOLICITUD DE SERVICIO
        private async Task GenerarSolicitud()
        {
            List<string> Validaciones = new List<string>();

            RespuestaGenericaDTO respuestaGenericaDTO;
            //TimeSpan Salida = HoraSalida.TimeOfDay;
            //TimeSpan Entrega = HoraEntrega.TimeOfDay;
            //var fechaPosicionamiento = FechaSalida.Date.Add(Salida);
            //var fechaEntregaFinal = FechaEntrega.Date.Add(Entrega);

            objSLOSolicitudes.FechaPosicionamiento = FechaSalida;
            objSLOSolicitudes.FechaFin = FechaEntrega;
            objSLOSolicitudes.sloSOlicitudesDetalle = Items;

            // Forzar que el grid se sincronice con Items            
            // al final de GenerarSolicitud(), después de confirmar que todo fue válido
            //Items = Items.ToList(); // fuerza una copia “limpia”
            //await gridSolicitudesDetalle.Reload();            

            //Validaciones
            if (objSLOSolicitudes.IdCatCliente < 1)
                Validaciones.Add("No se ha seleccionado un <strong>Cliente</strong>");

            if (objSLOSolicitudes.ReferenciaCliente != null && string.IsNullOrWhiteSpace(objSLOSolicitudes.ReferenciaCliente))
            {
                objSLOSolicitudes.ReferenciaCliente = null; // Asignamos null                
            }

            if (!(await MostrarValidaciones(Validaciones)))
            {
                return;
            }

            if (objSLOSolicitudes.IdCatTipoOperComercio < 1)
            {
                Validaciones.Add("Debe seleccionar al menos un <strong>Tipo de Operación Comercial</strong>");
            }

            if (objSLOSolicitudes.IdCatTipoOperacion < 1)
                Validaciones.Add("Debe seleccionar al menos un <strong>Tipo de Operación</strong>");

            if (!(await MostrarValidaciones(Validaciones)))
            {
                return;
            }

            if (objSLOSolicitudes.IdCatUbicacionOrigen < 1)
                Validaciones.Add("Debe seleccionar al menos una <strong>Ubicación de Origen</strong>");

            if (objSLOSolicitudes.IdCatUbicacionDestino < 1)
                Validaciones.Add("Debe seleccionar una <strong>Ubicación de Destino</strong>");

            if (objSLOSolicitudes.IdCatUbicacionOrigen == objSLOSolicitudes.IdCatUbicacionDestino)
                Validaciones.Add("La <strong>Ubicación de Origen</strong> y la <strong>Ubicación de Destino</strong> no pueden ser iguales");

            if (!(await MostrarValidaciones(Validaciones)))
            {
                return;
            }

            

            if (!objSLOSolicitudes.FechaPosicionamiento.HasValue)
                Validaciones.Add("Debe seleccionar una <strong>Fecha de Posicionamiento</strong>");

            if (!objSLOSolicitudes.FechaFin.HasValue)
                Validaciones.Add("Debe seleccionar una <strong>Fecha de Finalización</strong>");

            if (objSLOSolicitudes.FechaPosicionamiento == objSLOSolicitudes.FechaFin)
                Validaciones.Add("La <strong>Fecha de Posicionamiento</strong> no puede ser la misma que <strong>Fecha esperada de Entrega</strong>");

            if (objSLOSolicitudes.FechaPosicionamiento > objSLOSolicitudes.FechaFin)
                Validaciones.Add("La <strong>Fecha de Posicionamiento</strong> no puede ser superior que <strong>Fecha esperada de Entrega</strong>");


            if (!(await MostrarValidaciones(Validaciones)))
            {
                return;
            }

            if (objSLOSolicitudes.IdCatTipoCarga < 1 || objSLOSolicitudes.IdCatTipoCarga == null)
                Validaciones.Add("Se debe establecer un <strong>Tipo de Carga</strong>");

                        
            

            if (!(await MostrarValidaciones(Validaciones)))
            {
                return;
            }
            //if (objSLOSolicitudes.sloSOlicitudesDetalle.Count < 1 || objSLOSolicitudes.sloSOlicitudesDetalle == null)
            //    Validaciones.Add("La solicitud debe tener al menos una <strong>Mercancia</strong> a transportar");

            //if(objSLOSolicitudes.sloSOlicitudesDetalle == null || objSLOSolicitudes.sloSOlicitudesDetalle.Count == 0)
            //    Validaciones.Add("Debe cargar al menos una <strong>Mercancía</strong> a la solicitud");

            //if (objSLOSolicitudes.FechaPosicionamiento == objSLOSolicitudes.FechaFin)
            //    Validaciones.Add("La <strong>Fecha de Posicionamiento</strong> no puede ser la misma que <strong>Fecha esperada de Entrega</strong>");
            // Si hay errores, los mostramos y salimos
            if (gridSolicitudesDetalle != null)
            {


                if (!gridSolicitudesDetalle.IsValid)
                {
                    Validaciones.Add("Existe mercancía en edición.");
                }
                var editingItems = gridSolicitudesDetalle.EditRows;
            }
            // colección de filas en modo edición

            if (detalleEnEdicion != null)
            {
                Validaciones.Add("La información de la mercancía no es válida. Por favor, verifica los datos ingresados.");
            }

            if (!(await MostrarValidaciones(Validaciones)))
            {
                return;
            }
            //int index = 1;
            //foreach (var mercancia in Items)
            //{
            //    var errores = ValidarDetalleLista(mercancia);

            //    foreach (var error in errores)
            //    {
            //        Validaciones.Add($"Mercancía {index}: {error}");
            //    }

            //    index++;
            //}

            if (objSLOSolicitudes.sloSOlicitudesDetalle.Count < 1 || objSLOSolicitudes.sloSOlicitudesDetalle == null)
            {
                Validaciones.Add("La solicitud debe tener al menos una <strong>Mercancia</strong> a transportar");
            }
            if (!(await MostrarValidaciones(Validaciones)))
            {
                return;
            }
            else
            {
                bool validado = false;
                foreach (var mercancia in Items)
                {
                    validado = ValidarDetalleListaGuardado(mercancia);
                    if (!validado)
                    {
                        break;
                    }
                    //foreach (var error in errores)
                    //{
                    //    Validaciones.Add($"Mercancía {index}: {error}");
                    //}
                    //index++;                                    
                }

                if (!validado)
                    Validaciones.Add("Datos no completos en Detalle de Mercancía.");
            }

            ////////VALIDAR AQUI
            if (!(await MostrarValidaciones(Validaciones)))
            {
                return;
            }

            // Si pasó todas las validaciones            
            //objSLOSolicitudes.FechaInicio = DateTime.Now;
            objSLOSolicitudes.FechaRegistro = DateTime.Now;
            objSLOSolicitudes.IdCatTipoEstado = 1;            
            objSLOSolicitudes.Activo = true;
            objSLOSolicitudes.IdCatUsuario = UsuarioToken.IdCatUsuario;
            objSLOSolicitudes.catClienteUbicacionDestino = null;
            objSLOSolicitudes.catClienteUbicacionOrigen = null;

            respuestaGenericaDTO = await SLOSolicitudesService.CrearSolicitudSLO(objSLOSolicitudes);

            if (respuestaGenericaDTO.IsSuccess)
            {                             

                Solicitud = (respuestaGenericaDTO.Entidad as JObject)?.ToObject<SLOSolicitudes>();                

                if(lstCargarArchivos.Count > 0)
                    await ProcesarDocumentosAsync();

                await SweetAlertService.FireAsync("Solicitud creada", "La solicitud se creó correctamente.", SweetAlertIcon.Success);
                DialogService.Close(true);
            }
            else
            {
                await SweetAlertService.FireAsync("Error", "No se pudo crear la solicitud.", SweetAlertIcon.Error);
            }

        }

        //ACTUALIZAR SOLICITUD DE SERVICIO
        private async Task ActualizarSolicitud(SLOSolicitudes solicitud)
        {
            List<string> Validaciones = new List<string>();
            RespuestaGenericaDTO respuestaGenericaDTO;

            // Asignación de fechas y detalles
            objSLOSolicitudes.FechaPosicionamiento = FechaSalida;
            objSLOSolicitudes.FechaFin = FechaEntrega;
            objSLOSolicitudes.sloSOlicitudesDetalle = Items;
            objSLOSolicitudes.IdCatTipoOperComercio = 1;

            // === Validaciones replicadas de GenerarSolicitud ===
            if (objSLOSolicitudes.IdCatCliente < 1)
                Validaciones.Add("No se ha seleccionado un <strong>Cliente</strong>");

            if (objSLOSolicitudes.ReferenciaCliente != null && string.IsNullOrWhiteSpace(objSLOSolicitudes.ReferenciaCliente))
            {
                objSLOSolicitudes.ReferenciaCliente = null; // Asignamos null                
            }

            if (objSLOSolicitudes.IdCatUbicacionOrigen < 1)
                Validaciones.Add("Debe seleccionar al menos una <strong>Ubicación de Origen</strong>");

            if (objSLOSolicitudes.IdCatUbicacionDestino < 1)
                Validaciones.Add("Debe seleccionar al menos una <strong>Ubicación de Destino</strong>");

            if(objSLOSolicitudes.IdCatTipoOperComercio < 1)
            {
                Validaciones.Add("Debe seleccionar al menos un <strong>Tipo de Operación Comercial</strong>");
            }

            if (objSLOSolicitudes.IdCatTipoOperacion < 1)
                Validaciones.Add("Debe seleccionar al menos un <strong>Tipo de Operación</strong>");

            if (!objSLOSolicitudes.FechaPosicionamiento.HasValue)
                Validaciones.Add("Debe seleccionar una <strong>Fecha de Posicionamiento</strong>");

            if (!objSLOSolicitudes.FechaFin.HasValue)
                Validaciones.Add("Debe seleccionar una <strong>Fecha de Finalización</strong>");

            if (objSLOSolicitudes.IdCatTipoCarga < 1 || objSLOSolicitudes.IdCatTipoCarga == null)
                Validaciones.Add("Se debe establecer un <strong>Tipo de Carga</strong>");

            if (objSLOSolicitudes.IdCatUbicacionOrigen == objSLOSolicitudes.IdCatUbicacionDestino)
                Validaciones.Add("La <strong>Ubicación de Origen</strong> y la <strong>Ubicación de Destino</strong> no pueden ser iguales");

            if (objSLOSolicitudes.FechaPosicionamiento == objSLOSolicitudes.FechaFin)
                Validaciones.Add("La <strong>Fecha de Posicionamiento</strong> no puede ser la misma que <strong>Fecha esperada de Entrega</strong>");

            if (objSLOSolicitudes.FechaPosicionamiento > objSLOSolicitudes.FechaFin)
                Validaciones.Add("La <strong>Fecha de Posicionamiento</strong> no puede ser superior que <strong>Fecha esperada de Entrega</strong>");           

            // Validaciones del grid y detalle en edición
            if (!gridSolicitudesDetalle.IsValid)
                Validaciones.Add("Existen filas en edición en <strong>Agregar Mercancia</strong>.");

            if (detalleEnEdicion != null)
                Validaciones.Add("Información no valida de Mercancía, por favor valida.");

            // Validaciones de cada mercancía
            //int index = 1;
            //List<string> errores = new();
            if (objSLOSolicitudes.sloSOlicitudesDetalle.Count < 1 || objSLOSolicitudes.sloSOlicitudesDetalle == null)
            {
                Validaciones.Add("La información de la mercancía no es válida. Por favor, verifica los datos ingresados.");
            }
            else
            {
                bool validado = false;
                foreach (var mercancia in Items)
                {
                    validado = ValidarDetalleListaGuardado(mercancia);
                    if (!validado)
                    {
                        break;
                    }
                    //foreach (var error in errores)
                    //{
                    //    Validaciones.Add($"Mercancía {index}: {error}");
                    //}
                    //index++;                                    
                }

                if (!validado)
                    Validaciones.Add("Datos no completos en Detalle de Mercancía.");
            }
                


            // Mostrar errores si existen
            if (Validaciones.Any())
            {
                string mensaje = "<ul style='padding-left: 20px; line-height: 1.6;'>" +
                                 string.Join("", Validaciones.Select(e => $"<li>{e}</li>")) +
                                 "</ul>";
                await SweetAlertService.FireAsync("Datos incompletos o no válidos", mensaje, SweetAlertIcon.Warning);
                return;
            }

            // Limpieza de referencias antes de actualizar
            var objClon = new SLOSolicitudes
            {
                IdSLOSolicitud = objSLOSolicitudes.IdSLOSolicitud,
                IdCatCliente = objSLOSolicitudes.IdCatCliente,
                IdCatUbicacionOrigen = objSLOSolicitudes.IdCatUbicacionOrigen,
                IdCatUbicacionDestino = objSLOSolicitudes.IdCatUbicacionDestino,
                IdCatTipoCarga = objSLOSolicitudes.IdCatTipoCarga,
                IdCatTipoOperComercio = objSLOSolicitudes.IdCatTipoOperComercio,
                FechaPosicionamiento = objSLOSolicitudes.FechaPosicionamiento,
                ReferenciaCliente = objSLOSolicitudes.ReferenciaCliente,
                Booking = objSLOSolicitudes.Booking,
                FechaInicio = objSLOSolicitudes.FechaInicio,
                FechaFin = objSLOSolicitudes.FechaFin,
                IdCatUsuario = UsuarioToken.IdCatUsuario, // nuevo valor si aplica
                Notas = objSLOSolicitudes.Notas,
                IdCatTipoEstado = objSLOSolicitudes.IdCatTipoEstado,
                IdOrden = objSLOSolicitudes.IdOrden,
                Activo = objSLOSolicitudes.Activo,
                FechaRegistro = objSLOSolicitudes.FechaRegistro,
                IdCatTipoOperacion = objSLOSolicitudes.IdCatTipoOperacion,

                // Relaciones anuladas
                Cliente = null,
                TipoCarga = null,
                catUsuario = null,
                catClienteUbicacionOrigen = null,
                catClienteUbicacionDestino = null,
                Orden = null,
                catTipoEstado = null,
                catTipoOperacion = null,
                sloSOlicitudesDetalle = Items
            };
            
            // Llamada al servicio de actualización
            try
            {
                respuestaGenericaDTO = await SLOSolicitudesService.ActualizarSolicitudSLO(objClon);
                if (respuestaGenericaDTO.IsSuccess)
                {
                    await ProcesarDocumentosAsync();

                    await SweetAlertService.FireAsync("Actualizado", "Solicitud de Servicios Actualizada Correctamente", SweetAlertIcon.Success);
                    //DialogService.Close(true);
                }
                else
                {
                    await SweetAlertService.FireAsync(
                        "Solicitud de servicio no fue actualizada",
                        "La solicitud no pudo ser modificada correctamente.",
                        SweetAlertIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        //MANEJO DE UBICACIONES
        private async Task AgregarUbicacion()
        {
            string anchoModal = "";
            string altoModal = "";
            try
            {
                if (objSLOSolicitudes?.IdCatCliente == null)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Debe seleccionarse un cliente primero");
                    return;
                }

                if(ancho < 1300)
                {
                    anchoModal = "85%";
                    altoModal = "75%";
                }
                else
                {
                    anchoModal = "50%";
                    altoModal = "55%";
                }
                    // Abrir el diálogo de agregar ubicación
                    var resultado = await DialogService.OpenAsync<UbicacionForm>(
                        "Agregar Ubicación",
                        new Dictionary<string, object>
                        {
                        { "IdCatCliente", objSLOSolicitudes.IdCatCliente }
                        },
                        new DialogOptions
                        {
                            Width = anchoModal,
                            Height = altoModal,
                            Resizable = true,
                            Draggable = true,
                            Style = "border-radius: 12px;"
                        });

                if (resultado)
                {
                    // Actualizar la lista de ubicaciones
                    lstCatClientesUbicaciones = await CatClientesUbicacionesService.CatClientesUbicacionesListar();
                    StateHasChanged();
                    NotificationService.Notify(NotificationSeverity.Success, "Ubicación del cliente agregada correctamente");
                }
                else
                {
                    //NotificationService.Notify(NotificationSeverity.Error, "No se agregó una nueva ubicación del cliente");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //NotificationService.Notify(NotificationSeverity.Error, $"Ocurrió un error: {ex.Message}");
            }
        }

        private void CambioUbicacion(object value)
        {
            if (value != null)
            {
                // Obtener la ubicación seleccionada del listado
                int idSeleccionado = (int)value;
                objSLOSolicitudes.catClienteUbicacionOrigen = lstCatClientesUbicaciones
                    .FirstOrDefault(u => u.IdClienteUbicacion == idSeleccionado);

                // Forzar actualización de la interfaz
                StateHasChanged();
            }
        }

        private void CambioUbicacionDestino(object value)
        {
            if (value != null)
            {
                // Obtener la ubicación seleccionada del listado
                int idSeleccionado = (int)value;
                objSLOSolicitudes.catClienteUbicacionDestino = lstCatClientesUbicaciones
                    .FirstOrDefault(u => u.IdClienteUbicacion == idSeleccionado);

                // Forzar actualización de la interfaz
                StateHasChanged();
            }
        }

        //CERRAR MODAL
        private void CerrarModal()
        {
            Solicitud = null;
            DialogService.Close(false);
        }

        #region DOCUMENTOS
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

            // Abrir modal para clasificar los archivos
            var result = await DialogService.OpenAsync<UltimaMillaModalDocumentos>(
                "Clasificar Documentos",
                new Dictionary<string, object>() { { "Archivos", files }, { "Modo", "TIMELINE" } },
                new DialogOptions()
                { Width = "60%", Height = "55%", Resizable = true, Draggable = true, ShowClose = false, Style = "border-radius: 12px;" }
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
                            //Identificador = Solicitud?.Orden?.ReferenciaALO,
                            NombreArchivo = doc.FileInfo?.Name,
                            SizeFile = doc.FileInfo?.Size ?? 0,
                            ContentType = doc.FileInfo?.ContentType,
                            FileBytes = fileBytes,
                            //IdOrden = Solicitud.IdOrden,
                            IdUsuario = UsuarioToken.IdCatUsuario
                        });
                    }
                    var lista = lstCargarArchivos;
                    //await ProcesarDocumentosAsync();
                    limpiarPendiente = true;
                }
                catch (Exception ex)
                {
                    return;
                }

                if(Modo == "E" && Solicitud?.catTipoEstado?.TipoEstado == "A")
                {
                    await ProcesarDocumentosAsync();
                }

                // Limpiar selección del upload
                limpiarPendiente = true;
            }
            limpiarPendiente = true;
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
                    //archiCarga.IdOrden = objSolicitudes.IdOrden;
                    //archiCarga.IdUsuario = UsuarioToken.IdCatUsuario;
                    //archiCarga.Identificador = objSLOTransporteSolicitud.IdSLOTransporteSolicitud.ToString();
                    archiCarga.IdOrden = Solicitud.IdOrden;
                    archiCarga.IdUsuario = UsuarioToken.IdCatUsuario;
                    archiCarga.Identificador = Solicitud.Orden.ReferenciaALO;

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
                    else
                    {
                        todosCorrectos = true;
                        lstCargarArchivos = new List<SLOCargarArchivo>();
                        lstDocumentos = await sloDocumentosService.SLOListarArchivosSolicitud(Solicitud.IdSLOSolicitud); 
                        if(gridArchivos != null)
                        {
                            await gridArchivos.Reload();
                        }                        
                        StateHasChanged();
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
                return;
            }
            else
            {
                await SweetAlertService.FireAsync("Proceso incompleto", "Algunos documentos no se pudieron cargar.", SweetAlertIcon.Warning);
            }
        }

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
                NotificationService.Notify(new NotificationMessage
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
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
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
                    await SweetAlertService.FireAsync("Baja", "Documento dado de baja correctamente", SweetAlertIcon.Success);
                    // Opcional: refrescar lista de documentos
                    lstDocumentos = await sloDocumentosService.SLOListarArchivosSolicitud(Solicitud.IdSLOSolicitud);
                    await gridArchivos.Reload();
                }
                else
                {
                    await SweetAlertService.FireAsync("Error", "Error al dar de baja el documento.", SweetAlertIcon.Error);
                }
            }
            else
            {
                // El usuario canceló
                //await sweetAlertService.FireAsync("Cancelado", "No se eliminó el documento.", SweetAlertIcon.Info);
            }
        }
        #endregion

        #endregion

    }
}


