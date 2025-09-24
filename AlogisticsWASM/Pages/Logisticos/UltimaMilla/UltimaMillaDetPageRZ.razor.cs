using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static AlogisticsWASM.Pages.Vacios.Solicitudes.SolicitudesCRUDCMP;

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



        private async Task AgregarDetalle(SLOSolicitudesDetalle item)
        {
            try
            {
                var errores = ValidarDetalleLista(item);

                if (errores.Any())
                {
                    // Construir un mensaje HTML para SweetAlert
                    string mensaje = "<ul style='padding-left:20px; line-height:1.5;'>" +
                                     string.Join("", errores.Select(e => $"<li>{e}</li>")) +
                                     "</ul>";

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

            if (item.NoParte == null || item.NoParte == "")
                errores.Add("<strong>NoParte</strong> no puede estar vacío");

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

        #endregion

        #region GUARDADO Y VALIDACIONES
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

            if (objSLOSolicitudes.IdCatUbicacionOrigen < 1)
                Validaciones.Add("Debe seleccionar al menos una <strong>Ubicación de Origen</strong>");

            if (objSLOSolicitudes.IdCatUbicacionDestino < 1)
                Validaciones.Add("Debe seleccionar una <strong>Ubicación de Destino</strong>");

            if (objSLOSolicitudes.IdCatTipoOperacion < 1)
                Validaciones.Add("Debe seleccionar al menos un <strong>Tipo de Operación Comercial</strong>");

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

            if (objSLOSolicitudes.sloSOlicitudesDetalle.Count < 1 || objSLOSolicitudes.sloSOlicitudesDetalle == null)
                Validaciones.Add("La solicitud debe tener al menos una <strong>Mercancia</strong> a transportar");

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
                Validaciones.Add("No se permite registrar mercancía con cantidad cero o sin datos. Verifica y corrige antes de continuar.");
            }


            int index = 1;
            foreach (var mercancia in Items)
            {
                var errores = ValidarDetalleLista(mercancia);

                foreach (var error in errores)
                {
                    Validaciones.Add($"Mercancía {index}: {error}");
                }

                index++;
            }



            if (Validaciones.Any())
            {
                // Unimos los errores en un string con saltos de línea
                string mensaje = "<ul style='padding-left: 20px; line-height: 1.6;'>" +
                                 string.Join("", Validaciones.Select(e => $"<li>{e}</li>")) +
                                 "</ul>";
                await SweetAlertService.FireAsync("Datos incompletos", mensaje, SweetAlertIcon.Warning);
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

            if (objSLOSolicitudes.IdCatUbicacionOrigen < 1)
                Validaciones.Add("Debe seleccionar al menos una <strong>Ubicación de Origen</strong>");

            if (objSLOSolicitudes.IdCatUbicacionDestino < 1)
                Validaciones.Add("Debe seleccionar al menos una <strong>Ubicación de Destino</strong>");

            if (objSLOSolicitudes.IdCatTipoOperacion < 1)
                Validaciones.Add("Debe seleccionar al menos un <strong>Tipo de Operación Comercial</strong>");

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

            if (objSLOSolicitudes.sloSOlicitudesDetalle.Count < 1 || objSLOSolicitudes.sloSOlicitudesDetalle == null)
                Validaciones.Add("La solicitud debe tener al menos una <strong>Mercancia</strong> a transportar");

            // Validaciones del grid y detalle en edición
            if (!gridSolicitudesDetalle.IsValid)
                Validaciones.Add("Existen filas en edición en <strong>Agregar Mercancia</strong>.");

            if (detalleEnEdicion != null)
                Validaciones.Add("Debe agregar correctamente la informacion de la <strong>Mercancia</strong>");

            // Validaciones de cada mercancía
            int index = 1;
            List<string> errores = new();
            foreach (var mercancia in Items)
            {
                errores = ValidarDetalleLista(mercancia);
                //foreach (var error in errores)
                //{
                //    Validaciones.Add($"Mercancía {index}: {error}");
                //}
                //index++;                                    
            }

            if (errores.Any())
                Validaciones.Add("Datos no completos en Detalle de Mercancía.");


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
            objSLOSolicitudes.IdCatUsuario = UsuarioToken.IdCatUsuario;
            objSLOSolicitudes.catClienteUbicacionDestino = null;
            objSLOSolicitudes.catClienteUbicacionOrigen = null;
            objSLOSolicitudes.catTipoEstado = null;
            objSLOSolicitudes.catTipoOperacion = null;
            objSLOSolicitudes.catUsuario = null;
            objSLOSolicitudes.Cliente = null;
            objSLOSolicitudes.Orden = null;

            // Llamada al servicio de actualización
            try
            {
                respuestaGenericaDTO = await SLOSolicitudesService.ActualizarSolicitudSLO(solicitud);
                if (respuestaGenericaDTO.IsSuccess)
                {
                    await SweetAlertService.FireAsync("Actualizado", "Solicitud de Servicios Actualizada Correctamente", SweetAlertIcon.Success);
                    DialogService.Close(true);
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
        #endregion
       
    }
}


