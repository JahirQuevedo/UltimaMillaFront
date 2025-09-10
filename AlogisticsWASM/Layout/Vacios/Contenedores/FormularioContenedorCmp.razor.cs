using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Utilerias;
using ALOG.Modelos.Modelos.DTO.Vacios;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Control.IControl;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Text.RegularExpressions;

namespace AlogisticsWASM.Layout.Vacios.Contenedores
{
    public partial class FormularioContenedorCmp
    {

        [Parameter] public CatLineaNegocioTariPrecio Servicio { get; set; }
        [Parameter] public PeticionesContenedores Contenedor { get; set; }
        [Parameter] public int IdReferencia { get; set; }
        [Parameter] public int IdContenedor { get; set; }
        //[Parameter] public string Contenedor { get; set; }
        [Parameter] public bool AgregarServicio { get; set; }
        [Parameter] public Ordenes Orden { get; set; }
        [Parameter] public List<int> IdsServiciosContenedor { get; set; }

        [Inject] public ICatTipoContenedorService CatTipoContenedorService { get; set; }
        [Inject] public ICatTransportistaService TransportistaService { get; set; }
        [Inject] public NotificationService NotificationService { get; set; }
        [Inject] public ICatClientesService ClientesService { get; set; }
        [Inject] public ICatNavieraService NavieraService { get; set; }
        [Inject] public ICatPatiosServices PatioService { get; set; }
        [Inject] public IControlService ControlService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public IOrdenService OrdenService { get; set; }
        [Inject] public SweetAlertService Swal { get; set; }


        private ICollection<CatTipoContenedor> _tiposContenedor;
        private ICollection<CatTransportistas> _transportistas;
        private ICollection<CatClientes> _clientesFacturar;
        private ICollection<CatNavieras> _navieras;
        private ICollection<CatClientes> _clientes;
        private ICollection<CatPatios> _patios;
        private List<string> _errores;

        private PeticionesContenedoresClienteExternoDTO _contenedor;
        private PeticionesServiciosClienteExternoDTO _servicio;
        private CatTransportistas _transportistaSeleccionado;
        private CatClientes _clienteServicioSeleccionado;
        private CatClientes _clienteFacturarSeleccionado;
        private CatTipoContenedor _tipoContenedorSeleccionado;
        private CatNavieras _navieraSeleccionada;
        private CatPatios _patioSeleccionado;


        private RadzenUpload _uploadExpediente;
        private Radzen.FileInfo _fileInfo;
        private string _base64;
        private bool _busy;
        private FiltroPatioDTO _filtro;

        protected override async Task OnInitializedAsync()
        {
            _clienteServicioSeleccionado = new CatClientes();
            _clienteFacturarSeleccionado = new CatClientes();
            _tiposContenedor = new List<CatTipoContenedor>();
            _patioSeleccionado = new CatPatios();
            _navieraSeleccionada = new CatNavieras();
            _transportistaSeleccionado = new CatTransportistas();
            _contenedor = new PeticionesContenedoresClienteExternoDTO();
            _contenedor.Servicios = new List<PeticionesServiciosClienteExternoDTO>();
            _servicio = new PeticionesServiciosClienteExternoDTO();
            _tipoContenedorSeleccionado = new CatTipoContenedor();
            _errores = new List<string>();
            _filtro = new FiltroPatioDTO();


            _transportistas = await TransportistaService.GetTransportistas();
            _clientesFacturar = await ClientesService.GetClientes();
            _clientes = await ClientesService.GetClientes();
            _navieras = await NavieraService.GetNavieras();
            _patios = new List<CatPatios>();
            _tiposContenedor = await CatTipoContenedorService.GetTiposContenedorAsync();
            //_patios = await PatioService.GetPatios();
        }

        protected override async Task OnParametersSetAsync()
        {

            if (_tiposContenedor == null || !_tiposContenedor.Any())
            {
                _tiposContenedor = await CatTipoContenedorService.GetTiposContenedorAsync();
            }

            if (AgregarServicio && Orden != null)
            {
                _contenedor.Contenedor = Contenedor.Contenedor;
                _tipoContenedorSeleccionado = _tiposContenedor.FirstOrDefault(t => t.IdCatTipoContenedor == Contenedor.IdCatTipoContenedor) ?? new CatTipoContenedor();
                _contenedor.ReferenciaCliente = Contenedor.RefenciaCliente;
                _contenedor.IdCatTipoContenedor = _tipoContenedorSeleccionado.IdCatTipoContenedor;
                _filtro.IdCatAduana = Orden.catAduana?.IdCatAduana;
                _patioSeleccionado = Contenedor.catPatios != null ? Contenedor.catPatios : new CatPatios();
                string filtroCifrado = await ControlService.GetFiltroCifrado(_filtro);
                _patios = await PatioService.ObtenerListaPatiosFiltrados(filtroCifrado);
            }

            if (Orden != null)
            {
                _filtro.IdCatAduana = Orden.catAduana?.IdCatAduana;
                string filtroCifrado = await ControlService.GetFiltroCifrado(_filtro);
                _patios = await PatioService.ObtenerListaPatiosFiltrados(filtroCifrado);
            }
        }

