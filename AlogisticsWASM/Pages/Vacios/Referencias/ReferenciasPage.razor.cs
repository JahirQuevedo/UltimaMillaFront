
using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Vacios;
using AlogisticsWASM.Layout.Utilerias;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Security.Claims;

namespace AlogisticsWASM.Pages.Vacios.Referencias
{
    public partial class ReferenciasPage : ComponentBase
    {

        #region Variables
        [Inject] public IReferenciaService iReferenciasService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private ICatClientesService icatClientesService { get; set; }
        [Inject] private ICatPatiosServices icatPatiosServices { get; set; }
        [Inject] private ICatNavieraService icatNavieraService { get; set; }
        [Inject] private ICatAduanaService icatAduanaService { get; set; }
        [Inject] private SweetAlertService sweetAlertService { get; set; }
        [Inject] private ILoginService loginService { get; set; }





        bool isLoading;
        int count;
        private FiltroOrdenesReferenciasDTO _filtro;
        ODataEnumerable<RespObtenerReferenciasDTO> _ODReferencias;
        ODataEnumerable<PeticionesContenedores> _ODContenedores;
        IList<RespObtenerReferenciasDTO> selectedReferencia;
        RadzenDataGrid<RespObtenerReferenciasDTO> grid;
        IEnumerable<PeticionesContenedores> _IEContenedores = new List<PeticionesContenedores>();
        IEnumerable<PeticionesServicios> _IEServicio = new List<PeticionesServicios>();
        IEnumerable<RespObtenerReferenciasDTO> _IEReferencias2;
        RadzenDataGrid<RespObtenerReferenciasDTO> grid2;
        private ICollection<RespObtenerReferenciasDTO> _lstReferencias;
        private List<RespObtenerReferenciasDTO> _lstReferenciasPage;
        private ICollection<CatAduana> lstAduanas = new List<CatAduana>();
        private IEnumerable<RespObtenerReferenciasDTO> _IEReferencias;
        LogicalFilterOperator logicalFilterOperator = LogicalFilterOperator.And;
        FilterCaseSensitivity filterCaseSensitivity = FilterCaseSensitivity.CaseInsensitive;

        private DateTime dFSolicitudIni;
        private DateTime dFSolicitudFin;
        private DateTime dFSolicitudCierre;
        private string strPcontenedor;
        private string strPcliente;
        private int intPcliente = 0;
        private string strPPatio;
        private int intPPatio = 0;
        private int intPNaviera = 0;
        private string strPrefcliente;
        private string strPAduana;
        private string strSucursal = "";
        private int intPAduana = 0;
        private string strPrefalo;
        private string strPNaviera;
        private UtileriasPage objutileriasPage = new UtileriasPage();

        private string strProveedor;
        private int intProveedor;
        IEnumerable<string> selectedCustomers;
        private ICollection<RespListarCoincidenciasDTO> respListarCoincidencias;
        private ICollection<RespuestaGenericaCatalogosDTO> respListarCoincidenciasGen;
        private ICollection<RespuestaGenericaCatalogosDTO> respListarCoincidenciasNavGen;
        private RespListarCoincidenciasDTO selectedCliente;
        private RespuestaGenericaCatalogosDTO selectedGen;
        private RespuestaGenericaCatalogosDTO selectedGenNaviera;

        private string token;
        private string usuario;
        private IEnumerable<Claim> userClaims;
        private UsuarioTokenDTO objUsuarioTokenDTO = new UsuarioTokenDTO();
        private Dictionary<string, bool> dicAcceso = new Dictionary<string, bool>();
        private bool dicAccesoActualizar = false;
        private bool blhabilitarActualizar = false;
        int position = 1;

        #endregion Variables


        #region FiltroGeneral

        #region AutoCompleteCliente
        void AutoCompleteOnClienteChange(object value)
        {

            intPcliente = 0;
            strPcliente = null;
            selectedCliente = respListarCoincidencias
          .FirstOrDefault(c => c.RazonSocial == value?.ToString());

            if (selectedCliente != null)
            {
                var selectedId = selectedCliente.Id;
                intPcliente = selectedId;
                strPcliente = selectedCliente.RazonSocial;


                // Aquí puedes asignar el ID seleccionado a una propiedad o realizar otras acciones
            }

        }

        async Task AutoCompleteOnLoadCliente(LoadDataArgs args)
        {

            var strQuery = args.Filter;
            respListarCoincidencias = await icatClientesService.GetClientesConincidencia(strQuery);




        }
        #endregion AutoCompleteCliente

        #region AutoCompletePatio
        void AutoCompleteOnPatioChange(object value)
        {

            intPPatio = 0;
            strPPatio = null;
            selectedGen = respListarCoincidenciasGen
          .FirstOrDefault(c => c.Nombre == value?.ToString());

            if (selectedGen != null)
            {
                var selectedId = selectedGen.Id;
                intPPatio = selectedId;
                strPPatio = selectedGen.Nombre;


                // Aquí puedes asignar el ID seleccionado a una propiedad o realizar otras acciones
            }

        }

