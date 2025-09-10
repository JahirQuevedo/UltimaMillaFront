using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Integracion1G;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Orden;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRepositorios.Services.Utilerias.IUtilerias;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Logisticos.Referencias
{
    public partial class SLODetalleReferenciaCMP
    {
        [Inject] private IExportarExcelService iExportarExcelService { get; set; }
        [Inject] private ISLOReferenciasService iSLOReferenciasService { get; set; }
        [Inject] private ICatClientesService icatClientesService { get; set; }
        [Inject] private ICatTipoMonedaService icatTipoMonedaService { get; set; }
        [Inject] private SweetAlertService sweetAlertService { get; set; }

        [Parameter] public Ordenes OrdenSeleccionada { get; set; }
        [Parameter] public Dictionary<string, object> DiccionarioInfoReferencias { get; set; } = new();

        private InputFile inputFile;
        private IBrowserFile _archivo;
        private RadzenDataGrid<SLOPeticionesServicios> grid;

        private IEnumerable<RespListarCoincidenciasDTO> respListarCoincidencias = new List<RespListarCoincidenciasDTO>();
        private IEnumerable<CatServicios> serviciosDisponibles = new List<CatServicios>();

        private List<Dictionary<string, string>> _datosExcel = new();
        private List<string> _columnas = new();
        private List<CatServicios> _servicios = new();
        private List<SLOPeticionesContenedores> serviciosLigados = new();
        private List<SLOPeticionesServicios> serviciosPlano = new();
        private List<SLOPeticionesServicios> serviciosSeleccionados = new();
        private SLOPeticionesServicios servicioBackup;
        private Dictionary<string, string> copiaTemporal = new();
        private SLOPeticionesServicios servicioEnEdicion;
        private RespListarCoincidenciasDTO _respListarCoincidenciasDTOSeleccionada = new();
        private CatServicios _servicioSeleccionado = new();
        private HashSet<int> idsServiciosFacturados = new();


        RadzenDataGrid<Dictionary<string, string>> gridDatos;

        bool mostrarServiciosLigados = true;
        private bool seleccionarTodos = false;
        private bool _isLoading = false;
        private bool mostrarModalFacturacion = false;
        private string comentarioEncabezado = string.Empty;
        private string comentarioPartida = string.Empty;


        //public class PlantillaExcel
        //{
        //    public string Moneda { get; set; }
        //    public string ReferenciaALO { get; set; }
        //    public string Contenedor { get; set; }
        //    public string BL { get; set; }
        //    public string Clave { get; set; }
        //    public string Servicios { get; set; }
        //    public double Cantidad { get; set; }
        //    public double Subtotal { get; set; }
        //    public string ComentariosEncabezado { get; set; }
        //    public string ComentariosPartida { get; set; }
        //    public bool Procesado { get; set; } = false;
        //}       

        #region Autocompletado y excel
        private async Task SubirYProcesarArchivo(InputFileChangeEventArgs e)
        {
            _archivo = e.File;

            if (_archivo == null)
                return;

            _isLoading = true;
            StateHasChanged();

            using var content = new MultipartFormDataContent();
            var buffer = new byte[_archivo.Size];
            await _archivo.OpenReadStream().ReadAsync(buffer);
            content.Add(new ByteArrayContent(buffer), "File", _archivo.Name);

            //try
            //{
            //    _datosExcel = await iExportarExcelService.CargarExcelAsync(content);

            //    if (_datosExcel.Any())
            //    {
            //        _columnas = _datosExcel.First().Keys.ToList();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await js.InvokeVoidAsync("alert", $"Error al cargar el archivo: {ex.Message}");
            //}

            //mostrarServiciosLigados = false;
            //_isLoading = false;
            //StateHasChanged();
            try
            {
                var datosTemporales = await iExportarExcelService.CargarExcelAsync(content);

                // 🧹 Filtrar filas completamente vacías o con solo N/A
                _datosExcel = datosTemporales
                    .Where(dic => dic.Values.Any(v => !string.IsNullOrWhiteSpace(v) && v != "N/A"))
                    .ToList();

                if (_datosExcel.Any())
                {
                    _columnas = _datosExcel.First().Keys.ToList();
                }
            }
            catch (Exception ex)
            {
                await js.InvokeVoidAsync("alert", $"Error al cargar el archivo: {ex.Message}");
            }

            mostrarServiciosLigados = false;
            _isLoading = false;
            StateHasChanged();
        }

        private void CancelarCarga()
        {
            _datosExcel?.Clear();
            _columnas?.Clear();
            _archivo = null;
            mostrarServiciosLigados = true;
            StateHasChanged();
        }

        private void OnClienteSeleccionado(object value, SLOPeticionesServicios serv)
        {
            var cliente = respListarCoincidencias.FirstOrDefault(c => c.RazonSocial.Equals(value.ToString()));

            if (cliente != null)
            {
                _respListarCoincidenciasDTOSeleccionada = cliente;
                serv.catClientesFacturarA = new CatClientes
                {
                    IdCatCliente = cliente.Id,
                    RazonSocial = cliente.RazonSocial
                };
            }
        }

        async Task AutoCompleteOnLoadCliente(LoadDataArgs args)
        {
            var strQuery = args.Filter ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(strQuery))
            {
                respListarCoincidencias = await icatClientesService.GetClientesConincidencia(strQuery);
            }
            else
            {
                respListarCoincidencias = new List<RespListarCoincidenciasDTO>();
            }
        }

        private void OnClientefactSeleccionado(object value, Dictionary<string, string> dato)
        {
            if (dato == null || value == null)
                return;

            // Si el valor seleccionado es una instancia del DTO correcto
            if (value is RespListarCoincidenciasDTO clienteSeleccionado)
            {
                // Asignamos solo el nombre (RazonSocial) al diccionario
                dato["Cliente factura"] = clienteSeleccionado.RazonSocial;

                // También puedes guardar la selección si lo necesitas para otros usos
                _respListarCoincidenciasDTOSeleccionada = clienteSeleccionado;
            }
            else
            {
                // Si no se pudo hacer el cast, tratamos de buscarlo por nombre
                var cliente = respListarCoincidencias.FirstOrDefault(c => c.RazonSocial.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase));
                if (cliente != null)
                {
                    dato["Cliente factura"] = cliente.RazonSocial;
                    _respListarCoincidenciasDTOSeleccionada = cliente;
                }
            }
        }

        private void OnServicioSeleccionado(object value, Dictionary<string, string> dato)
        {
            if (dato == null || value == null)
                return;

            if (value is CatServicios servicioSeleccionado)
            {
                dato["Servicio"] = servicioSeleccionado.Nombre;
                _servicioSeleccionado = servicioSeleccionado; // si necesitas guardarlo para otro uso
            }
            else
            {
                var servicio = serviciosDisponibles.FirstOrDefault(s => s.Nombre.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase));
                if (servicio != null)
                {
                    dato["Servicio"] = servicio.Nombre;
                    _servicioSeleccionado = servicio;
                }
            }
        }

        async Task AutoCompleteOnLoadServicio(LoadDataArgs args)
        {
            var strQuery = args.Filter ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(strQuery))
            {
                serviciosDisponibles = await iSLOReferenciasService.ObtenerServiciosPorCoincidenciaAsync(strQuery);
            }
            else
            {
                serviciosDisponibles = new List<CatServicios>();
            }
        }
        #endregion Autocompletado y excel

        //protected override async Task OnInitializedAsync()
        //{
        //    int idOrden = 0;
        //    var valorOrden = DiccionarioInfoReferencias.GetValueOrDefault("No.Orden")?.ToString();

        //    if (!string.IsNullOrWhiteSpace(valorOrden) && int.TryParse(valorOrden, out var id))
        //    {
        //        idOrden = id;
        //    }

        //    serviciosLigados = await iSLOReferenciasService.ObtenerServicios(idOrden);

        //    serviciosPlano = serviciosLigados
        //        .SelectMany(cont =>
        //        {
        //            // Capturamos la referencia asociada al contenedor
        //            var referencia = cont.SLOpeticionesReferencias;

        //            return cont.SLOpeticionesServicios
        //                .Where(serv => serv.Activo)
        //                .Select(serv =>
        //                {
        //                    cont.SLOpeticionesReferencias = referencia; // Reasignación explícita
        //                    serv.SLOpeticionesContenedores = cont;
        //                    return serv;
        //                });
        //        })
        //        .ToList();


        //    mostrarServiciosLigados = true;

        //    _datosExcel ??= new List<Dictionary<string, string>>();
        //    _columnas ??= new List<string>
        //    {
        //        "Moneda",
        //        "Contenedor",
        //        "BL",
        //        "Servicio",
        //        "Cantidad",
        //        "Subtotal",
        //        "Referencia cliente",
        //        "Cliente factura"
        //    };
        //}

        protected override async Task OnInitializedAsync()
        {
            //int idOrden = 0;
            //var valorOrden = DiccionarioInfoReferencias.GetValueOrDefault("No.Orden")?.ToString();

            //if (!string.IsNullOrWhiteSpace(valorOrden) && int.TryParse(valorOrden, out var id))
            //{
            //    idOrden = id;
            //}

            //var orden = await iSLOReferenciasService.ObtenerServicios(idOrden);

            //// Extraer los contenedores de la orden
            //serviciosLigados = orden.SLOpeticionesReferencias?
            //    .SelectMany(r => r.SLOpeticionesContenedores)
            //    .ToList() ?? new List<SLOPeticionesContenedores>();

            //serviciosPlano = serviciosLigados
            //    .SelectMany(cont =>
            //    {
            //        var referencia = cont.SLOpeticionesReferencias;

            //        return cont.SLOpeticionesServicios
            //            .Where(serv => serv.Activo)
            //            .Select(serv =>
            //            {
            //                cont.SLOpeticionesReferencias = referencia;
            //                serv.SLOpeticionesContenedores = cont;
            //                return serv;
            //            });
            //    })
            //    .ToList();

            //var objOrden = OrdenSeleccionada;

            if (OrdenSeleccionada != null)
            {
                serviciosLigados = OrdenSeleccionada.SLOpeticionesReferencias?
                    .SelectMany(r => r.SLOpeticionesContenedores)
                    .ToList() ?? new();

                serviciosPlano = serviciosLigados
                    .SelectMany(cont =>
                    {
                        var referencia = cont.SLOpeticionesReferencias;
                        return cont.SLOpeticionesServicios
                            .Where(s => s.Activo)
                            .Select(s =>
                            {
                                cont.SLOpeticionesReferencias = referencia;
                                s.SLOpeticionesContenedores = cont;
                                return s;
                            });
                    })
                    .ToList();
            }

            idsServiciosFacturados = OrdenSeleccionada?.SLOintegracionFacturaEnc?
               .SelectMany(enc => enc.SLOintegracionFacturaDet)
               .Where(det => det.Idpservicios != 0)
               .Select(det => det.Idpservicios)
               .ToHashSet() ?? new HashSet<int>();


            mostrarServiciosLigados = true;

            _datosExcel ??= new List<Dictionary<string, string>>();
            _columnas ??= new List<string>
    {
        "Moneda",
        "Contenedor",
        "BL",
        "Servicio",
        "Cantidad",
        "Subtotal",
        "Referencia cliente",
        "Cliente factura"
    };
        }

        //"Comentario encabezado",
        //        "Comentario partida"

        #region GridServicios ligados a la referencia
        private void ToggleSeleccionIndividual(SLOPeticionesServicios serv, bool seleccionado)
        {
            if (seleccionado)
                serviciosSeleccionados.Add(serv);
            else
                serviciosSeleccionados.Remove(serv);
        }

        private void ToggleSeleccionarTodos(bool seleccionar)
        {
            //seleccionarTodos = seleccionar;
            //if (seleccionar)
            //    serviciosSeleccionados = serviciosPlano.ToList();
            //else
            //    serviciosSeleccionados.Clear();
            seleccionarTodos = seleccionar;

            if (seleccionar)
            {
                serviciosSeleccionados = serviciosPlano
                    .Where(s => !idsServiciosFacturados.Contains(s.IdServicio))
                    .ToList();
            }
            else
            {
                serviciosSeleccionados.Clear();
            }
        }

        private async Task GuardarEdicion(SLOPeticionesServicios servicio)
        {
            if (_respListarCoincidenciasDTOSeleccionada.Id != 0)
                servicio.IdClienteFacturarA = _respListarCoincidenciasDTOSeleccionada.Id;

            if (servicio.Cantidad == 0)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Cantidad en cero",
                    Detail = "El valor de la cantidad no puede ser menor o igual a cero.",
                    Duration = 4000
                });
                return;
            }

            if (servicio.Monto == 0)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Monto en cero",
                    Detail = "El valor del subtotal no puede ser menor o igual a cero.",
                    Duration = 4000
                });
                return;
            }

            var respuesta = await iSLOReferenciasService.ActualizarServicioAsync(servicio);

            if (respuesta.IsSuccess)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Servicio actualizado",
                    Detail = respuesta.strMensaje,
                    Duration = 3000
                });
                await Recargar();

            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = respuesta.strMensaje,
                    Duration = 4000
                });
            }

            _respListarCoincidenciasDTOSeleccionada = new();
            servicioEnEdicion = null;
        }

        private void CancelarEdicionServicios(object serv)
        {
            //gridSolicitudesDetalle.CancelEditRow(servicio);
            //servicioEnEdicion = null;
            if (serv is SLOPeticionesServicios servicio && servicioBackup != null && servicio.IdServicio == servicioBackup.IdServicio)
            {
                servicio.Cantidad = servicioBackup.Cantidad;
                servicio.Monto = servicioBackup.Monto;
                servicio.ReferenciaClienteFactura = servicioBackup.ReferenciaClienteFactura;
                // Restaura otros campos si los estás permitiendo editar

                grid.CancelEditRow(servicio);
                servicioEnEdicion = null;
                servicioBackup = null;
                StateHasChanged();
                //Recargar();
            }
        }

        private async Task EliminarServicio(SLOPeticionesServicios servicio)
        {
            var confirm = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "¿Estás seguro?",
                Text = "Esta acción eliminará el servicio.",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí, eliminar",
                CancelButtonText = "Cancelar"
            });

            if (confirm.IsConfirmed)
            {
                var respuesta = await iSLOReferenciasService.EliminarServicioAsync(servicio.IdServicio);

                if (respuesta.IsSuccess)
                {
                    serviciosPlano.Remove(servicio);
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Servicio eliminado",
                        Detail = respuesta.strMensaje,
                        Duration = 3000
                    });
                }
                else
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Error al eliminar",
                        Detail = respuesta.strMensaje,
                        Duration = 4000
                    });
                }
                await Recargar();

                //StateHasChanged();
                //await OnInitializedAsync();
            }
        }

        private async Task Recargar()
        {
            int.TryParse(DiccionarioInfoReferencias.GetValueOrDefault("No.Orden")?.ToString(), out var idOrden);
            OrdenSeleccionada = await iSLOReferenciasService.ObtenerServicios(idOrden);

            // ⏪ Recarga estructuras internas
            serviciosLigados = OrdenSeleccionada.SLOpeticionesReferencias?
                .SelectMany(r => r.SLOpeticionesContenedores)
                .ToList() ?? new();

            serviciosPlano = serviciosLigados
                .SelectMany(cont =>
                {
                    var referencia = cont.SLOpeticionesReferencias;
                    return cont.SLOpeticionesServicios
                        .Where(s => s.Activo)
                        .Select(s =>
                        {
                            cont.SLOpeticionesReferencias = referencia;
                            s.SLOpeticionesContenedores = cont;
                            return s;
                        });
                }).ToList();

            idsServiciosFacturados = OrdenSeleccionada?.SLOintegracionFacturaEnc?
                .SelectMany(enc => enc.SLOintegracionFacturaDet)
                .Where(det => det.Idpservicios != 0)
                .Select(det => det.Idpservicios)
                .ToHashSet() ?? new();

            await InvokeAsync(StateHasChanged);


            //    foreach (var enc in OrdenSeleccionada?.SLOintegracionFacturaEnc ?? new List<SLOIntegraFacturaEnc>())
            //    {
            //        foreach (var det in enc.SLOintegracionFacturaDet)
            //        {
            //            if (det.Idpservicios != 0)
            //            {
            //                var servicio = serviciosPlano.FirstOrDefault(s => s.IdServicio == det.Idpservicios);
            //                if (servicio != null)
            //                {
            //                    // Asignar la referencia inversa de Encabezado a Detalle
            //                    det.SLOintegracionFacturaEnc = enc;

            //                    // Inicializar la colección si es null
            //                    servicio.SLOintegraFacturaDet ??= new List<SLOIntegraFacturaDet>();

            //                    // Agregar el detalle si aún no está
            //                    if (!servicio.SLOintegraFacturaDet.Any(d => d.IdIntFacturaDet == det.IdIntFacturaDet))
            //                    {
            //                        servicio.SLOintegraFacturaDet.Add(det);
            //                    }
            //                }
            //            }
            //        }
            //    }
        }

        #endregion GridServicios ligados a la referencia

        #region GridAgregarServicios
        private async Task AgregarNuevoRegistro()
        {
            if (_columnas == null || !_columnas.Any())
            {
                _columnas = new List<string>
                {
                    "Moneda",
                    "Contenedor",
                    "BL",
                    "Servicio",
                    "Cantidad",
                    "Subtotal",
                    "Referencia cliente",
                    "Cliente factura"
                };
            }
            //"Comentario encabezado",
            //        "Comentario partida"

            var hayRegistroVacio = _datosExcel.Any(d => d.Values.All(string.IsNullOrWhiteSpace));
            if (hayRegistroVacio)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Registro vacío",
                    Detail = "Ya tienes un registro sin llenar.",
                    Duration = 4000
                });
                return;
            }

            var nuevo = _columnas.ToDictionary(col => col, col => string.Empty);
            _datosExcel.Add(nuevo);
            _datosExcel = _datosExcel.ToList(); // Forzar actualización de UI

            //await gridDatos.EditRow(nuevo); // Entrar en modo edición automáticamente
            if (gridDatos != null)
            {
                await gridDatos.EditRow(nuevo);
            }

            mostrarServiciosLigados = false;
        }

        private async Task GuardarRegistroNuevo(Dictionary<string, string> registro)
        {
            // Validar que al menos una columna tenga datos
            bool tieneDatos = registro.Values.Any(v => !string.IsNullOrWhiteSpace(v) && v.ToUpper() != "N/A");

            if (!tieneDatos)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Registro vacío",
                    Detail = "Ya tienes un registro sin llenar.",
                    Duration = 4000
                });

                gridDatos.CancelEditRow(registro);
                _datosExcel.Remove(registro);
                return;
            }

            // Normalización de valores N/A o nulos
            var claves = registro.Keys.ToList();
            foreach (var key in claves)
            {
                var valor = registro[key];
                if (string.IsNullOrWhiteSpace(valor) || valor.ToUpper() == "N/A")
                {
                    registro[key] = string.Empty;
                }
            }
        }

        private void AceptarEdicion(Dictionary<string, string> item)
        {
            // Aquí puedes validar o guardar el cambio si deseas
            StateHasChanged();
        }

        private void EditarRegistro(Dictionary<string, string> item)
        {
            copiaTemporal = item.ToDictionary(entry => entry.Key, entry => entry.Value);
            gridDatos.EditRow(item);
        }

        private async Task CancelarEdicion(Dictionary<string, string> item)
        {
            //gridDatos.CancelEditRow(item);

            //// Si es un registro vacío nuevo y se cancela, lo quitamos
            //if (item.Values.All(string.IsNullOrWhiteSpace))
            //{
            //    _datosExcel.Remove(item);
            //    _datosExcel = _datosExcel.ToList();
            //}
            ////await OnInitializedAsync();
            // Restaurar valores previos
            foreach (var key in copiaTemporal.Keys)
            {
                if (item.ContainsKey(key))
                    item[key] = copiaTemporal[key];
            }

            gridDatos.CancelEditRow(item);

            // Si es un registro vacío nuevo, lo quitamos
            if (item.Values.All(string.IsNullOrWhiteSpace))
            {
                _datosExcel.Remove(item);
                _datosExcel = _datosExcel.ToList();
            }

            StateHasChanged();
        }

        private void EliminarRegistro(Dictionary<string, string> item)
        {
            if (_datosExcel.Contains(item))
            {
                _datosExcel.Remove(item);
                _datosExcel = _datosExcel.ToList();
                StateHasChanged();
            }
        }

        private async Task GuardarRegistrosAsync()
        {
            if (_datosExcel == null || !_datosExcel.Any())
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Sin datos",
                    Detail = "No hay datos para guardar.",
                    Duration = 3000
                });
                return;
            }

            var listaMonedas = await icatTipoMonedaService.ObtenerMonedas();
            var listaContenedores = new List<SLOPeticionesContenedores>();
            var referenciaALO = DiccionarioInfoReferencias.GetValueOrDefault("ReferenciaALO")?.ToString();

            foreach (var fila in _datosExcel)
            {
                //<--- SERVICIOS --->

                string nombreServicio = fila.GetValueOrDefault("Servicio")?.Trim();
                if (string.IsNullOrWhiteSpace(nombreServicio))
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Warning,
                        Summary = "Servicio vacío",
                        Detail = "Una fila no tiene nombre de servicio.",
                        Duration = 4000
                    });
                    return;
                }

                var serviciosCoincidentes = await iSLOReferenciasService.ObtenerServiciosPorCoincidenciaAsync(nombreServicio);
                var servicio = serviciosCoincidentes.FirstOrDefault(s =>
                    string.Equals(s.Nombre?.Trim(), nombreServicio, StringComparison.OrdinalIgnoreCase));

                if (servicio == null)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Servicio no encontrado",
                        Detail = $"No se encontró el servicio: {nombreServicio}",
                        Duration = 5000
                    });
                    return;
                }

                //<--- MONEDA --->

                string monedaStr = fila.GetValueOrDefault("Moneda")?.Trim();

                if (string.IsNullOrWhiteSpace(monedaStr))
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Warning,
                        Summary = "Moneda vacía",
                        Detail = "Una fila no tiene moneda.",
                        Duration = 4000
                    });
                    return;
                }

                var monedaValida = listaMonedas.FirstOrDefault(m =>
                    string.Equals(m.ClaveSAT?.Trim(), monedaStr, StringComparison.OrdinalIgnoreCase));

                if (monedaValida == null)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Moneda no válida",
                        Detail = $"La moneda '{monedaStr}' no es reconocida como clave SAT válida.",
                        Duration = 5000
                    });
                    return;
                }

                //<--- CANTIDAD --->

                var cantidadStr = fila.GetValueOrDefault("Cantidad")?.Trim();
                double cantidad = 0;
                if (!string.IsNullOrWhiteSpace(cantidadStr))
                {
                    double.TryParse(cantidadStr, out cantidad);
                }

                //<--- MONTO/SUBTOTAL --->

                // Conversión robusta para Subtotal
                var subtotalStr = fila.GetValueOrDefault("Subtotal")?.Trim();
                if (!string.IsNullOrWhiteSpace(subtotalStr))
                {
                    subtotalStr = subtotalStr.Replace(",", "").Replace("$", "");
                }

                if (!double.TryParse(subtotalStr, System.Globalization.NumberStyles.Any,
                                     System.Globalization.CultureInfo.InvariantCulture, out var subtotal))
                {
                    subtotal = 0;
                }

                //<--- CLIENTE FACTURA --->

                string nombreClienteFact = fila.GetValueOrDefault("Cliente factura")?.Trim();
                if (string.IsNullOrWhiteSpace(nombreClienteFact))
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Warning,
                        Summary = "Cliente factura vacío",
                        Detail = "Una fila no tiene nombre de Cliente a facturar.",
                        Duration = 4000
                    });
                    return;
                }

                var ClienteFactCoincidentes = await icatClientesService.GetClientesConincidencia(nombreClienteFact);
                var ClienteFact = ClienteFactCoincidentes.FirstOrDefault(s =>
                    string.Equals(s.RazonSocial?.Trim(), nombreClienteFact, StringComparison.OrdinalIgnoreCase));

                if (ClienteFact == null)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Cliente factura no encontrado",
                        Detail = $"No se encontró el Cliente a factura: {nombreClienteFact}",
                        Duration = 5000
                    });
                    return;
                }

                var contenedor = new SLOPeticionesContenedores
                {
                    SLOpeticionesReferencias = new SLOPeticionesReferencias
                    {
                        ordenes = new Ordenes
                        {
                            ReferenciaALO = referenciaALO
                        }
                    },
                    Contenedor = fila.GetValueOrDefault("Contenedor"),
                    BL = fila.GetValueOrDefault("BL"),
                    Moneda = monedaStr,
                    SLOpeticionesServicios = new List<SLOPeticionesServicios>
                    {
                        new SLOPeticionesServicios
                        {
                            IdCatServicio = servicio.IdCatServicio,
                            Cantidad = double.TryParse(fila.GetValueOrDefault("Cantidad"), out var cant) ? cant : 0,
                            //Monto = double.TryParse(fila.GetValueOrDefault("Subtotal"), out var subtotal) ? subtotal : 0,
                            Monto = subtotal,
                            IdClienteFacturarA = ClienteFact.Id,
                            ReferenciaClienteFactura = fila.GetValueOrDefault("Referencia cliente"),
                            Moneda = monedaStr
                        }
                    }
                };

                listaContenedores.Add(contenedor);
            }

            var respuesta = await iSLOReferenciasService.GuardarServicios(listaContenedores);

            if (respuesta.IsSuccess)
            {
                await sweetAlertService.FireAsync("Servicios guardados", respuesta.strMensaje, SweetAlertIcon.Success);


                int.TryParse(DiccionarioInfoReferencias.GetValueOrDefault("No.Orden")?.ToString(), out var idOrden);
                OrdenSeleccionada = await iSLOReferenciasService.ObtenerServicios(idOrden);

                // ⏪ Recarga estructuras internas
                serviciosLigados = OrdenSeleccionada.SLOpeticionesReferencias?
                    .SelectMany(r => r.SLOpeticionesContenedores)
                    .ToList() ?? new();

                serviciosPlano = serviciosLigados
                    .SelectMany(cont =>
                    {
                        var referencia = cont.SLOpeticionesReferencias;
                        return cont.SLOpeticionesServicios
                            .Where(s => s.Activo)
                            .Select(s =>
                            {
                                cont.SLOpeticionesReferencias = referencia;
                                s.SLOpeticionesContenedores = cont;
                                return s;
                            });
                    }).ToList();

                idsServiciosFacturados = OrdenSeleccionada?.SLOintegracionFacturaEnc?
                    .SelectMany(enc => enc.SLOintegracionFacturaDet)
                    .Where(det => det.Idpservicios != 0)
                    .Select(det => det.Idpservicios)
                    .ToHashSet() ?? new();

                //await Recargar();
                _datosExcel?.Clear();
                mostrarServiciosLigados = true;
                StateHasChanged();
            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error al guardar",
                    Detail = respuesta.strMensaje,
                    Duration = 6000
                });
            }
        }

        #endregion GridAgregarServicios

        #region Mandar a facturar
        private async Task AbrirModalFacturacion()
        {
            if (serviciosSeleccionados == null || !serviciosSeleccionados.Any())
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Advertencia",
                    Detail = "Selecciona al menos un servicio para enviar a facturar.",
                    Duration = 4000
                });
                return;
            }

            var result = await DialogService.OpenAsync<SLOEnviarAFacturarDialog>("Enviar a Facturar", new Dictionary<string, object>(), new DialogOptions
            {
                Width = "500px",
                CloseDialogOnOverlayClick = true
            });

            if (result is ValueTuple<string, string> comentarios)
            {
                comentarioEncabezado = comentarios.Item1;
                comentarioPartida = comentarios.Item2;

                await EnviarAFacturar();
            }
        }

        private async Task EnviarAFacturar()
        {
            if (serviciosSeleccionados == null || !serviciosSeleccionados.Any())
            {
                await js.InvokeVoidAsync("console.error", "No hay servicios seleccionados.");
                return;
            }

            // Clonamos la orden base
            var ordenFiltrada = new Ordenes
            {
                IdOrden = OrdenSeleccionada.IdOrden,
                ReferenciaALO = OrdenSeleccionada.ReferenciaALO,
                IdCatLineaNegocio = OrdenSeleccionada.IdCatLineaNegocio,
                SLOintegracionFacturaEnc = new List<SLOIntegraFacturaEnc>
        {
            new SLOIntegraFacturaEnc
            {
                Comentario = comentarioEncabezado,
                SLOintegracionFacturaDet = serviciosSeleccionados.Select(s => new SLOIntegraFacturaDet
                {
                    Comentario = comentarioPartida
                }).ToList()
            }
        }
            };

            // Agrupamos los servicios seleccionados por referencia y contenedor
            var agrupado = serviciosSeleccionados.GroupBy(s => new
            {
                ReferenciaId = s.SLOpeticionesContenedores?.SLOpeticionesReferencias?.IdReferencia,
                ContenedorId = s.SLOpeticionesContenedores?.IdContenedor
            });

            // Preparamos las referencias y contenedores con solo los servicios seleccionados
            var referenciasFiltradas = new List<SLOPeticionesReferencias>();

            foreach (var grupo in agrupado)
            {
                var primerServicio = grupo.First();
                var contenedorOriginal = primerServicio.SLOpeticionesContenedores;
                var referenciaOriginal = contenedorOriginal?.SLOpeticionesReferencias;

                if (contenedorOriginal == null || referenciaOriginal == null)
                {
                    await js.InvokeVoidAsync("console.warn", $"Faltan datos en grupo con No. Contenedor: {grupo.Key.ContenedorId}");
                    continue;
                }

                var serviciosFiltrados = grupo.Select(s => new SLOPeticionesServicios
                {
                    IdServicio = s.IdServicio,
                    Cantidad = s.Cantidad,
                    Monto = s.Monto,
                    Moneda = s.Moneda,
                    IdCatServicio = s.IdCatServicio,
                    IdClienteFacturarA = s.IdClienteFacturarA,
                    ReferenciaClienteFactura = s.ReferenciaClienteFactura
                }).ToList();

                var contenedorFiltrado = new SLOPeticionesContenedores
                {
                    IdContenedor = contenedorOriginal.IdContenedor,
                    Contenedor = contenedorOriginal.Contenedor,
                    BL = contenedorOriginal.BL,
                    Moneda = contenedorOriginal.Moneda,
                    Aduana = contenedorOriginal.Aduana,
                    SLOpeticionesServicios = serviciosFiltrados
                };

                foreach (var servicio in serviciosFiltrados)
                {
                    servicio.SLOpeticionesContenedores = contenedorFiltrado;
                }

                var referenciaFiltrada = new SLOPeticionesReferencias
                {
                    IdReferencia = referenciaOriginal.IdReferencia,
                    IdOrden = referenciaOriginal.IdOrden,
                    SLOpeticionesContenedores = new List<SLOPeticionesContenedores> { contenedorFiltrado },
                    ordenes = ordenFiltrada
                };

                contenedorFiltrado.SLOpeticionesReferencias = referenciaFiltrada;

                referenciasFiltradas.Add(referenciaFiltrada);
            }

            ordenFiltrada.SLOpeticionesReferencias = referenciasFiltradas;

            // Enviar al servicio toda la orden con solo los servicios seleccionados
            var respuesta = await iSLOReferenciasService.EnviarAFacturarAsync(ordenFiltrada);

            if (respuesta.IsSuccess)
            {
                await sweetAlertService.FireAsync("Éxito", respuesta.strMensaje, SweetAlertIcon.Success);
                await Recargar();
                //StateHasChanged();
            }
            else
            {
                await sweetAlertService.FireAsync("Error", respuesta.strMensaje, SweetAlertIcon.Error);
            }
        }


        #endregion Mandar a facturar
    }
}
