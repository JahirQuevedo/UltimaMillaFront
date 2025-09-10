
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
namespace AlogisticsWASM.Pages.Vacios.Referencias
{
    public partial class ReferenciasDetallePage : ComponentBase
    {
        #region Parametros
        [Parameter] public int paramIdOrden { get; set; } = 0;
        [Parameter] public string paramstrTipoAccion { get; set; }
        #endregion Parametros
        #region Variables

        [Inject] public IOrdenService iordenService { get; set; }
        [Inject] public IReferenciaService ireferenciaService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private SweetAlertService sweetAlertService { get; set; }
        [Inject] private ILoginService loginService { get; set; }

        bool isLoading = true;
        int count = 0;
        private FiltroOrdenesReferenciasDTO _filtro = new FiltroOrdenesReferenciasDTO();
        ODataEnumerable<PeticionesReferencias> _ODReferencias;
        ODataEnumerable<PeticionesContenedores> _ODContenedores;
        IList<PeticionesReferencias> selectedReferencia = new List<PeticionesReferencias>();
        RadzenDataGrid<PeticionesReferencias> grid = new RadzenDataGrid<PeticionesReferencias>();
        IEnumerable<PeticionesContenedores> _IEContenedores = new List<PeticionesContenedores>();
        IEnumerable<PeticionesServicios> _IEServicio = new List<PeticionesServicios>();
        IEnumerable<PeticionesDocumentos> _IEDocumentos = new List<PeticionesDocumentos>();
        RadzenDataGrid<PeticionesReferencias> grid2 = new RadzenDataGrid<PeticionesReferencias>();
        private ICollection<PeticionesReferencias> _lstReferencias = new List<PeticionesReferencias>();
        private PeticionesReferencias _objReferencia = new PeticionesReferencias();
        private List<PeticionesReferencias> _lstReferenciasPage = new List<PeticionesReferencias>();
        private Ordenes _objOrden = new Ordenes();
        private IEnumerable<PeticionesReferencias> _IEReferencias = new List<PeticionesReferencias>();
        LogicalFilterOperator logicalFilterOperator = LogicalFilterOperator.And;
        FilterCaseSensitivity filterCaseSensitivity = FilterCaseSensitivity.CaseInsensitive;

        private RespObtenerReferenciasDTO objRespObtenerReferenciasDTO = null;
        private DateTime dFSolicitudIni;
        private DateTime dFSolicitudFin;
        private DateTime dFSolicitudCierre;
        private string strPcontenedor;
        private string strClienteRazonSocial = "";
        private string strPcliente;
        private string strAduana = "";
        private int intIdReferencia = 0;

        private int intPcliente = 0;
        private string strProveedor;
        private string strSucursal = "";
        private int intProveedor = 0;
        IEnumerable<string> selectedCustomers = new List<string>();
        int position = 1;
        private Dictionary<string, bool> dicAcceso = new Dictionary<string, bool>();
        private bool dicAccesoActualizar = false;
        private bool blhabilitacontroles = false;
        #endregion Variables

        #region Init
        protected override async Task OnInitializedAsync()
        {
            #region ValidarAcceso
            try
            {
                if (_UsuarioTokenDTO.IdCatUsuario == 0)
                {
                    _UsuarioTokenDTO = await loginService.ObtenerdatosToken();

                }
                if (!await loginService.validarAccesoPagina(10, _UsuarioTokenDTO))
                {

                    _navigationManager.NavigateTo("/");
                }
                else
                {

                    dicAcceso = await loginService.validarControlesPagina(2, _UsuarioTokenDTO);
                    dicAccesoActualizar = dicAcceso["ACTUALIZAR"];


                }

            }
            catch (Exception ex)
            {
                ////Console.WriteLine("Error:" + ex.Message);
                _navigationManager.NavigateTo("/");
            }
            #endregion ValidarAcceso

            isLoading = true;
            _objOrden = new Ordenes();

            await ObtenerDatos();
            isLoading = false;
        }
        #endregion Init

        #region Radzen
        void Change(string text)
        {
            //////Console.WriteLine.Log($"{text}");
        }

        private async Task EnviarDatos()
        {
            _filtro = new FiltroOrdenesReferenciasDTO();
            //_filtro.Contenedor = strPcontenedor;
            //_filtro.FechaSolicitudIni = dFSolicitudIni;
            //_filtro.FechaSolicitudFin = dFSolicitudFin;
            _filtro.IdOrden = paramIdOrden;

            await ObtenerDatos();
        }