        async Task AutoCompleteOnLoadPatio(LoadDataArgs args)
        {

            var strQuery = args.Filter;
            respListarCoincidenciasGen = await icatPatiosServices.GetPatiosConincidencia(strQuery, 0);




        }
        #endregion AutoCompletePatio

        #region AutoCompleteNaviera
        void AutoCompleteOnNavieraChange(object value)
        {

            intPNaviera = 0;
            strPNaviera = null;
            selectedGenNaviera = respListarCoincidenciasNavGen
          .FirstOrDefault(c => c.Nombre == value?.ToString());

            if (selectedGenNaviera != null)
            {
                var selectedId = selectedGenNaviera.Id;
                intPNaviera = selectedId;
                strPNaviera = selectedGenNaviera.Nombre;


                // Aquí puedes asignar el ID seleccionado a una propiedad o realizar otras acciones
            }

        }

        async Task AutoCompleteOnLoadNaviera(LoadDataArgs args)
        {

            var strQuery = args.Filter;
            respListarCoincidenciasNavGen = await icatNavieraService.GetNavierasConincidencia(strQuery);




        }
        #endregion AutoCompleteNaviera

        void DropDownOnAduanaChange()
        {

            try
            {
                if (strPAduana.Length > 0)
                {
                    intPAduana = lstAduanas.Where(x => x.Nombre.Equals(strPAduana)).FirstOrDefault().IdCatAduana;
                }
                else intPAduana = 0;

            }
            catch (Exception ex)
            {
                intPAduana = 0;

            }
        }
        #endregion FiltroGeneral

        #region GridDetalle

        DataGridExpandMode expandMode = DataGridExpandMode.Single;
        bool? allRowsExpanded;


        async Task ToggleRowsExpand(bool? value)
        {
            allRowsExpanded = value;

            if (value == true)
            {
                await grid.ExpandRows(grid.PagedView);
            }
            else if (value == false)
            {
                await grid.CollapseRows(grid.PagedView);
            }
        }



        void RowRender(RowRenderEventArgs<RespObtenerReferenciasDTO> args)
        {
            args.Expandable = args.Data.peticionesContenedores.Any();
        }

        void OnCustomersDataGridRender(DataGridRenderEventArgs<RespObtenerReferenciasDTO> args)
        {
            if (args.FirstRender)
            {
                InvokeAsync(() => args.Grid.ExpandRow(args.Grid.View.Where(c => c.peticionesContenedores.Any()).FirstOrDefault()));
            }
        }

        void OnContenedoresDataGridRender(DataGridRenderEventArgs<PeticionesContenedores> args)
        {
            if (args.FirstRender)
            {
                InvokeAsync(() => args.Grid.ExpandRow(args.Grid.View.Where(c => c.Servicios.Any()).FirstOrDefault()));
            }
        }


        #endregion GridDetalle


        #region Init
        protected override async Task OnInitializedAsync()
        {
            #region ValidarAcceso
            try
            {
                isLoading = true;
                ////Console.WriteLine($"ENTRO REFPAGE VACIO:" + _UsuarioTokenDTO.IdCatUsuario);
                if (_UsuarioTokenDTO.IdCatUsuario == 0)
                {
                    ////Console.WriteLine($"ENTRO REFPAGE VACIO");
                    _UsuarioTokenDTO = await loginService.ObtenerdatosToken();
                    //await UserService.InitializeAsync();
                    ////Console.WriteLine($"SALIO REFPAGE {_UsuarioTokenDTO.Nombre}");
                }
                //else { //Console.WriteLine($"INIT REFPAGE {_UsuarioTokenDTO.Nombre}"); }
                if (!await loginService.validarAccesoPagina(10, _UsuarioTokenDTO))
                {
                    _navigationManager.NavigateTo("/");
                }
                else
                {
                    dicAcceso = await loginService.validarControlesPagina(2, _UsuarioTokenDTO);
                    dicAccesoActualizar = dicAcceso["ACTUALIZAR"];
                    ////Console.WriteLine($"Clave ACTUALIZAR: {dicAccesoActualizar}");
                    //foreach (KeyValuePair<string, bool> par in dicAcceso)
                    //{
                    //    //Console.WriteLine($"Clave: {par.Key}, Valor: {par.Value}");
                    //}
                }

            }
            catch (Exception ex)
            {

                _navigationManager.NavigateTo("/");
            }
            #endregion ValidarAcceso



            isLoading = true;

            //Cargamos Aduanas.
            lstAduanas = await icatAduanaService.GetAduanas();


            _filtro = new FiltroOrdenesReferenciasDTO();

            dFSolicitudIni = DateTime.Now.AddDays(-1);
            dFSolicitudFin = DateTime.Now;
            intPcliente = 0;
            if (_UsuarioTokenDTO.catUsuarios.catUsuariosClientes != null && _UsuarioTokenDTO.catUsuarios.catUsuariosClientes.Count > 0)
            {
                intPcliente = _UsuarioTokenDTO.catUsuarios.catUsuariosClientes?.FirstOrDefault().IdCatCliente ?? 0;
            }

            //Cargar cliente obligado si no es ALO-INCOM, Sacar
            _filtro.FechaSolicitudIni = dFSolicitudIni;
            _filtro.FechaSolicitudFin = dFSolicitudFin;
            _filtro.IdCliente = intPcliente;

            _filtro.IdEmpresa = _UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.Select(e => e.idCatEmpresa.ToString()).ToList();

            //_filtro.Activo = true;
            await ObtenerDatos();


            isLoading = false;


        }
        #endregion Init

