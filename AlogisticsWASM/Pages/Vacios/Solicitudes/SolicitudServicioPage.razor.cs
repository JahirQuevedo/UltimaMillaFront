using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Utilerias;
using ALOG.Modelos.Modelos.DTO.Vacios;
using AlogisticsWASM.Layout.Utilerias;
using AlogisticsWASM.Layout.Vacios.Patios;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Control.IControl;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using OfficeOpenXml;
using Radzen;
using Radzen.Blazor;
using System.Net;

namespace AlogisticsWASM.Pages.Vacios.Solicitudes
{
    public partial class SolicitudServicioPage
    {

        [Inject] NotificationService NotificationService { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }
        [Inject] private IOrdenService OrdenService { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ILoginService LoginService { get; set; }
        [Inject] private UsuarioTokenDTO UsuarioTokenDTO { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private TooltipService TooltipService { get; set; }
        [Inject] private ICatTipoContenedorService TipoContenedorService { get; set; }

        [Inject] public IControlService ControlService { get; set; }
        [Inject] public ICatPatiosServices PatioService { get; set; }
        [Inject] public ICatNavieraService NavieraService { get; set; }

        private const string WIDTH_COLUM_RFC = "170px";
        private const string WIDTH_COLUM_170 = "170px";

        private List<PeticionesContenedoresClienteExternoDTO> _contenedoresManiobras;
        private List<PeticionesContenedoresClienteExternoDTO> _contenedoresEir;

        private List<PeticionesContenedoresClienteExternoDTO> _contenedoresManiobrasSeleccionados;
        private List<PeticionesContenedoresClienteExternoDTO> _contenedoresEirSeleccionados;
        private ICollection<CatPatios> _patios;

        private List<string> _encabezadosExcel;
        private List<string> _errores;

        private GridPeticionContenedorCmp _gridContenedorCmpMV;
        private GridPeticionContenedorCmp _gridContenedorCmpEIR;

        private RadzenUpload _uploadMV;
        private RadzenUpload _uploadEIR;
        private Radzen.FileInfo _fileInfoBl;

        private ExcelWorksheet _sheet;
        private bool _isLoading;
        private int _indexTab;
        private bool _existeNavieraHyndai;
        private bool _existePatioHazesa;

        private RadzenUpload _uploadBl;
        private RadzenUpload _uploadEir;

        private bool _contenedoresTienenDocumentos;

        private List<PeticionesContenedoresClienteExternoDTO> _seleccionadosMV = new();
        private List<PeticionesContenedoresClienteExternoDTO> _seleccionadosEIR = new();
        private ICollection<CatTipoContenedor> _tiposContenedor;
        private ICollection<CatNavieras> _navieras;

        private const string HAZESA_RFC = "TSH081231SK5";

        private bool _procesandoDatos;

        private bool _estaCargando;
        private dynamic _modalReferenciaCargando;
        private bool _esClienteExterno;

        IEnumerable<int> _checkGestiones;

        private Dictionary<string, bool> _servicioGD = new();
        private Dictionary<string, bool> _servicioGG = new();

        private List<CatClientes> _clientesSolicitantes;
        private int _idCatClienteSolicitanteSeleccionado;
        private PeticionesReferenciasClienteExternoDTO _referencia;
        private ElementReference _gdHeader;
        private ElementReference _ggHeader;
        private FiltroPatioDTO _filtroPatio;
        private CatPatios _patioSeleccionado;
        private int _keyGrid = 0;

        protected override async Task OnInitializedAsync()
        {
            _errores = new List<string>();
            _isLoading = true;
            _referencia = new PeticionesReferenciasClienteExternoDTO();
            _contenedoresManiobras = new List<PeticionesContenedoresClienteExternoDTO>();
            _contenedoresEir = new List<PeticionesContenedoresClienteExternoDTO>();
            _contenedoresManiobrasSeleccionados = new List<PeticionesContenedoresClienteExternoDTO>();
            _contenedoresEirSeleccionados = new List<PeticionesContenedoresClienteExternoDTO>();
            _tiposContenedor = new List<CatTipoContenedor>();
            _navieras = new List<CatNavieras>();
            _indexTab = 0;
            _existeNavieraHyndai = false;
            _existePatioHazesa = false;
            _contenedoresTienenDocumentos = false;
            _filtroPatio = new FiltroPatioDTO();
            _patioSeleccionado = new CatPatios();
            _procesandoDatos = false;
            await ObtenerDatosUsuarioAsync();
            _tiposContenedor = await TipoContenedorService.GetTiposContenedorAsync();
            InicializarListaEncabezadosExcel();
            InicializarClientesSolicitantes();
            string filtroCifrado = await ControlService.GetFiltroCifrado(_filtroPatio);
            _patios = await PatioService.ObtenerListaPatiosFiltrados(filtroCifrado) ?? new List<CatPatios>();
            _navieras = await NavieraService.GetNavieras();
            //_isLoading = true;
            await Task.Delay(100);
            _isLoading = false;
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

                        var roles = UsuarioTokenDTO?.catUsuarios?.catUsuarioRoles;
                        var empresas = UsuarioTokenDTO?.catUsuarios?.catUsuariosEmpresas;
                        var idCatUsuario = UsuarioTokenDTO?.catUsuarios?.IdCatUsuarios;
                        var usuarioRfc = UsuarioTokenDTO?.catUsuarios?.RFC;
                        // ADMINUSER ==> USUARIO INTERNO
                        _esClienteExterno = roles.Any(r => r.CatRoles.Nombre.Equals("CLIENTE"));

                        if (!await LoginService.validarAccesoPagina(2, UsuarioTokenDTO))
                        {
                            NavigationManager.NavigateTo("/");
                        }
                        else
                        {
                            //_dicAcceso = await LoginService.validarControlesPagina(8, UsuarioTokenDTO);
                            //_tieneAcceso = _dicAcceso["CREAR"];
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "¡Error al obtener datos del usuario!",
                    Detail = "Ocurrió un error inesperado al cargar los datos del usuario. Por favor, recargar el sitio o inténtalo visitar el sitio mas tarde.",
                    Duration = 8000
                });
            }
        }