        private void VerDocumento(PeticionesDocumentos pDocumento)
        {

        }
        #region GridRender
        void OnContenedoresDataGridRender(DataGridRenderEventArgs<PeticionesContenedores> args)
        {
            //if (args.FirstRender)
            //{
            //    InvokeAsync(() => args.Grid.ExpandRow(args.Grid.View.Where(c => c.Servicios.Any()).FirstOrDefault()));
            //}
            try
            {


                if (args.FirstRender)
                {
                    var rowToExpand = args.Grid.View.Where(c => c.Servicios.Any()).FirstOrDefault();
                    if (rowToExpand != null)
                    {
                        InvokeAsync(() => args.Grid.ExpandRow(rowToExpand));
                    }
                }
            }
            catch (NullReferenceException ex)
            {

                ////Console.WriteLine($"OnContenedoresDataGridRender: {ex.Message}");
            }
            catch (Exception nex)
            {
                ////Console.WriteLine($"OnContenedoresDataGridRender: {nex.Message}");
            }

        }

        void OnServiciosDataGridRender(DataGridRenderEventArgs<PeticionesServicios> args)
        {
            try
            {
                if (args.FirstRender)
                {
                    InvokeAsync(() => args.Grid.ExpandRow(args.Grid.View.Where(c => c.Documentos.Any()).FirstOrDefault()));
                }
            }
            catch (NullReferenceException ex)
            {

                ////Console.WriteLine($"OnContenedoresDataGridRender: {ex.Message}");
            }
            catch (Exception nex)
            {
                ////Console.WriteLine($"OnContenedoresDataGridRender: {nex.Message}");
            }
        }


        #endregion GridRender

        #endregion Radzen

        #region Operaciones


        private async Task ObtenerDatos()
        {


            _lstReferencias = new List<PeticionesReferencias>();
            _objReferencia = new PeticionesReferencias();
            _objOrden = new Ordenes();
            _objOrden = await iordenService!.GetOrden(paramIdOrden);

            if (_objOrden != null)
            {
                strClienteRazonSocial = _objOrden.catClientes.RazonSocial;
                strSucursal = _objOrden.CatSucursales.Nombre;
                strAduana = _objOrden.catAduana.Acronimo;
            }

            intIdReferencia = _objOrden.peticionesReferencias.FirstOrDefault().IdReferencia;

            _objReferencia = await ireferenciaService!.ObtenerReferencia(intIdReferencia);


            if (_objReferencia == null)
            {
                _objReferencia = new PeticionesReferencias();
            }
            else
            {
                //_lstAcarreosWork = _lstAcarreos;
                _lstReferencias.Add(_objReferencia);
                var result = _lstReferencias.AsEnumerable();
                // Update the Data property
                _ODReferencias = result.AsODataEnumerable();
                _IEReferencias = _lstReferencias.AsEnumerable();

                //string json = JsonSerializer.Serialize(_objReferencia, new JsonSerializerOptions { WriteIndented = true });

                // Escribir el JSON en la consola
                //////Console.WriteLine("JSON:" + json);

                var contenedoresList = _IEReferencias
                   .Where(referencia => referencia.Contenedores != null)
                    .SelectMany(referencia => referencia.Contenedores)
                    .ToList();


                // Asignar la lista combinada a _ODContenedores
                _IEContenedores = contenedoresList.AsEnumerable();


                //Obtenemos Servicios.
                var serviciosList = _IEContenedores
                    .Where(contenedor => contenedor.Servicios != null)
                    .SelectMany(contenedor => contenedor.Servicios)
                    .ToList();
                _IEServicio = serviciosList.AsEnumerable();


                var DocumentosList = _IEServicio
                    .Where(serv => serv.Documentos != null)
                    .SelectMany(serv => serv.Documentos)
                    .ToList();
                _IEDocumentos = DocumentosList.AsEnumerable();

            }
        }

        private void CrearReferencia()
        {
            _navigationManager.NavigateTo("ordenes-crear");
        }

        private void VerDetalles(RespObtenerReferenciasDTO referencia)
        {
            //ObtenerReferenciaSeleccionada(referencia);
            _navigationManager.NavigateTo($"/contenedores/{referencia.IdReferencia}");
        }
        #endregion Operaciones
    }
}
