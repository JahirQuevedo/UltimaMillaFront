using OfficeOpenXml;
using Radzen.Blazor;
using Radzen;
using Microsoft.AspNetCore.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRespositorios.Modelos.Dtos.Control;
using Microsoft.JSInterop;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using System.Text.RegularExpressions;
using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Catalogos;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using AlogisticsWASM.Pages.Vacios.Referencias;
using System.Text.Json;


namespace AlogisticsWASM.Pages.Vacios.Solicitudes
{
    public partial class SolicitudesPage : ComponentBase
    {

        private Dictionary<int, List<int>> _serviciosSeleccionadosPorTicket;
        private Dictionary<string, bool> _dicAcceso;
        private IList<SolTicketDTO> _solicitudesSeleccionadas;
        private List<CatServiciosDTO> _serviciosSeleccionados;
        private List<int> idsServiciosSeleccionados;
        private ICollection<CatTipoMoneda> _monedas;
        private List<SolTicketDTO> _solicitudes;
        private ICollection<CatAduana> _aduanas;

        private RadzenDataGrid<SolTicketDTO> _grid;
        private string _claveMonedaSeleccionada;
        private bool _allowRowSelectOnRowClick;
        private int _idAduanaSeleccionada;
        private bool _tieneAcceso;
        private bool _estaCargando;