        private void OnPatioSeleccionado(object value)
        {
            string razonSocial = value.ToString() ?? "";
            _patioSeleccionado = _patios.FirstOrDefault(t => t.RazonSocial.Equals(razonSocial)) ?? new CatPatios();
        }

        private void InicializarClientesSolicitantes()
        {

            _clientesSolicitantes = new List<CatClientes> {
                new CatClientes {
                    IdCatCliente = 189,
                    RazonSocial = "NAD GLOBAL",
                    RFC = "NGL0712111M2"
                }
                //,
                //new CatClientes {
                //    IdCatCliente = 149,
                //    RazonSocial = "WELLDEX INTERNACIONAL",
                //    RFC = "WIN100618RG4"
                //}
            };

        }

        private async void MostrarModalCarga()
        {
            _estaCargando = true;
            _modalReferenciaCargando = await DialogService.OpenAsync<ModalLoadingCmp>(
                title: null,
                parameters: new Dictionary<string, object> { { "Mensaje", "Procesando datos, por favor espere..." } },
                options: new DialogOptions
                {
                    Width = "400px",
                    ShowClose = false,
                    CloseDialogOnOverlayClick = false,
                    Draggable = false,
                    Resizable = false
                });
        }

        private void CerrarModalCarga()
        {
            _estaCargando = false;
            DialogService.Close();
            _modalReferenciaCargando = null;
        }

        private void OnSeleccionadosActualizados(List<PeticionesContenedoresClienteExternoDTO> seleccionados)
        {
            if (_indexTab == 0)
                _seleccionadosMV = seleccionados;
            else if (_indexTab == 1)
                _seleccionadosEIR = seleccionados;
        }

        private async Task OnDocumentoCargado(UploadChangeEventArgs args)
        {
            try
            {
                MostrarModalCarga();
                _fileInfoBl = args.Files.FirstOrDefault();
                if (_fileInfoBl == null)
                {
                    IJsHelper.MostrarNotificacion(
                        NotificationService,
                        "Archivo no válido",
                        "Debes seleccionar un documento PDF.",
                        NotificationSeverity.Error,
                        4000
                    );
                    CerrarModalCarga();
                    return;
                }

                if (_fileInfoBl.Size > ExcelService.TAMANIO_PERMITIDO)
                {
                    IJsHelper.MostrarNotificacion(
                        NotificationService,
                        "Error al cargar documento",
                        "El documento cargado excede el tamaño permitido (10 MB)",
                        NotificationSeverity.Error,
                        5000
                    );

                    if (_uploadBl != null) await _uploadBl?.ClearFiles();
                    if (_uploadEir != null) await _uploadEir?.ClearFiles();

                    CerrarModalCarga();
                    return;
                }

                using var stream = new MemoryStream();
                await _fileInfoBl.OpenReadStream(maxAllowedSize: ExcelService.TAMANIO_PERMITIDO).CopyToAsync(stream);

                // Simulamos ruta en servidor (puedes cambiarla según tu backend real)
                string rutaDocumento = $"uploads/{Guid.NewGuid()}_{_fileInfoBl.Name}";
                //string rutaDocumento = file.Name;
                var base64 = Convert.ToBase64String(stream.ToArray());
                //string rutaDocumento = $"data:application/pdf;base64,{base64}";
                // Aquí deberías guardar el archivo en backend o en wwwroot si aplica
                // await ServicioUpload.Guardar(stream, rutaDocumento);
                if (_indexTab == 0)
                {
                    foreach (var c in _seleccionadosMV)
                    {
                        var doc = c.Servicios.FirstOrDefault()?.BL;
                        foreach (var c1 in _contenedoresManiobras)
                        {
                            if (c.Contenedor.Equals(c1.Contenedor))
                            {
                                doc.Nombre = _fileInfoBl.Name;
                                doc.MimeType = _fileInfoBl.ContentType;
                                doc.Base64 = base64;
                            }
                        }
                        _contenedoresTienenDocumentos = !string.IsNullOrEmpty(doc.Base64);
                    }
                }
                if (_indexTab == 1)
                {

                    foreach (var c in _seleccionadosEIR)
                    {
                        var doc = c.Servicios.FirstOrDefault()?.EirDeLleno;
                        foreach (var c1 in _contenedoresEir)
                        {
                            if (c.Contenedor.Equals(c1.Contenedor))
                            {
                                doc.Nombre = _fileInfoBl.Name;
                                doc.MimeType = _fileInfoBl.ContentType;
                                doc.Base64 = base64;
                            }
                            _contenedoresTienenDocumentos = !string.IsNullOrEmpty(doc.Base64);
                        }
                    }
                }
                if (_indexTab == 0)
                {
                    await _gridContenedorCmpMV.ActualizarTabla();
                }
                else if (_indexTab == 1)
                {
                    await _gridContenedorCmpEIR.ActualizarTabla();
                }

                SincronizarEstadosServicios();

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Documento asociado",
                    Detail = "",
                    Duration = 4000
                });