        private async Task GuardarAsync()
        {
            _busy = true;
            SetContenedor();

            if (ValidarDatos() == false)
            {
                _busy = false;
                return;
            }
            // Si se está agregando un contenedor
            if (AgregarServicio == false)
            {
                PeticionesRespuestaDTO respuesta = await OrdenService.AgregarContenedorAReferencia(IdReferencia, _contenedor);
                _busy = false;
                if (respuesta != null)
                {
                    if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await Swal.FireAsync(
                            $"Contenedor agregado correctamente",
                            $"El contenedor {_contenedor.Contenedor} se agregó correctamente a la orden"
                        );
                        DialogService.Close(true);
                    }

                    if (respuesta.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        string mensaje = string.Join("<br/>", respuesta.ErrorMessages);
                        IJsHelper.MostrarNotificacion(
                            NotificationService,
                            "Error al guardar el contenedor",
                            mensaje,
                            NotificationSeverity.Error,
                            5000
                        );
                    }
                }

            }
            // Si se está agregando un servicio al contenedor
            if (AgregarServicio == true)
            {
                _servicio.IdCatServicio = Servicio.catLineaNegocioTarifa.catServicios.IdCatServicio;
                PeticionesRespuestaDTO respuesta = await OrdenService.AgregarServicioAContenedor(IdReferencia, IdContenedor, _servicio);
                _busy = false;
                if (respuesta != null)
                {
                    if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        MostrarNotificacion(
                            "Servicio agregado con éxito",
                            "",
                            NotificationSeverity.Success,
                            4000
                        );
                        DialogService.Close(true);
                    }
                }
            }
        }

        public void Cancelar()
        {
            DialogService.Close();
        }
        private void MostrarNotificacion(string titulo, string mensaje, NotificationSeverity notificationSeverity, int duration)
        {

            NotificationService.Notify(new NotificationMessage
            {
                Severity = notificationSeverity,
                Summary = titulo,
                Detail = mensaje,
                Duration = duration == 0 ? 4000 : duration
            });

        }

        private void SetContenedor()
        {

            _contenedor.Servicios.Clear();
            _contenedor.ClienteRFC = _clienteServicioSeleccionado.RFC;
            _contenedor.ClienteRazonSocial = _clienteServicioSeleccionado.RazonSocial;
            _contenedor.IdCatTipoContenedor = _tipoContenedorSeleccionado.IdCatTipoContenedor;

            _servicio.RFCFacturar = _clienteFacturarSeleccionado.RFC;
            _servicio.RazonSocialFacturar = _clienteFacturarSeleccionado.RazonSocial;

            switch (Servicio.catLineaNegocioTarifa.catServicios.IdCatServicio)
            {
                case 1:
                    _servicio.IdCatServicio = 1;

                    _servicio.IdCatPatio = _patioSeleccionado.IdCatPatios;
                    //_servicio.PatioRFC = "CMA100106AH8";

                    _servicio.TransporteRFC = _transportistaSeleccionado.RFC;
                    _servicio.TransporteNombreContacto = "TMO160804RF5";
                    _servicio.TransporteRazonSocial = _transportistaSeleccionado.RazonSocial;
                    if (string.IsNullOrWhiteSpace(_servicio.TransporteEmailContacto))
                        _servicio.TransporteEmailContacto = _transportistaSeleccionado.Correo;

                    _servicio.NavieraRFC = _navieraSeleccionada.RFC;
                    _servicio.NavieraRazonSocial = _navieraSeleccionada.RazonSocial;
                    //_servicio.BL = CrearObjectoPeticionDocumento();
                    //_servicio.BL.IdTipoDocumento = 9;
                    break;
                case 2:
                    _servicio.IdCatServicio = 2;
                    _servicio.IdCatPatio = _patioSeleccionado.IdCatPatios;
                    _servicio.PatioRFC = "CMA100106AH8";
                    //_servicio.EirDeLleno = CrearObjectoPeticionDocumento();
                    _servicio.PatioRazonSocial = _patioSeleccionado.RazonSocial;
                    break;
                default:
                    _servicio.IdCatServicio = Servicio.catLineaNegocioTarifa.catServicios.IdCatServicio;
                    break;

            }

            _contenedor.Servicios.Add(_servicio);
        }

        #region Método para la carga del documento
        private PeticionesDocumentosClienteExternoDTO CrearObjectoPeticionDocumento()
        {

            if (_fileInfo == null)
            {
                return new PeticionesDocumentosClienteExternoDTO();
            }

            PeticionesDocumentosClienteExternoDTO documento = new PeticionesDocumentosClienteExternoDTO();

            //documento = new PeticionesDocumentosClienteExternoDTO();

            documento.Nombre = _fileInfo.Name;
            documento.MimeType = _fileInfo.ContentType;
            documento.Base64 = _base64;

            return documento;
        }

        private async Task OnDocumentoCargado(UploadChangeEventArgs args)
        {
            try
            {
                _fileInfo = args.Files.FirstOrDefault();

                if (_fileInfo == null)
                    return;

                using var stream = new MemoryStream();
                await _fileInfo.OpenReadStream().CopyToAsync(stream);
                _base64 = Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Método para la carga del documento

        #region Métodos para realizar el binding con los autocomplete
        private void OnClienteServicioSeleccionado(object value)
        {
            string razonSocial = value.ToString() ?? "";
            _clienteServicioSeleccionado = _clientes.FirstOrDefault(c => c.RazonSocial.Equals(razonSocial)) ?? new CatClientes();
        }

        private void OnTipoContenedorSeleccionado(object value)
        {
            string nomenclatura = value.ToString() ?? "";
            _tipoContenedorSeleccionado = _tiposContenedor.FirstOrDefault(tipo => tipo.Nomenclatura.Equals(nomenclatura)) ?? new CatTipoContenedor();
        }
        private void OnClienteFacturarSeleccionado(object value)
        {
            string razonSocial = value.ToString() ?? "";
            _clienteFacturarSeleccionado = _clientesFacturar.FirstOrDefault(c => c.RazonSocial.Equals(razonSocial)) ?? new CatClientes();
        }

        private void OnNavieraSeleccionada(object value)
        {
            string razonSocial = value.ToString() ?? "";
            _navieraSeleccionada = _navieras.FirstOrDefault(n => n.RazonSocial.Equals(razonSocial)) ?? new CatNavieras();
        }

        private void OnTransportistaSeleccionado(object value)
        {
            string razonSocial = value.ToString() ?? "";
            _transportistaSeleccionado = _transportistas.FirstOrDefault(t => t.RazonSocial.Equals(razonSocial)) ?? new CatTransportistas();
        }
        private void OnPatioSeleccionado(object value)
        {
            string razonSocial = value.ToString() ?? "";
            _patioSeleccionado = _patios.FirstOrDefault(t => t.RazonSocial.Equals(razonSocial)) ?? new CatPatios();
        }
        #endregion Métodos para realizar el binding con los autocomplete

        #region Métodos de validaciones
        private bool ValidarDatos()
        {

            _errores.Clear();
            var regex = new Regex(@"^[A-Z]{4}\d{7}$");

            if (string.IsNullOrEmpty(_contenedor.Contenedor))
            {
                _errores.Add("El número de contenedor no puede estar vacío");
            }
            else
            {
                if (!regex.IsMatch(_contenedor.Contenedor))
                {
                    _errores.Add($"El contenedor {_contenedor.Contenedor}  no es un contenedor valido.");
                }
            }

            if (_tipoContenedorSeleccionado.IdCatTipoContenedor == 0)
                _errores.Add($"Debes indicar el tipo de contenedor.");

            //if (string.IsNullOrEmpty(_contenedor.ClaveTipoContenedor)) {
            //    _errores.Add("El tipo de contenedor es requerido");
            //} else {
            //    if (!TipoContenedorEsValido(_contenedor.ClaveTipoContenedor)) {
            //        _errores.Add($"El tipo de contenedor {_contenedor.ClaveTipoContenedor} no tiene un formato válido");
            //    }
            //}

            if (string.IsNullOrEmpty(_contenedor.ClienteRFC))
            {
                _errores.Add($"Debes indicar un RFC de cliente servico o RFC de cliente a facturar para el contenedor {_contenedor.Contenedor}");
            }

            if (string.IsNullOrEmpty(_servicio.RFCFacturar))
            {
                _errores.Add($"Debes indicar un RFC de cliente a facturar para el contenedor {_contenedor.Contenedor}");
            }
            // Aquí se valida el RFC del cliente solicitante
            if (!string.IsNullOrEmpty(_contenedor.ClienteRFC))
            {
                if (!RfcValido(_contenedor.ClienteRFC))
                {
                    _errores.Add("El RFC del cliente servicio no es válido");
                }
                else
                {
                    if (string.IsNullOrEmpty(_servicio.RFCFacturar))
                    {
                        _servicio.RFCFacturar = _contenedor.ClienteRFC;
                    }
                }
            }

            if (!string.IsNullOrEmpty(_servicio.RFCFacturar))
            {
                if (!RfcValido(_servicio.RFCFacturar))
                {
                    _errores.Add("El RFC del cliente a facturar no es válido");
                }
                else
                {
                    if (string.IsNullOrEmpty(_contenedor.ClienteRFC))
                    {
                        _contenedor.ClienteRFC = _servicio.RFCFacturar;
                    }
                }
            }

            #region Validaciones servicio de maniobras de vacíos
            if (_servicio.IdCatServicio == 1)
            {

                if (string.IsNullOrEmpty(_servicio.BL.Nombre) || string.IsNullOrEmpty(_servicio.BL.MimeType) || string.IsNullOrEmpty(_servicio.BL.Base64))
                {
                    _errores.Add("Debes cargar el documento Bl");
                }

                if (string.IsNullOrEmpty(_servicio.NumeroBL))
                {
                    _errores.Add("El número de BL es requerido");
                }

                //if (string.IsNullOrEmpty(_servicio.NavieraRFC)) {
                //    _errores.Add("El RFC de la naviera es requerido");
                //} else {
                //    if (!RfcValido(_servicio.NavieraRFC)) {
                //        //_errores.Add("El RFC de la naviera no es válido");
                //    }
                //}

                if (string.IsNullOrEmpty(_servicio.NumeroViaje))
                {
                    _errores.Add("El número del viaje es requerido");
                }

                if (string.IsNullOrEmpty(_servicio.NombreBuque))
                {
                    _errores.Add("El nombre del buque es requerido");
                }

                if (string.IsNullOrEmpty(_servicio.TransporteRFC))
                {
                    _errores.Add("El RFC del transportista es requerido");
                }
                else
                {
                    if (!RfcValido(_servicio.TransporteRFC))
                    {
                        _errores.Add("El RFC del transportista no es válido");
                    }
                }

                //if (string.IsNullOrEmpty(_servicio.TransporteEmailContacto)) {
                //    _errores.Add("El email del transportista es requerido");
                //}

                if (string.IsNullOrEmpty(_servicio.TransporteRazonSocial))
                {
                    _errores.Add("La razón social del transportista es requerida");
                }

            }
            #endregion Validaciones servicio de maniobras de vacíos

            #region Validaciones servicio de eir

            if (_servicio.IdCatServicio == 2)
            {

                if (string.IsNullOrEmpty(_servicio.EirDeLleno.Nombre) || string.IsNullOrEmpty(_servicio.EirDeLleno.MimeType) || string.IsNullOrEmpty(_servicio.EirDeLleno.Base64))
                {
                    _errores.Add("Debes cargar el documento E.I.R. de lleno");
                }

                if (string.IsNullOrEmpty(_servicio.PatioRFC))
                {
                    _errores.Add("El RFC del patio es requerido");
                }
                else
                {
                    if (!RfcValido(_servicio.PatioRFC))
                    {
                        _errores.Add("El RFC del patio no es válido");
                    }
                }
            }
            #endregion Validaciones servicio de eir

            if (_errores.Count() > 0)
            {

                string mensaje = string.Join("<br/>", _errores);

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Ocurrieron los siguientes errores al agregar el contenedor a la orden",
                    Detail = mensaje,
                    Duration = 8000
                });
                return false;
            }

            return true;

        }

        private bool TipoContenedorEsValido(string tipoContenedor)
        {

            if (string.IsNullOrWhiteSpace(tipoContenedor)) return false;

            tipoContenedor = tipoContenedor.ToUpper().Trim();

            // Verifica que coincida con el patrón general
            if (!Regex.IsMatch(tipoContenedor, @"^[A-Z]{2}\d{2}$"))
                return false;

            // Lista de prefijos no permitidos
            var prefijosInvalidos = new HashSet<string> { "XX", "ZZ", "AA" };

            string prefijo = tipoContenedor.Substring(0, 2);
            if (prefijosInvalidos.Contains(prefijo))
                return false;

            return true;
        }

        private bool RfcValido(string rfc)
        {

            var resultado = ValidateRfc.RfcIsValid(rfc);
            return resultado.EsValido;
        }
        #endregion Métodos de validaciones
    }
}