        [Parameter][EditorRequired] public bool ParamMostrarControler { get; set; }

        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private ICatAduanaService CatAduanaService { get; set; }
        [Inject] private ICatTipoMonedaService MonedaService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private UsuarioTokenDTO UsuarioTokenDTO { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IOrdenService OrdenService { get; set; }
        [Inject] private ILoginService LoginService { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }


        protected override async Task OnInitializedAsync()
        {

            _serviciosSeleccionadosPorTicket = new Dictionary<int, List<int>>();
            _dicAcceso = new Dictionary<string, bool>();
            _serviciosSeleccionados = new List<CatServiciosDTO>();
            idsServiciosSeleccionados = new List<int>();
            _solicitudes = new List<SolTicketDTO>();
            _monedas = new List<CatTipoMoneda>();
            _allowRowSelectOnRowClick = false;

            _tieneAcceso = false;
            _estaCargando = true;

            await ObtenerDatosUsuarioAsync();

            _monedas = await MonedaService.ObtenerMonedas();
            _aduanas = await CatAduanaService.GetAduanas();

            foreach (var solicitud in _solicitudes)
            {
                if (!_serviciosSeleccionadosPorTicket.ContainsKey(solicitud.Id))
                {
                    _serviciosSeleccionadosPorTicket[solicitud.Id] = new List<int>();
                }
            }

            _estaCargando = false;

        }
        private async Task ObtenerDatosUsuarioAsync()
        {

            try
            {
                var token = await JsRuntime.InvokeAsync<string>("localStorage.getItem", "JWT Token");
                if (!string.IsNullOrEmpty(token))
                {
                    if (UsuarioTokenDTO.IdCatUsuario == 0)
                    {
                        UsuarioTokenDTO = await LoginService.ObtenerdatosToken();

                        if (!await LoginService.validarAccesoPagina(8, UsuarioTokenDTO))
                        {
                            NavigationManager.NavigateTo("/");
                        }
                        else
                        {
                            _dicAcceso = await LoginService.validarControlesPagina(8, UsuarioTokenDTO);
                            _tieneAcceso = _dicAcceso["CREAR"];
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("APP.RAZOR:" + ex.Message);
            }
        }
        private async Task CargarExcelAsync(UploadChangeEventArgs e)
        {
            _solicitudes.Clear();
            var file = e.Files.FirstOrDefault();
            Regex regex = new Regex(@"[^A-Za-z]");

            if (file != null)
            {
                using (var stream = new MemoryStream())
                {
                    await file.OpenReadStream().CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            // Verificar si las celdas requeridas están vacías
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text) ||
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text) ||
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].Text) ||
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 4].Text) ||
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 5].Text))
                            {
                                continue; // Saltar esta fila si alguna celda requerida está vacía
                            }
                            string referenciaCliente = regex.Replace(worksheet.Cells[row, 3].Text, "").ToUpper();
                            referenciaCliente = worksheet.Cells[row, 3].Text;
                            SolTicketDTO ticket = new SolTicketDTO
                            {
                                Id = row,
                                Ticket = int.TryParse(worksheet.Cells[row, 1].Text, out int tick) ? tick : 0,
                                Contenedor = worksheet.Cells[row, 2].Text,
                                ReferenciaCliente = referenciaCliente,
                                ReferenciaClienteFacturar = referenciaCliente,
                                ClaveTipoContenedor = worksheet.Cells[row, 4].Text,
                                RfcCLienteFacturar = worksheet.Cells[row, 5].Text
                            };
                            _solicitudes.Add(ticket);
                        }
                    }
                }
                await ActualizarTabla();
            }
        }
        private async Task ProcesarDatosAsync()
        {

            bool solicitudValida = false;

            if (!_solicitudes.Any())
            {
                await MostrarAlertaAsync("¡Advertencia!", "No existen solicitudes disponibles para procesar", SweetAlertIcon.Warning);
                return;
            }

            solicitudValida = await ValidarSolicitudesAsync();

            if (solicitudValida)
            {
                await GuardarSolicitudesAsync();
            }

        }
        private async Task<bool> ValidarSolicitudesAsync()
        {

            bool solicitudValida = true;
            int idCatUsuario = UsuarioTokenDTO != null ? (int)UsuarioTokenDTO.IdCatUsuario : 0;
            List<string> errores = new List<string>();

            foreach (var solicitud in _solicitudes)
            {

                solicitud.IdCatEmpresa = 1;

                solicitud.IdCatLineaNegocio = 1;

                solicitud.IdCatUsuario = idCatUsuario;

                solicitud.Estatus = "Procesando";

                solicitud.IdCatAduana = _idAduanaSeleccionada;

                solicitud.Moneda = _claveMonedaSeleccionada;

                solicitud.IdCatClienteSolicitante = (int)UsuarioTokenDTO.IdCatUsuario;

                solicitud.Errores.Clear();

                if (solicitud.Servicios is null || !solicitud.Servicios.Any())
                {
                    solicitud.Errores.Add($"La solicitud {solicitud.Ticket} no tiene servicios asignados");
                    //await MostrarAlertaAsync("¡Advertencia!", $"La solicitud {solicitud.Ticket} no tiene servicios asignados", SweetAlertIcon.Warning);
                    errores.Add($"La solicitud {solicitud.Ticket} no tiene servicios asignados");
                    solicitud.Estatus = "Rechazado";
                }
                await OrdenService.ValidarSolicitud(solicitud);

                if (solicitud.Estatus.Equals("Rechazado"))
                {
                    solicitudValida = false;
                }
            }

            if (solicitudValida == false)
            {
                OnClick("Error al validar solicitudes", errores, NotificationSeverity.Error);
            }

            await ActualizarTabla();

            return solicitudValida;
        }
        private async Task GuardarSolicitudesAsync()
        {

            string referenciaCliente = "NA";
            List<SolTicketDTO> solicitudesSinReferencia = new List<SolTicketDTO>();

            foreach (var solicitud in _solicitudes)
            {
                if (solicitud.Estatus.Equals("Válido"))
                {
                    if (solicitud.ReferenciaCliente.Equals("NA"))
                    {
                        solicitudesSinReferencia.Add(solicitud);
                        continue;
                    }
                    await OrdenService.GenerarSolicitud(solicitud);
                }
            }

            foreach (var solicitud in solicitudesSinReferencia)
            {
                if (solicitud.Estatus.Equals("Válido"))
                {

                    solicitud.ReferenciaCliente = referenciaCliente;
                    await OrdenService.GenerarSolicitud(solicitud);

                    if (referenciaCliente.Equals("NA"))
                    {
                        referenciaCliente = solicitud.ReferenciaAlo;
                    }

                    solicitud.ReferenciaCliente = referenciaCliente;
                }
            }

            await ActualizarTabla();
        }

        private async Task AbrirModalServiciosAsync()
        {
            if (_solicitudesSeleccionadas == null || !_solicitudesSeleccionadas.Any())
            {
                await MostrarAlertaAsync("¡Advertencia!", "Debe marcar al menos una casilla para asignar servicios a la solicitud", SweetAlertIcon.Warning);
                return;
            }

            var result = await DialogService.OpenAsync<AsignarServiciosCMP>("Asignar Servicios", null);

            if (result != null)
            {
                var selectedServicios = (List<CatServicios>)result;

                foreach (var solicitud in _solicitudesSeleccionadas)
                {
                    if (!_serviciosSeleccionadosPorTicket.ContainsKey(solicitud.Id))
                    {
                        _serviciosSeleccionadosPorTicket[solicitud.Id] = new List<int>();
                    }
                    _serviciosSeleccionadosPorTicket[solicitud.Id] = selectedServicios.Select(s => s.IdCatServicio).ToList();
                    solicitud.Servicios = new List<CatServicios>(selectedServicios);
                }
            }

        }

        private void ActualizarServiciosSeleccionados(SolTicketDTO solicitud)
        {

            if (!_serviciosSeleccionadosPorTicket.ContainsKey(solicitud.Id))
            {
                _serviciosSeleccionadosPorTicket[solicitud.Id] = new List<int>();
            }

            solicitud.Servicios = solicitud.Servicios
                .Where(s => _serviciosSeleccionadosPorTicket[solicitud.Id].Contains(s.IdCatServicio))
                .ToList();

            //if (_serviciosSeleccionadosPorTicket.TryGetValue(solicitud.Id, out var serviciosSeleccionados)) {
            //    // Filtrar solo los servicios seleccionados en la lista de servicios de la solicitud
            //    solicitud.Servicios = solicitud.Servicios
            //        .Where(s => serviciosSeleccionados.Contains(s.IdCatServicio))
            //        .ToList();
            //}
        }
        private async Task EliminarSolicitudesAsync()
        {

            if (!await ConfirmarEliminacion())
            {
                return;
            }

            try
            {

                if (_solicitudesSeleccionadas is null || !_solicitudesSeleccionadas.Any())
                {
                    _solicitudes.Clear();
                }
                else
                {
                    _solicitudes = _solicitudes.Except(_solicitudesSeleccionadas).ToList();
                    _solicitudesSeleccionadas.Clear();
                }

                NotificationService.Notify(
                    new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Éxito",
                        Detail = "Solicitudes eliminadas correctamente."
                    }
                );
                await ActualizarTabla();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = $"No se pudo eliminar la solicitud: {ex.Message}"
                });
            }
        }
        private async Task ActualizarTabla()
        {
            StateHasChanged();
            await _grid.Reload();
        }
        private void EliminarServicio(SolTicketDTO solicitud, CatServiciosDTO servicio)
        {

            var servicioEliminar = solicitud.Servicios.FirstOrDefault(s => s.IdCatServicio == servicio.IdCatServicio);

            if (servicioEliminar != null)
            {
                solicitud.Servicios.Remove(servicioEliminar); // Elimina solo el servicio seleccionado
                StateHasChanged(); // Actualiza el estado del componente
            }
        }
        private async Task MostrarAlertaAsync(string titulo, string mensage, SweetAlertIcon icon)
        {
            await Swal.FireAsync(titulo, mensage, icon);
        }
        private async Task<bool> ConfirmarEliminacion()
        {

            if (!_solicitudes.Any())
            {
                await MostrarAlertaAsync("¡Advertencia!", "No existen solicitudes disponibles para eliminar", SweetAlertIcon.Warning);
                return false;
            }

            var result = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "¿Estás seguro?",
                Text = "¡No podrás revertir esto!",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí, eliminar",
                CancelButtonText = "No, cancelar"
            });
            return !string.IsNullOrEmpty(result.Value);
        }
        private void OnClick(string titulo, List<string> mensages, NotificationSeverity notificationSeverity)
        {

            string mensajeHtml = "<ul>";
            foreach (var mensaje in mensages)
            {
                mensajeHtml += $"<li>{mensaje}</li>";
            }
            mensajeHtml += "</ul>";

            NotificationService.Notify(new NotificationMessage { Severity = notificationSeverity, Summary = titulo, Detail = mensajeHtml });
        }
        private async Task EliminarSolicitudAsync(SolTicketDTO solicitud)
        {
            if (await ConfirmarEliminacion() && _solicitudes.Contains(solicitud))
            {
                _solicitudes.Remove(solicitud);
                await ActualizarTabla();
            }
        }

        #region Modal
        async Task OpenModal()
        {
            var parameters = new Dictionary<string, object> { };
            //parameters.Add("paramstrTipoAccion", "C");
            var options = new DialogOptions
            {
                Width = "45%", // Cambia el ancho del modal
                Height = "50%" // Cambia la altura del modal
            };
            var objResultModal = await DialogService.OpenAsync<SolicitudesCRUDCMP>("Agregar, Actualizar Contenedores", parameters, options);

            if (objResultModal != null)
            {
                Console.WriteLine("OpenModal:" + JsonSerializer.Serialize(objResultModal));
                var objTicketRespuesta = (SolTicketDTO)objResultModal;
                var objRow = _solicitudes.Count() + 1;
                var objDuplicado = _solicitudes.Any(x => x.Contenedor.Equals(objTicketRespuesta.Contenedor));
                if (objDuplicado)
                {
                    await Swal.FireAsync("Error", "El contenedor ya existe", SweetAlertIcon.Error);
                    return;
                }
                else
                {
                    SolTicketDTO objTicket = new SolTicketDTO
                    {
                        Id = objRow,
                        Ticket = objTicketRespuesta.Ticket,
                        Contenedor = objTicketRespuesta.Contenedor,
                        ReferenciaCliente = objTicketRespuesta.ReferenciaCliente,
                        ReferenciaClienteFacturar = objTicketRespuesta.ReferenciaClienteFacturar,
                        ClaveTipoContenedor = objTicketRespuesta.ClaveTipoContenedor,
                        RfcCLienteFacturar = objTicketRespuesta.RfcCLienteFacturar
                    };
                    _solicitudes.Add(objTicket);
                    await ActualizarTabla();
                }

            }
        }


        #endregion Modal

    }
}