                CerrarModalCarga();
            }
            catch (Exception ex)
            {
                NotificarErrores("Error al subir el documento", new List<string> { ex.Message });
            }
        }

        private void InicializarListaEncabezadosExcel()
        {
            _encabezadosExcel = new List<string> {
                "contenedor",
                "referencia del cliente",
                "tipo contenedor",
                "rfc cliente servicio",
                "rfc cliente facturar",
                "bl",
                "rfc naviera",
                "buque"
            };
        }

        private async Task GenerarSolicitud()
        {

            _referencia.Contenedores = new List<PeticionesContenedoresClienteExternoDTO>();
            _errores.Clear();

            //_estaCargando = true;
            MostrarModalCarga();

            bool hayRepetidos = _contenedoresManiobras
                                .Select(c => c.Contenedor)
                                .Intersect(_contenedoresEir.Select(c => c.Contenedor))
                                .Any();
            var contenedoresRepetidos = _contenedoresManiobras
                                        .Select(c => c.Contenedor)
                                        .Intersect(_contenedoresEir.Select(c => c.Contenedor))
                                        .ToList();

            //if (_contenedoresTienenDocumentos == false && _contenedoresManiobras.Count() > 0) {
            //    _errores.Add("Debe cargar el documento BL correspondiente a los contenedores");
            //}

            //if (_contenedoresTienenDocumentos == false && _contenedoresEir.Count() > 0) {
            //    _errores.Add("Debe cargar el documento EIR lleno correspondiente a los contenedores");
            //}
            if (contenedoresRepetidos.Any())
            {
                _errores.Add($"Los siguientes contenedores están duplicados entre MV y EIR: {string.Join(", ", contenedoresRepetidos)}. Por favor, elimina una de las lstSLOSolicitudes");
                MostrarNotificacionError("Ocurrió un problema al generar la solicitud", _errores);
                _errores.Clear();

                _estaCargando = false;
                CerrarModalCarga(); // Cierra el modal

                return;
            }

            if (_contenedoresManiobras.Count() > 0)
            {
                foreach (var cont in _contenedoresManiobras)
                {
                    _referencia.Contenedores.Add(cont);
                    var errores = ObtenerValidadorDeContenedor()?.ContenedorEsValido(cont, 0) ?? new List<string>();
                    foreach (var e in errores)
                    {
                        if (_errores.Contains(e))
                            continue;
                        _errores.Add(e);
                    }
                    if (!_esClienteExterno)
                    {
                        foreach (var servicio in cont.Servicios)
                        {
                            if (servicio.IdCatServicio == 1)
                            {
                                var naviera = _navieras.Where(n => n.RFC.Equals(servicio.NavieraRFC)).FirstOrDefault();
                                if (naviera != null)
                                {
                                    servicio.NavieraRazonSocial = naviera.RazonSocial;
                                }
                            }
                        }
                    }

                }
            }

            if (_contenedoresEir.Count() > 0)
            {
                foreach (var cont in _contenedoresEir)
                {
                    _referencia.Contenedores.Add(cont);
                    var errores = ObtenerValidadorDeContenedor()?.ContenedorEsValido(cont, 1) ?? new List<string>();
                    foreach (var e in errores)
                    {
                        if (_errores.Contains(e))
                            continue;
                        _errores.Add(e);
                    }
                }
            }

            if (_errores.Count() > 0)
            {
                MostrarNotificacionError("Ocurrió un problema al generar la solicitud", _errores);
                _errores.Clear();

                _estaCargando = false;
                CerrarModalCarga(); // Cierra el modal

                return;
            }

            await MostrarModalAsignarAduanaComentarioAsync(_referencia);

            if (_referencia.IdCatAduana == 0)
            {
                MostrarNotificacionError("Debes indicar una aduana", new List<string> { });

                _estaCargando = false;
                CerrarModalCarga(); // Cierra el modal

                return;
            }

            //ExisteNavieraHyundai();
            //await MostrarModalSoporteNaviera(_contenedoresManiobras);

            if (_esClienteExterno == false)
            {
                if (_referencia.IdCatClienteSolicitante == 0)
                {
                    MostrarNotificacionError("Debes indicar el cliente solicitante", new List<string> { });
                    _estaCargando = false;
                    CerrarModalCarga();
                    return;
                }
            }
            var respuesta = await OrdenService.CrearOrdenServicio(_referencia);

            //_estaCargando = false;
            CerrarModalCarga(); // Cierra el modal

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var orden = await OrdenService.GetOrden(respuesta.IdOrdenServicio);

                if (!_esClienteExterno)
                {
                    var respuestaIntegracion = await OrdenService.Integracion1G(orden.IdOrden);
                    if (respuestaIntegracion.ErrorMessages.Count() > 0)
                    {
                        MostrarNotificacionError("Ocurrió un problema al generar la solicitud de facturación", respuesta.ErrorMessages);
                        return;
                    }
                    var respuestaAnticipo = await OrdenService.GenerarAnticipo(orden.IdOrden);
                    if (respuestaAnticipo.ErrorMessages.Count() > 0)
                    {
                        MostrarNotificacionError("Ocurrió un problema al generar la solicitud de anticipos", respuesta.ErrorMessages);
                        return;
                    }
                }

                await Swal.FireAsync("Orden de servicio generada", $"Se generó la orden de servicio no. {respuesta.IdOrdenServicio} con la referencia operativa {orden.ReferenciaALO}.");
                NavigationManager.NavigateTo("/referenciasRZ");
            }
            else
            {
                if (respuesta.ErrorMessages.Count() > 0)
                {
                    MostrarNotificacionError("Ocurrió un problema al generar la solicitud", respuesta.ErrorMessages);
                }
                else
                {
                    MostrarNotificacionError("Ocurrió un problema al generar la solicitud", new List<string>());
                }

                if (respuesta.ErrorMessages.Count() == 0 && respuesta.StatusCode == HttpStatusCode.OK)
                {
                    await Swal.FireAsync("Orden de servicio generada", $"Se generó la orden de servicio no. {respuesta.IdOrdenServicio}.");
                    NavigationManager.NavigateTo("/referenciasRZ");
                }
            }
        }

        #region LecturaExcel
        private async Task OnUploadChangedAsync(UploadChangeEventArgs args, string tipoServicio)
        {

            try
            {
                _isLoading = true;
                var file = args.Files.FirstOrDefault();
                List<PeticionesContenedoresClienteExternoDTO> contenedores;

                if (file is null)
                {
                    NotificarErrores("Archivo no seleccionado", new List<string> { "Debes seleccionar un archivo válido." });

                    if (_indexTab == 0)
                    {
                        _contenedoresManiobras.Clear();
                        _seleccionadosMV.Clear();
                        await _uploadBl?.ClearFiles();

                        //_uploadBl = null;
                    }
                    else if (_indexTab == 1)
                    {
                        _contenedoresEir.Clear();
                        _seleccionadosEIR.Clear();
                        await _uploadEir?.ClearFiles();
                    }
                    //await _gridContenedorCmp.ActualizarTabla();
                    await ObtenerValidadorDeContenedor()?.ActualizarTabla();
                    SincronizarEstadosServicios();
                    _isLoading = false;
                    return;
                }

                using var stream = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(stream);
                await Task.Delay(1);
                switch (_indexTab)
                {
                    case 0:
                        contenedores = await LeerExcelAsync(stream);
                        _contenedoresManiobras = contenedores ??= new List<PeticionesContenedoresClienteExternoDTO>();
                        //await MostrarModalPatiosSiCorrespondeAsync(_contenedoresManiobras);

                        break;
                    case 1:
                        contenedores = await LeerExcelAsync(stream);
                        _contenedoresEir = contenedores ??= new List<PeticionesContenedoresClienteExternoDTO>();
                        //await MostrarModalPatiosSiCorrespondeAsync(_contenedoresManiobras);
                        //await MostrarModalSolicitudDatosHazesaAsync(_contenedoresEir);
                        break;
                }

                if (_contenedoresManiobras == null && _contenedoresEir == null) return;

                _isLoading = false;

                await InvokeAsync(StateHasChanged);

                //if (_indexTab == 0 && _existeNavieraHyndai) {
                //    await ObtenerValidadorDeContenedor()?.MostrarModalPatios();
                //} else if (_indexTab == 1 && _existePatioHazesa) {
                //    await ObtenerValidadorDeContenedor()?.MostrarModalPatios();
                //}


            }
            catch (Exception ex)
            {
                MostrarNotificacionError("Error inesperado", new List<string> { ex.Message });
            }
        }

        private async Task<List<PeticionesContenedoresClienteExternoDTO>> LeerExcelAsync(Stream stream)
        {
            try
            {
                var contenedores = new List<PeticionesContenedoresClienteExternoDTO>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(stream);
                _sheet = package.Workbook.Worksheets[0];

                var encabezadosExcel = new List<string>(); ;
                int col = 1;
                // Se obtienen los encabezados del documento excel
                while (!string.IsNullOrWhiteSpace(_sheet.Cells[1, col].Text))
                {
                    encabezadosExcel.Add(_sheet.Cells[1, col].Text);
                    col++;
                }
                // Se valida la estructura de los encabezados
                List<string> erroresCabeceras = ExcelService.ValidarEncabezadosExcel(_indexTab + 1, UsuarioTokenDTO, encabezadosExcel);

                if (erroresCabeceras.Count() > 0 && erroresCabeceras.Any())
                {
                    string mensajesError = string.Join("<br/>", erroresCabeceras);
                    IJsHelper.MostrarNotificacion(
                        NotificationService,
                        "Error en la estructura del documento excel",
                        mensajesError,
                        NotificationSeverity.Error,
                        8000
                    );
                    return null;
                }
                // 3) Creamos el diccionario [nombreCabecera → númeroDeColumna]
                var indices = ExcelService.MapearIndicesPorCabecera(encabezadosExcel);
                int row = 2;

                while (!string.IsNullOrEmpty(_sheet.Cells[row, 1].Text))
                {
                    var cont = new PeticionesContenedoresClienteExternoDTO
                    {
                        Contenedor = _sheet.Cells[row, indices[ExcelService.NUMERO_CONTENEDOR]].Text.Trim()
                    };
                    cont.Servicios = new List<PeticionesServiciosClienteExternoDTO>();
                    var servicio = new PeticionesServiciosClienteExternoDTO
                    {
                        IdCatServicio = _indexTab == 0 ? 1 : 2
                    };

                    //SE OBTIENE EL TIPO DEL CONTENEDOR PARA INGRESARLO AL OBJETO CONTENEDORES CLIENTE EXTERNO
                    string tipoContenedor = _sheet.Cells[row, indices[ExcelService.TIPO_CONTENEDOR]].Text.Trim().ToUpper();
                    cont.IdCatTipoContenedor = _tiposContenedor.Where(x => x.Nomenclatura.Equals(tipoContenedor)).FirstOrDefault().IdCatTipoContenedor;
                    cont.NombreEjecutivoSolicitante = _sheet.Cells[row, indices[ExcelService.NOMBRE_EJECUTIVO_SOLICITANTE]].Text;
                    cont.ClienteRFC = _sheet.Cells[row, indices[ExcelService.RFC_CLIENTE_SOLICITANTE]].Text;
                    servicio.RFCFacturar = _sheet.Cells[row, indices[ExcelService.RFC_CLIENTE_FACTURAR]].Text;

                    switch (_indexTab)
                    {
                        case 0:
                            servicio.NumeroBL = _sheet.Cells[row, indices[ExcelService.NUMERO_BL]].Text;
                            servicio.NombreBuque = _sheet.Cells[row, indices[ExcelService.NOMBRE_BUQUE]].Text;
                            servicio.NumeroViaje = _sheet.Cells[row, indices[ExcelService.NUMERO_BUQUE]].Text;
                            servicio.TransporteEmailContacto = _sheet.Cells[row, indices[ExcelService.TRANSPORTISTA_CORREO]].Text;
                            servicio.TransporteRFC = _sheet.Cells[row, indices[ExcelService.TRANSPORTISTA_RFC]].Text;
                            servicio.TransporteNombreContacto = _sheet.Cells[row, indices[ExcelService.TRANSPORTISTA_NOMBRE_CONTACTO]].Text;

                            if ((UsuarioTokenDTO.Rol == "ADMIN" || UsuarioTokenDTO.Rol == "ADMINUSER"))
                            {
                                servicio.TransporteRazonSocial = _sheet.Cells[row, indices[ExcelService.TRANSPORTISTA_RAZON_SOCIAL]].Text;
                                servicio.NavieraRazonSocial = _sheet.Cells[row, indices[ExcelService.NAVIERA_RAZON_SOCIAL]].Text;
                                cont.ReferenciaCliente = _sheet.Cells[row, indices[ExcelService.REFERENCIA_CLIENTE]].Text.Trim().ToUpper();
                                servicio.ReferenciaClienteFacturar = _sheet.Cells[row, indices[ExcelService.REFERENCIA_CLIENTE_FACTURAR]].Text.Trim().ToUpper();
                                servicio.IdCatPatio = int.Parse(_sheet.Cells[row, indices[ExcelService.ID_PATIO]].Text.Trim());
                                servicio.PatioRazonSocial = _sheet.Cells[row, indices[ExcelService.PATIO_RAZON_SOCIAL]].Text.Trim();
                                _referencia.Ticket = string.IsNullOrWhiteSpace(_sheet.Cells[row, indices[ExcelService.TICKET]].Text) ? null : int.Parse(_sheet.Cells[row, indices[ExcelService.TICKET]].Text.Trim());
                            }
                            else if (UsuarioTokenDTO.Rol.Equals("CLIENTE") && UsuarioTokenDTO.Email.Contains("nad", StringComparison.OrdinalIgnoreCase))
                            {
                                servicio.TransporteRazonSocial = _sheet.Cells[row, indices[ExcelService.TRANSPORTISTA_RAZON_SOCIAL]].Text;
                                servicio.NavieraRazonSocial = _sheet.Cells[row, indices[ExcelService.NAVIERA_RAZON_SOCIAL]].Text;
                                cont.ReferenciaCliente = _sheet.Cells[row, indices[ExcelService.REFERENCIA_CLIENTE]].Text.Trim().ToUpper();
                                servicio.ReferenciaClienteFacturar = _sheet.Cells[row, indices[ExcelService.REFERENCIA_CLIENTE_FACTURAR]].Text.Trim().ToUpper();
                                _referencia.Ticket = string.IsNullOrWhiteSpace(_sheet.Cells[row, indices[ExcelService.TICKET]].Text) ? null : int.Parse(_sheet.Cells[row, indices[ExcelService.TICKET]].Text.Trim());
                            }

                            //servicio.BL = new PeticionesDocumentosClienteExternoDTO() { IdTipoDocumento = 9, Nombre = "", MimeType = "", Base64 = "" };

                            break;
                        case 1:
                            servicio.PatioRFC = _sheet.Cells[row, indices[ExcelService.RFC_PATIO]].Text;
                            if (!string.IsNullOrWhiteSpace(_sheet.Cells[row, indices[ExcelService.RFC_PATIO]].Text))
                            {
                                var filtroPatio = new FiltroPatioDTO
                                {
                                    ProveedorInfo = _sheet.Cells[row, indices[ExcelService.RFC_PATIO]].Text
                                };

                                string filtroCifrado = await ControlService.GetFiltroCifrado(filtroPatio);
                                var lstCatPatio = await PatioService.ObtenerListaPatiosFiltrados(filtroCifrado) ?? new List<CatPatios>();

                                if (lstCatPatio.Any())
                                    servicio.IdCatPatio = lstCatPatio.FirstOrDefault().IdCatPatios;
                            }
                            servicio.FolioManiobra = _sheet.Cells[row, indices[ExcelService.FOLIO_MANIOBRA]].Text;
                            //servicio.EirDeLleno = new PeticionesDocumentosClienteExternoDTO() { IdTipoDocumento = 8, Nombre = "", MimeType = "", Base64 = "" };

                            if (!_esClienteExterno || UsuarioTokenDTO.Email.Contains("nad", StringComparison.OrdinalIgnoreCase))
                            {
                                cont.ReferenciaCliente = _sheet.Cells[row, indices[ExcelService.REFERENCIA_CLIENTE]].Text.Trim().ToUpper();
                                servicio.ReferenciaClienteFacturar = _sheet.Cells[row, indices[ExcelService.REFERENCIA_CLIENTE_FACTURAR]].Text.Trim().ToUpper();
                                _referencia.Ticket = string.IsNullOrWhiteSpace(_sheet.Cells[row, indices[ExcelService.TICKET]].Text) ? null : int.Parse(_sheet.Cells[row, indices[ExcelService.TICKET]].Text.Trim());
                            }
                            break;
                    }
                    cont.Servicios.Add(servicio);

                    _errores = ObtenerValidadorDeContenedor()?.ContenedorEsValido(cont, _indexTab) ?? new List<string>();
                    contenedores.Add(cont);
                    row++;
                }

                if (_errores.Count() > 0) MostrarNotificacionError("Error al procesar plantilla", _errores);

                return contenedores;

            }
            catch (Exception ex)
            {
                MostrarNotificacionError("Error inesperado al procesar el archivo", new List<string> { ex.Message });
                return null;
            }
        }

        #endregion LecturaExcel

        #region EventCallBack
        private void ActualizarListaContenedores(string tipo, List<PeticionesContenedoresClienteExternoDTO> contenedores)
        {
            if (tipo == "MV") _contenedoresManiobras = contenedores;
            else if (tipo == "EIR") _contenedoresEir = contenedores;
        }
        private GridPeticionContenedorCmp ObtenerValidadorPorTipo(string tipo)
        {
            return tipo == "MV" ? _gridContenedorCmpMV : _gridContenedorCmpEIR;
        }
        private void ActualizarContenedoresMV(List<PeticionesContenedoresClienteExternoDTO> actualizados)
        {
            _contenedoresManiobras = actualizados;
        }

        // Con este método se mantienen los check activados o desactivados correspondientes a las columnas de GG y GD
        // Cuando el usuario edita un registro
        private void GetContenedorActualizado(PeticionesContenedoresClienteExternoDTO contenedor)
        {

            if (contenedor.Servicios.Any(s => s.IdCatServicio == 3))
            {
                OnServicioCheckChanged(contenedor, 3, true);
            }

            if (contenedor.Servicios.Any(s => s.IdCatServicio == 4))
            {
                OnServicioCheckChanged(contenedor, 4, true);
            }
        }

        private void ActualizarContenedoresEIR(List<PeticionesContenedoresClienteExternoDTO> actualizados)
        {
            _contenedoresEir = actualizados;
        }
        #endregion EventCallBack

        private GridPeticionContenedorCmp ObtenerValidadorDeContenedor()
        {
            if (_indexTab == 0)
                return _gridContenedorCmpMV;
            else if (_indexTab == 1)
                return _gridContenedorCmpEIR;
            else
                return null;
        }

        private void MostrarNotificacionError(string titulo, List<string> mensajes)
        {
            ObtenerValidadorDeContenedor()?.ShowNotification(titulo, mensajes, NotificationSeverity.Error);
        }
        private void NotificarErrores(string titulo, List<string> errores)
        {
            string html = "<ul>";
            foreach (var e in errores)
            {
                html += $"<li>{e}</li>";
            }
            html += "</ul>";

            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = titulo,
                Detail = html,
                Duration = 50000,
                CloseOnClick = true,
                Payload = DateTime.Now
            });
        }

        private void OnChange(int index)
        {
            _indexTab = index;
        }

        private bool PeticionContenedorIsValid(PeticionesContenedoresClienteExternoDTO contenedor)
        {



            return true;
        }

        private void CargarBl()
        {

        }

        private void CargarEir()
        {

        }

        #region Asignación Patio
        private bool DebeMostrarAsignarPatio(List<PeticionesContenedoresClienteExternoDTO> contenedores)
        {
            // 1. Al menos uno con NavieraRazonSocial que contenga "HYUNDAI"
            bool existeHyundai = contenedores.Any(c =>
                c.Servicios.Any(s => s.NavieraRFC != null &&
                                     s.NavieraRFC.Equals("HYN140815591", StringComparison.OrdinalIgnoreCase)));
            ObtenerValidadorDeContenedor()?.ShowNotification(
                                                "Has indicado la naviera Hyundai",
                                                new List<string> { "Debes indicar los datos patio para continuar" },
                                                NotificationSeverity.Warning
                                                );
            // 2. Al menos uno con PatioRFC que contenga "TSH081231SK5"
            bool existePatioHazesa = contenedores.Any(c =>
                c.Servicios.Any(s => s.PatioRFC != null &&
                                     s.PatioRFC.Contains("TSH081231SK5", StringComparison.OrdinalIgnoreCase)));

            // Decide si mostrar el modal cuando se cumplan las reglas:
            // pueden ser independientes (||), o que ambas sean verdaderas (&&).
            // Ajusta la lógica según tu necesidad real.
            return existeHyundai || existePatioHazesa;
        }

        private bool DebeMostrarModalSolicitudDatosHazesa(List<PeticionesContenedoresClienteExternoDTO> contenedores)
        {
            var existePatioHazesa = contenedores.Any(c => c.Servicios.Any(s => s.PatioRFC != null && s.PatioRFC.Contains(HAZESA_RFC, StringComparison.OrdinalIgnoreCase)));
            return existePatioHazesa;
        }

        private async Task MostrarModalSolicitudDatosHazesaAsync(List<PeticionesContenedoresClienteExternoDTO> contenedores)
        {

            if (!DebeMostrarModalSolicitudDatosHazesa(contenedores))
            {
                return;
            }

            var resultado = await DialogService.OpenAsync<SolicitarDatosHazesaCmp>(
                title: "Datos complementario patio Hazesa",
                options: new DialogOptions
                {
                    Width = "800px",
                    Height = "480px",
                    CloseDialogOnOverlayClick = false,
                    ShowClose = true // Oculta la equis del modal
                }
            );
        }

        private async Task MostrarModalPatiosSiCorrespondeAsync(List<PeticionesContenedoresClienteExternoDTO> contenedores)
        {
            // 1. Verificar condiciones
            if (!DebeMostrarAsignarPatio(contenedores))
                return; // No cumple, no hacemos nada

            // 2. Preparar parámetros para el modal
            var parametros = new Dictionary<string, object>()
            {
                ["Contenedores"] = contenedores
            };

            // 3. Abrir el componente modal 'AsignarPatioCmp'
            //    Recibir la lista actualizada que se retorne por DialogService.Close( ... )
            var resultado = await DialogService.OpenAsync<AsignarPatioCmp>(
                title: "Asignar patio",
                parameters: parametros,
                options: new DialogOptions
                {
                    Width = "800px",
                    Height = "600px",
                    CloseDialogOnOverlayClick = false
                }
            );

            // 4. Procesar el resultado
            //    El componente modal va a retornar la lista actualizada de contenedores
            if (resultado is List<PeticionesContenedoresClienteExternoDTO> contenedoresActualizados)
            {
                // Actualizas la lista original con la que retornó del modal
                // Esto dependerá de si la reemplazas totalmente o si la sincronizas
                contenedores.Clear();
                contenedores.AddRange(contenedoresActualizados);

                // Podrías forzar un renderizado o refrescar tu gridSolicitudesDetalle, por ejemplo:
                // await _gridContenedorCmpMV.ActualizarTabla();
                // o el que corresponda a tu caso
            }
        }

        private void AsignarPatioSeleccionado(PeticionesContenedoresClienteExternoDTO contenedor)
        {
            if (contenedor.Servicios.Any(s => s.IdCatServicio == 1))
            {
                contenedor.Servicios.Where(s => s.IdCatServicio == 1).FirstOrDefault().IdCatPatio = _patioSeleccionado.IdCatPatios;
            }
        }

        #endregion Asignación Patio

        private async Task MostrarModalAsignarAduanaComentarioAsync(PeticionesReferenciasClienteExternoDTO referencia)
        {

            //if (_referencia.IdCatAduana > 0) {
            //    return;
            //}

            // 2. Preparar parámetros para el modal
            var parametros = new Dictionary<string, object>()
            {
                ["PeticionReferencia"] = referencia
            };

            // 3. Abrir el componente modal 'AsignarPatioCmp'
            //    Recibir la lista actualizada que se retorne por DialogService.Close( ... )
            var resultado = await DialogService.OpenAsync<AsignarAduanaComentarioCmp>(
                title: "Asignar aduana a solicitud",
                parameters: parametros,
                options: new DialogOptions
                {
                    Width = "800px",
                    Height = "450px",
                    CloseDialogOnOverlayClick = false
                }
            );
        }

        private void ExisteNavieraHyundai()
        {
            _existeNavieraHyndai = _contenedoresManiobras.Any(c => (c.Servicios.FirstOrDefault()?.NavieraRFC.Equals(AsignarSoporteNavieraCmp.RFC_NAVIERA_HYUNDAI) == true));
        }
        private async Task MostrarModalSoporteNaviera(List<PeticionesContenedoresClienteExternoDTO> contenedores)
        {

            ExisteNavieraHyundai();

            if (_existeNavieraHyndai == false)
            {
                return;
            }

            bool mostrarModal = contenedores.Any(c => c.Servicios.Any(s =>
                                                    s.NavieraRFC.Equals(AsignarSoporteNavieraCmp.RFC_NAVIERA_HYUNDAI) &&
                                                    (s.SoporteNaviera == null ||
                                                    string.IsNullOrEmpty(s.SoporteNaviera.MimeType) &&
                                                    string.IsNullOrEmpty(s.SoporteNaviera.Nombre) &&
                                                    string.IsNullOrEmpty(s.SoporteNaviera.Base64)
                                                    )));

            if (mostrarModal == false)
                return;
            // 2. Preparar parámetros para el modal
            var parametros = new Dictionary<string, object>()
            {
                ["Contenedores"] = contenedores
            };
            var resultado = await DialogService.OpenAsync<AsignarSoporteNavieraCmp>(
                title: "Asignación de soporte de naviera",
                parameters: parametros,
                options: new DialogOptions
                {
                    Width = "1000px",
                    Height = "600px",
                    CloseDialogOnOverlayClick = false,
                    ShowClose = false
                }
            );

            // 4. Procesar el resultado
            //    El componente modal va a retornar la lista actualizada de contenedores
            if (resultado is List<PeticionesContenedoresClienteExternoDTO> contenedoresActualizados)
            {
                // Actualizas la lista original con la que retornó del modal
                // Esto dependerá de si la reemplazas totalmente o si la sincronizas
                //contenedores.Clear();
                //contenedores.AddRange(contenedoresActualizados);
                foreach (var contOriginal in contenedores)
                {
                    var servicioOriginal = contOriginal.Servicios.FirstOrDefault();
                    foreach (var contCopia in contenedoresActualizados)
                    {
                        var servicioCopia = contCopia.Servicios.FirstOrDefault();
                        if (contOriginal.Contenedor.Equals(contCopia.Contenedor))
                        {
                            // Asinar los datos del soporte a la lista original
                            servicioOriginal.SoporteNaviera = servicioCopia.SoporteNaviera;
                            //servicioOriginal.SoporteNaviera.IdTipoDocumento = servicioCopia.SoporteNaviera.IdTipoDocumento;
                            //servicioOriginal.SoporteNaviera.Nombre = servicioCopia.SoporteNaviera.Nombre;
                            //servicioOriginal.SoporteNaviera.MimeType = servicioCopia.SoporteNaviera.MimeType;
                            //servicioOriginal.SoporteNaviera.Base64 = servicioCopia.SoporteNaviera.Base64;
                        }
                    }
                }
            }
        }

        private async Task AbrirPdfEnNuevaPestana(InputFileChangeEventArgs e)
        {
            var file = e.File;

            using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10MB
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();

            var base64 = Convert.ToBase64String(bytes);

            await JS.InvokeVoidAsync("abrirPdfEnNuevaPestana", base64);
        }

        private async Task AbrirDocumentoEnNuevaPestana(string? base64)
        {
            if (!string.IsNullOrWhiteSpace(base64))
            {
                await JS.InvokeVoidAsync("abrirPdfBase64", base64);
            }
        }

        private void OnServicioCheckChanged(PeticionesContenedoresClienteExternoDTO contenedor, int idCatServicio, bool isChecked)
        {

            if (contenedor == null) return;

            var servicios = contenedor.Servicios;
            string referenciaClienteFacturar = "";

            if (servicios.Any(s => s.IdCatServicio == 1))
            {
                var ser = servicios.Where(s => s.IdCatServicio == 1).FirstOrDefault();
                referenciaClienteFacturar = ser.ReferenciaClienteFacturar;
            }

            if (isChecked)
            {
                if (!servicios.Any(s => s.IdCatServicio == idCatServicio))
                {
                    var servicio = CrearObjectoServicio();
                    servicio.IdCatServicio = idCatServicio;
                    servicio.RFCFacturar = contenedor.Servicios.FirstOrDefault().RFCFacturar;
                    servicio.RazonSocialFacturar = contenedor.Servicios.FirstOrDefault().RazonSocialFacturar;
                    servicio.PatioRFC = contenedor.Servicios.FirstOrDefault().PatioRFC;
                    servicio.PatioRazonSocial = contenedor.Servicios.FirstOrDefault().PatioRazonSocial;
                    servicio.TransporteRFC = "TMO160804RF5";
                    servicio.PatioRFC = "AVE9909061M4";
                    servicio.TransporteEmailContacto = "transportista@gmail.com";
                    servicio.NombreInstitucionBancaria = idCatServicio == 3 ? "BBVA BANCOMER" : "";
                    servicio.ConsignatarioRFC = idCatServicio == 3 ? "AVE9909061M4" : "";
                    servicio.ConsignatarioRazonSocial = idCatServicio == 3 ? "Consignatario razón social" : "";
                    servicio.ReferenciaClienteFacturar = referenciaClienteFacturar;
                    servicios.Add(servicio);
                }
            }
            else
            {
                var servicioExistente = servicios.FirstOrDefault(s => s.IdCatServicio == idCatServicio);
                if (servicioExistente != null)
                    servicios.Remove(servicioExistente);
            }

            if (idCatServicio == 3)
                _servicioGG[contenedor.Contenedor] = isChecked;
            else if (idCatServicio == 4)
                _servicioGD[contenedor.Contenedor] = isChecked;
        }

        private void SincronizarEstadosServicios()
        {
            _servicioGG.Clear();
            _servicioGD.Clear();

            foreach (var cont in _contenedoresManiobras)
            {
                _servicioGG[cont.Contenedor] = cont.Servicios.Any(s => s.IdCatServicio == 3);
                _servicioGD[cont.Contenedor] = cont.Servicios.Any(s => s.IdCatServicio == 4);
            }

            foreach (var cont in _contenedoresEir)
            {
                _servicioGG[cont.Contenedor] = cont.Servicios.Any(s => s.IdCatServicio == 3);
                _servicioGD[cont.Contenedor] = cont.Servicios.Any(s => s.IdCatServicio == 4);
            }
        }

        private PeticionesServiciosClienteExternoDTO CrearObjectoServicio()
        {

            return new PeticionesServiciosClienteExternoDTO
            {
                IdCatServicio = 0,
                TransporteRFC = "",
                TransporteRazonSocial = "",
                TransporteNombreContacto = "",
                TransporteEmailContacto = "",
                NumeroBL = "",
                NavieraRFC = "",
                NavieraRazonSocial = "",
                NombreBuque = "",
                NumeroViaje = "",
                //BL = new PeticionesDocumentosClienteExternoDTO {
                //    IdTipoDocumento = 0,
                //    Nombre = "",
                //    MimeType = "",
                //    Base64 = ""
                //},
                //SoporteNaviera = new PeticionesDocumentosClienteExternoDTO {
                //    IdTipoDocumento = 0,
                //    Nombre = "",
                //    MimeType = "",
                //    Base64 = ""
                //},
                NombreConductor = "",
                NumLicenciaConductor = "",
                NumUnidad = "",
                PlacaUnidad = "",
                //EirDeLleno = new PeticionesDocumentosClienteExternoDTO {
                //    IdTipoDocumento = 0,
                //    Nombre = "",
                //    MimeType = "",
                //    Base64 = ""
                //},
                IdCatPatio = 0,
                PatioRFC = "",
                PatioRazonSocial = "",
                FolioManiobra = "",
                ConsignatarioRFC = "",
                ConsignatarioRazonSocial = "",
                NombreInstitucionBancaria = "",
                NumeroCuentaEgresoPago = 0,
                MontoSoliciud = 0,
                MontoTotal = 0,
                FechaPago = null,
                FechaTocaPiso = null,
                RFCFacturar = "",
                RazonSocialFacturar = ""
            };

        }

        void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Hello!", options);

        private bool EsClienteNad()
        {
            return UsuarioTokenDTO.Rol.Equals("CLIENTE") && UsuarioTokenDTO.Email.Contains("nad", StringComparison.OrdinalIgnoreCase);
        }

        private bool EsUsuarioInterno()
        {
            return (UsuarioTokenDTO.Rol == "ADMIN" || UsuarioTokenDTO.Rol == "ADMINUSER");
        }

        private async Task SetClienteSolicitanteAsync(object value)
        {

            int idCatCliente = value != null ? int.Parse(value.ToString()) : 0;

            var cliente = _clientesSolicitantes.Where(c => c.IdCatCliente == idCatCliente).FirstOrDefault();

            if (cliente != null) _idCatClienteSolicitanteSeleccionado = cliente.IdCatCliente;

            _isLoading = true;
            await Task.Delay(100);
            _isLoading = false;

        }

    }// Llave de la clase

}
