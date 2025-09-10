using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Orden;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Logisticos.Referencias
{
    public partial class SLOReferenciasPage
    {
        #region Variables

        [Inject] private ISLOReferenciasService iSLOReferenciasService { get; set; }
        [Inject] private ICatClientesService icatClientesService { get; set; }
        [Inject] private ICatAduanaService icatAduanaService { get; set; }
        [Inject] private UsuarioTokenDTO _UsuarioTokenDTO { get; set; }
        [Inject] private ILoginService iLoginService { get; set; }

        private List<SLOPeticionesContenedores> _lstContenedores = new();
        private List<Ordenes> _lstOrdenes = new();
        private List<CatClientes> _lstClientes = new();
        private RadzenDataGrid<Ordenes> grid;
        List<CatClientes> clientes = new();
        List<CatClientes> clientesFacturar = new();
        private IEnumerable<RespListarCoincidenciasDTO> respListarCoincidencias = new List<RespListarCoincidenciasDTO>();
        private IEnumerable<RespListarCoincidenciasDTO> respListarCoincidencias2 = new List<RespListarCoincidenciasDTO>();
        private ICollection<CatAduana> lstAduanas = new List<CatAduana>();
        private FiltroOrdenesReferenciasDTO _filtro = new();
        private RespListarCoincidenciasDTO selectedCliente;
        private RespListarCoincidenciasDTO selectedClienteFact;
        private RespListarCoincidenciasDTO _respListarCoincidenciasDTOSeleccionada;
        private RespListarCoincidenciasDTO _respListarCoincidenciasDTOSeleccionadaAct = new();

        private PeriodicTimer _timer;
        private CancellationTokenSource _cts = new();

        bool isLoading;
        private int intcliente = 0;
        private int intclienteFact = 0;
        private int intclienteA = 0;
        private int intclienteFactA = 0;
        private string strAduana;
        private int intAduana = 0;

        private string strcliente;
        private string strfactCliente;
        private string strRefalo;
        private string strRefcliente;
        private string strEnv1G;
        private int? selectedClienteId;
        private int IdLineaNegocio = 3; //Usarlo para la nueva versión 
        private bool puedeEditarClientes = true;
        private bool puedeEditarRef = true;

        DateTime? dFRegistro;
        DateTime? dFEnvio;

        int position = 1;

        #endregion Variables

        #region Filtro

        #region Autocompletar Cliente
        void OnClienteChange(object value)
        {
            selectedCliente = respListarCoincidencias
                .FirstOrDefault(c => c.RazonSocial == value?.ToString());

            if (selectedCliente != null)
            {
                intcliente = selectedCliente.Id;
                strcliente = selectedCliente.RazonSocial;
            }
            else
            {
                intcliente = 0;
                strcliente = null;
            }
        }
        //void OnClienteFactChange(object value)
        //{
        //    selectedClienteFact = respListarCoincidencias2
        //        .FirstOrDefault(c => c.RazonSocial == value?.ToString());

        //    if (selectedClienteFact != null)
        //    {
        //        intclienteFact = selectedClienteFact.Id;
        //        strfactCliente = selectedClienteFact.RazonSocial;
        //    }
        //    else
        //    {
        //        intclienteFact = 0;
        //        strfactCliente = null;
        //    }
        //}

        async Task AutoCompleteOnLoadCliente(LoadDataArgs args)
        {
            //var strQuery = args.Filter;
            //respListarCoincidencias = await icatClientesService.GetClientesConincidencia(strQuery);
            //respListarCoincidencias2 = await icatClientesService.GetClientesConincidencia(strQuery);
            var strQuery = args.Filter ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(strQuery))
            {
                respListarCoincidencias = await icatClientesService.GetClientesConincidencia(strQuery);
                // respListarCoincidencias2 = respListarCoincidencias.ToList();
            }
            else
            {
                // Para evitar error si no hay filtro, puedes usar una lista vacía
                respListarCoincidencias = new List<RespListarCoincidenciasDTO>();
                //respListarCoincidencias2 = new List<RespListarCoincidenciasDTO>();
            }
        }

        #endregion Autocompletar Cliente

        void DropDownOnAduanaChange()
        {

            try
            {
                if (strAduana.Length > 0)
                {
                    intAduana = lstAduanas.Where(x => x.Nombre.Equals(strAduana)).FirstOrDefault().IdCatAduana;
                }
                else intAduana = 0;

            }
            catch (Exception ex)
            {
                intAduana = 0;

            }
        }

        #endregion Filtro

        DataGridExpandMode expandMode = DataGridExpandMode.Single;

        void Change(string text)
        {
        }

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            _respListarCoincidenciasDTOSeleccionada = new RespListarCoincidenciasDTO();

            _UsuarioTokenDTO = await iLoginService.ObtenerdatosToken();
            //respListarCoincidencias2 = respListarCoincidencias != null
            //                        ? respListarCoincidencias.ToList()
            //                        : new List<RespListarCoincidenciasDTO>();

            lstAduanas = await icatAduanaService.GetAduanas();
            _filtro = new FiltroOrdenesReferenciasDTO();

            //dFRegistro = DateTime.Now;
            //dFEnvio = DateTime.Now;
            intcliente = 0;
            intclienteFact = 0;


            if (_UsuarioTokenDTO.catUsuarios.catUsuariosClientes != null && _UsuarioTokenDTO.catUsuarios.catUsuariosClientes.Count > 0)
            {
                intcliente = _UsuarioTokenDTO.catUsuarios.catUsuariosClientes?.FirstOrDefault()?.IdCatCliente ?? 0;
                intclienteFact = _UsuarioTokenDTO.catUsuarios.catUsuariosClientes?.FirstOrDefault()?.IdCatCliente ?? 0;
            }

            if (_UsuarioTokenDTO == null)
            {
                // puedes lanzar un mensaje o redirigir
                Console.WriteLine("No hay sesión activa.");
                return;
            }

            await ObtenerDatos();

            isLoading = false;

            _ = RefrescarPeriodicamente(_cts.Token);
        }

        //async Task ConsultarReferencias()
        //{
        //    filtro = new FiltroOrdenesReferenciasDTO
        //    {
        //        ReferenciaALO = strPrefalo,
        //        ReferenciaCliente = strPrefcliente,
        //        IdCliente = selectedClienteId,
        //        FechaSolicitudIni = dFRegistro,
        //        FechaSolicitudFin = dFEnvio
        //        // Puedes completar los demás campos que necesites
        //    };

        //    try
        //    {
        //        var resultado = await servicioReferencias.ConsultarReferencias(filtro);
        //        // Aquí puedes mostrar los resultados en un gridSolicitudesDetalle
        //    }
        //    catch (Exception ex)
        //    {
        //        await Swal.FireAsync("Error", "Ocurrió un error al consultar las referencias", "error");
        //    }
        //}

        private async Task ObtenerDatos()
        {
            isLoading = true;

            _filtro = new FiltroOrdenesReferenciasDTO
            {
                IdCliente = intcliente > 0 ? intcliente : null,
                IdClienteFact = intclienteFact > 0 ? intclienteFact : null,
                ReferenciaALO = strRefalo,
                ReferenciaCliente = strRefcliente,
                IdAduana = intAduana > 0 ? intAduana : null,
                FechaRegistro = dFRegistro,
                FechaEnvio1G = dFEnvio,
                Estado1G = strEnv1G
            };
            var referencias = await iSLOReferenciasService.ObtenerReferencias(_filtro);
            _lstOrdenes = referencias
                .ToList();
            isLoading = false;
        }

        private async Task FiltrarDatos()
        {
            await ObtenerDatos();
        }

        private async Task IniciarEdicion(Ordenes orden)
        {
            puedeEditarClientes = await ValidacionesEditClientes(orden);
            //puedeEditarRef = await ValidacionesEditReferencias(orden);
            grid.EditRow(orden);
        }

        private async Task AbrirModalDetalles(Ordenes orden)
        {
            var diccionario = new Dictionary<string, object>
            {
                { "No.Orden", orden.IdOrden },
                { "ReferenciaALO", orden.ReferenciaALO },
                { "Aduana", orden.catAduana.Nombre }
            };

            await DialogService.OpenAsync<SLODetalleReferenciaCMP>(
                title: "Detalle de Referencia",
                parameters: new Dictionary<string, object>
                {
                    { "DiccionarioInfoReferencias", diccionario },
                    { "OrdenSeleccionada", orden }
                },
                options: new DialogOptions
                {
                    Width = "1200px",       //1200px
                    CloseDialogOnOverlayClick = false,
                    ShowClose = true
                }
            );
        }

        private async Task GuardarCambios(Ordenes orden)
        {
            //var integracion = orden.SLOintegracionReferencias.FirstOrDefault();
            int IdCliente = 0;

            if (_respListarCoincidenciasDTOSeleccionada?.Id != 0)
            {
                IdCliente = _respListarCoincidenciasDTOSeleccionada.Id;
            }

            if (orden.SLOpeticionesReferencias == null)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "No se encontró los datos para esta orden."
                });
                return;
            }

            var ClienteSolic = new Ordenes
            {
                IdOrden = orden.IdOrden,
                IdCatCliente = IdCliente
                // Asegúrate de incluir cualquier otro campo requerido por el endpoint si es necesario
            };

            var resultado = await iSLOReferenciasService.EditarReferenciaCliente(ClienteSolic);

            if (resultado.IsSuccess)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Cliente actualizado",
                    Detail = resultado.strMensaje
                });

                // Opcional: refrescar datos si deseas que se actualice el gridSolicitudesDetalle
                await ObtenerDatos();
            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = resultado.strMensaje
                });
            }
        }

        private async Task<bool> ValidacionesEditClientes(Ordenes orden)
        {
            var integracion = orden.SLOintegracionReferencias?.FirstOrDefault();

            if (orden.SLOpeticionesReferencias == null)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error de validación",
                    Detail = "No se encontró información para esta orden."
                });
                return false;
            }

            // Validación: Cliente y Cliente a facturar no editables si Estado == 1
            if (integracion?.Enviado == true)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "No editable",
                    Detail = "Cliente Solicitante no pueden modificarse con Estado 1G igual a Procesado."
                });
                return false;
            }

            return true;
        }

        //private async Task<bool> ValidacionesEditReferencias(Ordenes orden)
        //{
        //    var integracion = orden.SLOintegracionReferencias.FirstOrDefault();

        //    // Llama al servicio que obtiene todas las facturas
        //    var facturas = await iSLOReferenciasService.ObtenerFacturas(orden.IdOrden);

        //    if (integracion == null)
        //    {
        //        NotificationService.Notify(new NotificationMessage
        //        {
        //            Severity = NotificationSeverity.Error,
        //            Summary = "Error de validación",
        //            Detail = "No se encontró información de integración para esta orden."
        //        });
        //        return false;
        //    }

        //    if (facturas.Any())
        //    {
        //        NotificationService.Notify(new NotificationMessage
        //        {
        //            Severity = NotificationSeverity.Warning,
        //            Summary = "No editable",
        //            Detail = "La Referencia Cliente no se puede modificar porque ya tiene mínimo una factura ligada."
        //        });
        //        return false;
        //    }

        //    return true;
        //}

        private void OnClienteSeleccionado(object value, Ordenes orden)
        {
            var cliente = respListarCoincidencias.FirstOrDefault(c => c.RazonSocial.Equals(value.ToString()));

            if (cliente != null)
            {
                _respListarCoincidenciasDTOSeleccionada = cliente;
                orden.catClientes = new CatClientes
                {
                    IdCatCliente = cliente.Id,
                    RazonSocial = cliente.RazonSocial
                };
            }
        }

        private async Task AbrirVentanaCrearReferencia()
        {
            await DialogService.OpenAsync<SLOCrearReferenciaDialog>(
                "Crear Nueva Referencia",
                new Dictionary<string, object>(), // Puedes pasar parámetros si es necesario
                new DialogOptions { Width = "800px", Height = "auto", Resizable = true }
            );
        }

        private async Task RefrescarPeriodicamente(CancellationToken token)
        {
            _timer = new PeriodicTimer(TimeSpan.FromMinutes(2)); // Cambia el intervalo si lo deseas

            try
            {
                while (await _timer.WaitForNextTickAsync(token))
                {
                    await InvokeAsync(async () =>
                    {
                        isLoading = true;
                        await ObtenerDatos(); // tu método ya existente
                        isLoading = false;

                        StateHasChanged();
                    });
                }
            }
            catch (OperationCanceledException)
            {
                // Timer cancelado
            }
        }
        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _timer?.Dispose();
        }
    }
}
