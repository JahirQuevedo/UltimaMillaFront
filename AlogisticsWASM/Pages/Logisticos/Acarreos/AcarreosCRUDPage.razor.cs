using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using AlogisticsWASM.Layout.Utilerias;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace AlogisticsWASM.Pages.Logisticos.Acarreos
{
    public partial class AcarreosCRUDPage : ComponentBase
    {
        #region Variables
        [Parameter] public int paramIdEncabezado { get; set; } = 0;
        [Parameter] public string paramStrTipoAccion { get; set; }
        //public Acarreo objAcarreo { get; set; }
        [Inject] public IAcarreosService iAcarreoService { get; set; }
        [Inject] public ICatClientesService iClientesService { get; set; }
        [Inject] public ICatServicioService iCatServiciosService { get; set; }
        [Inject] public ICatTipoEstadoService iTipoEstadoService { get; set; }
        [Inject] public ICatProveedorService iCatProveedorService { get; set; }
        [Inject] public IJSRuntime Js { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private ILoginService loginService { get; set; }

        //private ICollection<UltimaMillaDetalle> _detalles;
        private List<DtAcarreos> pagedData;
        private ICollection<CatTipoEstados> _tiposEstado;
        private int pageSize = 10;
        private int currentPage = 1;
        private int totalPages;
        private DtAcarreos _objAcarreo;
        private DtAcarreos _objAcarreoWork;
        private int _idTipoEstadoSeleccionado;
        private string _disabled;
        IJSObjectReference modulo;
        private bool mostrarToast = false;
        private List<string> _errores = new List<string>();
        private DtAcarreos? _encabezadoSeleccionado;
        private ICollection<RespListarCoincidenciasDTO> respListarCoincidencias;
        RespListarCoincidenciasDTO selectedCliente;
        private UtileriasPage objUtileriasPage = new UtileriasPage();
        private List<CatTipoEstados> objcatTipoEstados = new List<CatTipoEstados>();
        private Dictionary<string, bool> tieneAcceso = new Dictionary<string, bool>();
        private string strPproveedor;
        private ICollection<RespListarCoincidenciasDTO> respListarCoincidenciasProv;
        private ICollection<RespuestaGenericaCatalogosDTO> respListarCoincidenciasGenProv;
        private RespListarCoincidenciasDTO selectedProveedor;
        private RespuestaGenericaDTO objRespGenericaDTO;
        private bool isLoading = false;
        private int intIdCatProveedor = 0;
        private int[] lstintIdCatEmpresa;
        private int intIdCatCliente = 0;
        #region Editables
        private bool _esDeshabilitado = true;
        private bool _esDeshabilitadoServicio = true;
        private bool _esDeshabilitadoEstado = true;
        private bool _esDeshabilitadoCliente = true;
        private bool _esDeshabilitadoFechaEntrega = true;
        private bool _esDeshabilitadoFechaSolicitud = true;
        private bool _esDeshabilitadoProveedor = true;
        private bool _esDeshabilitadoAduana = true;
        private bool _esDeshabilitadoContenedor = true;
        private int? filaEnEdicionId = null;
        #endregion Editables

        #endregion Variables



        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            objcatTipoEstados = objUtileriasPage.ObtenerTipoEstados();

            #region ValidarAcceso
            try
            {

                if (UsuarioTokenDTO.catUsuarios == null)
                {
                    UsuarioTokenDTO = await loginService.ObtenerdatosToken();
                    if (UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas != null)
                    {
                        if (UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.FirstOrDefault().IdCatProveedor != null)
                            intIdCatProveedor = (int)UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.FirstOrDefault().IdCatProveedor;
                        if (UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.FirstOrDefault().IdCatCliente != null)
                            intIdCatCliente = (int)UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.FirstOrDefault().IdCatCliente;
                        if (UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.FirstOrDefault().IdCatCliente != null)
                        {
                            lstintIdCatEmpresa = UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.Select(e => e.idCatEmpresa ?? 0).ToArray();

                        }
                    }
                }
                if (!await loginService.validarAccesoPagina(3, UsuarioTokenDTO))
                {
                    _navigationManager.NavigateTo("/");
                }
                tieneAcceso = await loginService.validarControlesPagina(2, UsuarioTokenDTO);

            }
            catch (Exception ex)
            {

                _navigationManager.NavigateTo("/");
            }
            #endregion ValidarAcceso

            switch (paramStrTipoAccion)
            {


                case "M":
                case "C":
                    //_esEditable = false;
                    _objAcarreo = await iAcarreoService.ObtenerAcarreo(paramIdEncabezado);

                    break;
                case "A":
                    _objAcarreo = new DtAcarreos();
                    _objAcarreo.IdDtAcarreos = 0;
                    _objAcarreo.IdCatServicio = 76;
                    _objAcarreo.IdCatTipoEstado = 1;
                    _objAcarreo.IdEmpresa = 1;
                    _objAcarreo.IdUsuarioRegistro = 1;
                    objRespGenericaDTO = await iCatServiciosService.ObternerPorId(_objAcarreo.IdCatServicio);
                    _objAcarreo.catServicios = objRespGenericaDTO.Entidad as CatServicios;
                    break;
                default:
                    _navigationManager.NavigateTo("/");
                    break;
            }
            setEditables(paramStrTipoAccion);
            isLoading = false;


        }





        public async void Submit(DtAcarreos pArg)
        {
            try
            {


                switch (paramStrTipoAccion)
                {

                    case "M":
                        var blRespuesta = await iAcarreoService.Actualizar(pArg);

                        if (blRespuesta)
                        {
                            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = $"La actualización correcta de la orden {pArg.IdOrden}", Detail = "Registro Actualizado" });
                        }
                        else
                        {
                            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = $"Error en la actualización correcta de la orden {pArg.IdOrden}", Detail = "Error en actualización" });
                        }
                        break;
                    case "A":

                        pArg.IdDtAcarreos = 0;
                        pArg.FechaRegistro = DateTime.Now;
                        pArg.Fecha = DateTime.Now;
                        pArg.Mes = DateTime.Now.Month;
                        pArg.IdEmpresa = 1;
                        pArg.IdCatTipoEstado = 1;
                        pArg.Activo = true;
                        pArg.IdUsuarioRegistro = _objAcarreo.IdUsuarioRegistro;
                        pArg.IdCatServicio = _objAcarreo.IdCatServicio;
                        pArg.IdCliente = _objAcarreo.IdCliente;

                        //Validar Objeto
                        var lstValidacion = validarObjeto(pArg);

                        if (lstValidacion.Count == 0)
                        {

                            var objRespuesta = await iAcarreoService.CrearAcarreo(pArg);

                            if (objRespuesta == null || objRespuesta.MensajeError.Length > 0)
                                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = $"Error en la actualización {objRespuesta.MensajeError}", Detail = "Error en actualización" });
                            else

                                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = $"La actualización correcta de la orden {pArg.IdOrden}", Detail = "Registro Actualizado" });

                        }
                        else
                        {
                            string erroresConcatenados = string.Join(", ", lstValidacion);


                            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error en la actualización", Detail = $"Error en actualización: {erroresConcatenados}", Duration = 4000 });
                        }

                        break;
                    default:
                        //Navegar a pagina de error.
                        break;



                }
                //Validar
            }
            catch (Exception ex)
            {

                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = $"Error en la actualización: {ex.Message}", Detail = "Error en actualización" }); ;
            }


        }

        void Cancel()
        {
            //
            _navigationManager.NavigateTo("Acarreos");
        }
        void ButtonClicked()
        {
            // Handle the Click event of RadzenButton
        }
        /******************************************/
        private async Task ActualizarTipoEstatdo(int idTipoEstado)
        {
            int idTipoEstadoActual = _idTipoEstadoSeleccionado;
            bool respuesta = await iAcarreoService.CambiarEstado(paramIdEncabezado, idTipoEstado);

            if (respuesta)
            {
                _objAcarreo.IdCatTipoEstado = idTipoEstado;
                _idTipoEstadoSeleccionado = idTipoEstado;
                mostrarToast = true;
                //await modulo.ToastrSuccess("Post borrado correctamente");
            }
            else
            {
                _objAcarreo.IdCatTipoEstado = idTipoEstadoActual;
                var tipoEstado = _tiposEstado.FirstOrDefault(t => t.IdCatTipoEstados == idTipoEstadoActual);
                _errores.Add($"El estado actual de la orden es {tipoEstado.Nombre}, no es permitido efectuar cambios de estatus");
                await Js.InvokeVoidAsync("bootstrapInterop.showModal", "myModal");
            }
        }

        private List<string> validarObjeto(DtAcarreos pArg)
        {
            List<string> lstresult = new List<string>();
            if (pArg.IdCliente == 0)
                lstresult.Add("Falta Cliente");
            if (pArg.IdEmpresa == 0)
                lstresult.Add("Falta Empresa");
            if (pArg.IdCatServicio == 0)
                lstresult.Add("Falta Servicio");
            if (pArg.IdCatTipoEstado == 0)
                lstresult.Add("Falta tipo de estado");
            if (pArg.IdUsuarioRegistro == 0)
                lstresult.Add("Falta usuario registro");
            if (String.IsNullOrEmpty(pArg.catClientes.RazonSocial))
                lstresult.Add("Falta usuario registro");
            if (String.IsNullOrEmpty(pArg.catServicios.Nombre))
                lstresult.Add("Falta usuario registro");
            if (pArg.FechaRegistro.Date < DateTime.Now.Date && pArg.IdDtAcarreos == 0)
                lstresult.Add("La fecha de registro no puede ser menor a la fecha actual");
            if (pArg.Fecha.Date < pArg.FechaRegistro.Date)
                lstresult.Add("La fecha de entrega no puede ser menor a la fecha actual");

            return lstresult;
        }


        private void ObtenerEncabezadoEditado(DtAcarreos pAcarreo)
        {
            // //Console.WriteLine($"Recibo el encabezado editado {JsonConvert.SerializeObject(pAcarreo)}");
        }



        private void SetEstaEditando(int id)
        {
            filaEnEdicionId = id;
        }



        private async Task GuardarCambios(DtAcarreos detalle)
        {
            //Console.WriteLine($"detalle recibido {JsonConvert.SerializeObject(detalle)}");
            //await detalleService.Actualizar(detalle);
            // Aquí puedes guardar los cambios en la base de datos o la lista
            filaEnEdicionId = null; // Salir del modo edición
        }

        private void CancelarEdicion()
        {
            filaEnEdicionId = null; // Salir del modo edición sin guardar
        }

        private bool setEditables(string pTipoEdicion)
        {
            #region Varibles
            _esDeshabilitado = true;
            _esDeshabilitadoServicio = true;
            _esDeshabilitadoEstado = true;
            _esDeshabilitadoCliente = true;
            _esDeshabilitadoFechaEntrega = true;
            _esDeshabilitadoFechaSolicitud = true;
            _esDeshabilitadoProveedor = true;
            _esDeshabilitadoAduana = true;
            _esDeshabilitadoContenedor = true;
            #endregion Variables
            if (pTipoEdicion.Contains("A") && tieneAcceso["CREAR"])
            {
                _esDeshabilitadoServicio = true;
                _esDeshabilitadoCliente = true;
                _esDeshabilitadoFechaEntrega = true;
                _esDeshabilitadoFechaSolicitud = true;
                _esDeshabilitadoProveedor = true;
                _esDeshabilitadoAduana = true;
                _esDeshabilitadoEstado = true;
                _esDeshabilitadoContenedor = true;
            }

            if (pTipoEdicion.Contains("M") && tieneAcceso["ACTUALIZAR"])
            {
                _esDeshabilitadoServicio = true;
                _esDeshabilitadoCliente = true;
                _esDeshabilitadoFechaEntrega = true;
                _esDeshabilitadoFechaSolicitud = true;
                _esDeshabilitadoProveedor = true;
                _esDeshabilitadoAduana = true;
                _esDeshabilitadoEstado = false;
                if (_objAcarreo.IdCatTipoEstado == 1)
                    _esDeshabilitadoContenedor = false;
                else
                    _esDeshabilitadoContenedor = true;
            }
            if (pTipoEdicion.Contains("B") && tieneAcceso["ELIMINAR"])
            {

            }
            if (intIdCatProveedor > 0)
            {
                _esDeshabilitadoServicio = true;
                _esDeshabilitadoCliente = true;
                _esDeshabilitadoFechaEntrega = true;
                _esDeshabilitadoFechaSolicitud = true;
                _esDeshabilitadoProveedor = true;
                _esDeshabilitadoAduana = true;
                _esDeshabilitadoEstado = false;
                _esDeshabilitadoContenedor = true;

            }

            if (pTipoEdicion.Contains("C") || _objAcarreo.IdCatTipoEstado == (int)UtileriasPage.TiposEstados.CANCELADO || _objAcarreo.IdCatTipoEstado == (int)UtileriasPage.TiposEstados.TERMINADO)
            {
                _esDeshabilitadoServicio = true;
                _esDeshabilitadoCliente = true;
                _esDeshabilitadoFechaEntrega = true;
                _esDeshabilitadoFechaSolicitud = true;
                _esDeshabilitadoProveedor = true;
                _esDeshabilitadoAduana = true;
                _esDeshabilitadoEstado = true;
                _esDeshabilitadoContenedor = true;
            }

            return true;

        }

        void OnClienteChange(object value)
        {

            _objAcarreo.IdCliente = 0;
            _objAcarreo.catClientes = new CatClientes();
            _objAcarreo.catClientes.RazonSocial = "";
            selectedCliente = respListarCoincidencias
          .FirstOrDefault(c => c.RazonSocial == value?.ToString());

            if (selectedCliente != null)
            {
                var selectedId = selectedCliente.Id;
                _objAcarreo.IdCliente = selectedId;
                _objAcarreo.catClientes.IdCatCliente = selectedId;
                _objAcarreo.catClientes.RazonSocial = selectedCliente.RazonSocial;


                // Aquí puedes asignar el ID seleccionado a una propiedad o realizar otras acciones
            }

        }

        async Task OnLoadCliente(LoadDataArgs args)
        {

            var strQuery = args.Filter;
            respListarCoincidencias = await iClientesService.GetClientesConincidencia(strQuery);




        }

        #region AutoCompleteProveedor
        void AutoCompleteOnProveedorChange(object value)
        {


            selectedCliente = respListarCoincidencias
          .FirstOrDefault(c => c.RazonSocial == value?.ToString());

            if (selectedCliente != null)
            {
                var selectedId = selectedCliente.Id;



                // Aquí puedes asignar el ID seleccionado a una propiedad o realizar otras acciones
            }

        }
        async Task AutoCompleteOnLoadProveedor(LoadDataArgs args)
        {

            var strQuery = args.Filter;
            respListarCoincidenciasProv = await iCatProveedorService.ObtenerProveedoresConincidencia(strQuery);




        }
        async Task AutoCompleteUpdateProveedor()
        {

        }
        #endregion AutoCompleteProveedor



    }
}
