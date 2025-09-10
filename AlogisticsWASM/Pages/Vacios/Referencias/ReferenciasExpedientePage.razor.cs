using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
namespace AlogisticsWASM.Pages.Vacios.Referencias
{
    public partial class ReferenciasExpedientePage : ComponentBase
    {
        #region Parametros
        [Parameter] public int paramIdOrden { get; set; } = 0;
        [Parameter] public int paramIdReferencia { get; set; } = 0;
        [Parameter] public string paramstrTipoAccion { get; set; }
        #endregion Parametros

        #region Variables

        [Inject] public IOrdenService iordenService { get; set; }
        [Inject] public IReferenciaService ireferenciaService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private ILoginService loginService { get; set; }

        bool isLoading;
        int count;
        private FiltroOrdenesReferenciasDTO _filtro;
        ODataEnumerable<PeticionesReferencias> _ODReferencias;
        ODataEnumerable<PeticionesContenedores> _ODContenedores;
        IList<PeticionesReferencias> selectedReferencia;
        RadzenDataGrid<PeticionesReferencias> grid;
        IEnumerable<PeticionesContenedores> _IEContenedores;
        RadzenDataGrid<PeticionesReferencias> grid2;
        private ICollection<PeticionesReferencias> _lstReferencias;
        private PeticionesReferencias _objReferencia;
        private List<PeticionesReferencias> _lstReferenciasPage;
        private Ordenes _objOrden = new Ordenes();
        private IEnumerable<PeticionesReferencias> _IEReferencias;
        LogicalFilterOperator logicalFilterOperator = LogicalFilterOperator.And;
        FilterCaseSensitivity filterCaseSensitivity = FilterCaseSensitivity.CaseInsensitive;
        private DateTime dFSolicitudIni;
        private DateTime dFSolicitudFin;
        private DateTime dFSolicitudCierre;
        private string strPcontenedor;
        private string strPcliente;


        private int intPcliente;
        private string strProveedor;
        private int intProveedor;
        IEnumerable<string> selectedCustomers;
        int position = 1;

        #endregion Variables

        #region Radzen
        void Change(string text)
        {
            ////Console.WriteLine.Log($"{text}");
        }

        private async Task EnviarDatos()
        {
            _filtro = new FiltroOrdenesReferenciasDTO();
            //_filtro.Contenedor = strPcontenedor;
            //_filtro.FechaSolicitudIni = dFSolicitudIni;
            //_filtro.FechaSolicitudFin = dFSolicitudFin;
            _filtro.IdOrden = paramIdOrden;

            //await ObtenerDatos();
        }
        #region GridRender
        void OnContenedoresDataGridRender(DataGridRenderEventArgs<PeticionesContenedores> args)
        {
            if (args.FirstRender)
            {
                InvokeAsync(() => args.Grid.ExpandRow(args.Grid.View.Where(c => c.Servicios.Any()).FirstOrDefault()));
            }
        }
        #endregion GridRender

        #endregion Radzen

        protected override async Task OnInitializedAsync()
        {

            #region ValidarAcceso
            try
            {

                if (UsuarioTokenDTO.catUsuarios == null)
                {
                    UsuarioTokenDTO = await loginService.ObtenerdatosToken();
                }
                if (!await loginService.validarAccesoPagina(2, UsuarioTokenDTO))
                {
                    _navigationManager.NavigateTo("/");
                }


            }
            catch (Exception ex)
            {

                _navigationManager.NavigateTo("/");
            }
            #endregion ValidarAcceso
        }

    }
}
