using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using AlogisticsWASM.Layout.Utilerias;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Logisticos.Acarreos
{
    public partial class AcarreosPageRZ : ComponentBase
    {
        #region Variables
        [Inject] public IAcarreosService iAcarreoService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private ILoginService loginService { get; set; }

        int count;
        private FiltroDtAcarreosDTO _filtro;
        ODataEnumerable<RespObtenerAcarreosDTO> ODobjrespObtenerAcarreosDTO;
        IList<RespObtenerAcarreosDTO> selectedEmployees;
        RadzenDataGrid<RespObtenerAcarreosDTO> grid;
        private ICollection<RespObtenerAcarreosDTO> _lstAcarreos;
        private ICollection<RespObtenerAcarreosDTO> _lstAcarreosWork;
        private List<RespObtenerAcarreosDTO> _lstAcarreosPage;
        private List<RespObtenerAcarreosDTO> service;
        private List<RespObtenerAcarreosDTO> _lstAcarreosPageWork;
        private IEnumerable<RespObtenerAcarreosDTO> customers;
        LogicalFilterOperator logicalFilterOperator = LogicalFilterOperator.And;
        FilterCaseSensitivity filterCaseSensitivity = FilterCaseSensitivity.CaseInsensitive;
        private DateTime dFSolicitudIni;
        private DateTime dFSolicitudFin;
        private DateTime dFSolicitudCierre;
        private string strPcontenedor;
        private string strPcliente;
        private string strProveedor;
        private string strAccionDetalle = "C";
        private int intIdCatProveedor = 0;
        private int[] lstintIdCatEmpresa;
        private int intIdCatCliente = 0;
        IEnumerable<string> selectedCustomers;
        private UtileriasPage objUtileriasPage = new UtileriasPage();
        private bool isLoading = false;
        List<string> titles = new List<string> { "Sales Representative", "Vice President, Sales", "Sales Manager", "Inside Sales Coordinator" };
        IEnumerable<string> selectedTitles;

        int position = 1;

        #endregion Variables




        void Change(string text)
        {
            ////Console.WriteLine.Log($"{text}");
        }
        async Task OnSelectedTitlesChange(object value)
        {
            if (selectedTitles != null && !selectedTitles.Any())
            {
                selectedTitles = null;
            }
            await grid.FirstPage();
        }

        private async Task EnviarDatos()
        {
            isLoading = true;
            _filtro = new FiltroDtAcarreosDTO();
            _filtro.Contenedor = strPcontenedor;
            if (intIdCatProveedor != 0)
            {
                _filtro.IdCatProveedor = intIdCatProveedor;
            }
            if (intIdCatCliente != 0)
            {
                _filtro.IdCliente = intIdCatCliente;
            }
            _filtro.Cliente = strPcliente;
            _filtro.FSolicitudIni = dFSolicitudIni;
            _filtro.FSolicitudFin = dFSolicitudFin;
            await ObtenerDatos();
            isLoading = false;
        }
        async Task OnSelectedCustomersChange(object value)
        {
            if (selectedCustomers != null && !selectedCustomers.Any())
            {
                selectedCustomers = null;
            }

            await grid.FirstPage();
        }

        private async Task ObtenerDatos()
        {

            _lstAcarreos = new List<RespObtenerAcarreosDTO>();

            _lstAcarreos = await iAcarreoService!.ObtenerAcarreos(_filtro);
            ////Console.WriteLine(_lstAcarreos);



            if (_lstAcarreos == null)
            {
                _lstAcarreos = new List<RespObtenerAcarreosDTO>();
            }
            else
            {
                //_lstAcarreosWork = _lstAcarreos;
                customers = _lstAcarreos.AsEnumerable();

                var result = customers;
                // Update the Data property
                ODobjrespObtenerAcarreosDTO = result.AsODataEnumerable();


            }
        }





        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
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


            }
            catch (Exception ex)
            {

                _navigationManager.NavigateTo("/");
            }
            #endregion ValidarAcceso



            _filtro = new FiltroDtAcarreosDTO();
            _filtro.NumeroPagina = 1;
            _filtro.NumeroRegistros = 20;
            if (intIdCatProveedor != 0)
            {
                _filtro.IdCatProveedor = intIdCatProveedor;
            }
            if (intIdCatCliente != 0)
            {
                _filtro.IdCliente = intIdCatCliente;
            }
            //_filtro.IdCatTipoEstados = 2;
            dFSolicitudIni = DateTime.Now.AddDays(-7);
            dFSolicitudFin = DateTime.Now;
            dFSolicitudIni = DateTime.Parse("2024-07-01");
            dFSolicitudFin = DateTime.Parse("2024-07-30");


            _filtro.FSolicitudIni = dFSolicitudIni;
            _filtro.FSolicitudFin = dFSolicitudFin;
            //Validar con token
            //_filtro.IdCliente = UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.First().IdCatCliente ?? 0;
            //_filtro.IdEmpresa = UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.First().idCatEmpresa ?? 0;

            _filtro.Activo = true;
            await ObtenerDatos();
            isLoading = false;


        }

        private void OnClick(string pText)
        {

            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = "Cambios Habilitados", Detail = pText });
        }
        async Task LoadData(LoadDataArgs args)
        {
            _filtro = new FiltroDtAcarreosDTO();

            isLoading = true;

            //var result = await service.GetEmployees(filter: args.Filter, top: args.Top, skip: args.Skip, orderby: args.OrderBy, count: true, expand: "NorthwindOrders($expand=Customer)");
            await ObtenerDatos();
            var result = customers;
            // Update the Data property
            ODobjrespObtenerAcarreosDTO = result.AsODataEnumerable();
            // Update the count
            count = result.Count();

            isLoading = false;
        }
        private void VerDetalles(RespObtenerAcarreosDTO encabezado)
        {
            var strTipoAccion = "M";
            if (encabezado.IdCatTipoEstado == (int)UtileriasPage.TiposEstados.CANCELADO || encabezado.IdCatTipoEstado == (int)UtileriasPage.TiposEstados.TERMINADO)
            {
                strTipoAccion = "C";
            }
            _navigationManager.NavigateTo($"AcarreosEditar/{encabezado.idDtAcarreos}/{strTipoAccion}");
        }
        private void CrearAcarreo()
        {

            _navigationManager.NavigateTo($"AcarreosEditar/0/A");
        }


        #region Modal
        async Task OpenModal(RespObtenerAcarreosDTO prespObtenerAcarreosDTO, string pStrTipoAccion)
        {
            int pIdAcarreo = 0;
            if (prespObtenerAcarreosDTO != null)
            {
                pIdAcarreo = prespObtenerAcarreosDTO.idDtAcarreos;
            }
            if (pStrTipoAccion != "A")
            {
                if (prespObtenerAcarreosDTO.IdCatTipoEstado == (int)UtileriasPage.TiposEstados.CANCELADO || prespObtenerAcarreosDTO.IdCatTipoEstado == (int)UtileriasPage.TiposEstados.TERMINADO)
                {
                    pStrTipoAccion = "C";
                }
            }
            var parameters = new Dictionary<string, object> { { "paramIdEncabezado", pIdAcarreo } };
            parameters.Add("paramStrTipoAccion", pStrTipoAccion);
            var options = new DialogOptions
            {
                Width = "750px", // Cambia el ancho del modal
                Height = "650px" // Cambia la altura del modal
            };
            await DialogService.OpenAsync<AcarreosCRUDPage>("Detalle Acarreos", parameters, options);
        }
        #endregion Modal
    }
}