        #region OperacionesRadzen


        void Change(string text)
        {
            ////Console.WriteLine.Log($"{text}");
        }

        private async Task EnviarDatos()
        {
            //await sweetAlertService.FireAsync("Hello world!");
            isLoading = true;
            _filtro = new FiltroOrdenesReferenciasDTO();
            _filtro.Contenedor = strPcontenedor;
            _filtro.FechaSolicitudIni = dFSolicitudIni;
            _filtro.FechaSolicitudFin = dFSolicitudFin;
            _filtro.ReferenciaALO = strPrefalo;
            _filtro.ReferenciaCliente = strPrefcliente;
            _filtro.IdCliente = intPcliente;
            _filtro.IdPatio = intPPatio;
            _filtro.IdNaviera = intPNaviera;
            _filtro.IdAduana = intPAduana;


            await ObtenerDatos();
            isLoading = false;
        }
        #endregion OperacionesRadzen

        #region Operaciones
        private async Task ObtenerDatos()
        {
            _filtro.IdLNegocio = 1;
            _lstReferencias = new List<RespObtenerReferenciasDTO>();
            //Console.WriteLine($"ReferenciasRZ => _filtro {JsonConvert.SerializeObject(_filtro, Formatting.Indented)}");
            _lstReferencias = await iReferenciasService!.GetReferencias(_filtro);



            if (_lstReferencias == null)
            {
                _lstReferencias = new List<RespObtenerReferenciasDTO>();
            }
            else
            {
                //_lstAcarreosWork = _lstAcarreos;                
                var result = _lstReferencias.AsEnumerable();
                // Update the Data property
                _ODReferencias = result.AsODataEnumerable();
                _IEReferencias = _lstReferencias.AsEnumerable();

                //string json = JsonSerializer.Serialize(_IEReferencias, new JsonSerializerOptions { WriteIndented = true });

                //// Escribir el JSON en la consola
                //Console.WriteLine("JSON:" + json);

                var contenedoresList = _ODReferencias
                    .Where(referencia => referencia.peticionesContenedores != null)
                    .SelectMany(referencia => referencia.peticionesContenedores)
                    .ToList();

                // Asignar la lista combinada a _ODContenedores
                _IEContenedores = contenedoresList.AsEnumerable();

                //Obtenemos Servicios.
                _IEServicio = _IEContenedores
                    .Where(contenedor => contenedor.Servicios != null)
                    .SelectMany(contenedor => contenedor.Servicios)
                    .ToList();


            }
        }

        private void CargaPlantilla()
        {
            _navigationManager.NavigateTo("vaciosplantilla");
        }
        private void CrearReferencia()
        {
            _navigationManager.NavigateTo("vaciossolicitud");
        }

        private void VerDetalles(RespObtenerReferenciasDTO referencia)
        {
            //ObtenerReferenciaSeleccionada(referencia);
            _navigationManager.NavigateTo($"/referenciasdet/{referencia.IdOrden}/C");
        }


        public async Task CerrarTodosContenedores()
        {
            // Promise/Task based
            //await sweetAlertService.FireAsync("Hello world!");
            await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Are you sure?",
                Text = "You will not be able to recover this imaginary file!",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Yes, delete it!",
                CancelButtonText = "No, keep it"
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (!string.IsNullOrEmpty(result.Value))
                {
                    sweetAlertService.FireAsync(
                        "Deleted",
                        "Your imaginary file has been deleted.",
                        SweetAlertIcon.Success
                        );
                }
                else if (result.Dismiss == DismissReason.Cancel)
                {
                    sweetAlertService.FireAsync(
                        "Cancelled",
                        "Your imaginary file is safe :)",
                        SweetAlertIcon.Error
                        );
                }
            });

        }
        #endregion Operaciones

        #region Modal
        async Task OpenModal(int pIdOrden)
        {
            var parameters = new Dictionary<string, object> { { "paramIdOrden", pIdOrden } };
            parameters.Add("paramstrTipoAccion", "C");
            var options = new DialogOptions
            {
                Width = "90%", // Cambia el ancho del modal
                Height = "90%" // Cambia la altura del modal
            };
            await DialogService.OpenAsync<ReferenciasDetallePage>("Detalle Referencias", parameters, options);
        }
        #endregion Modal

    }
}
