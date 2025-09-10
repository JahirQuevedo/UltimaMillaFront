using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using AlogisticsWASM.Layout.Vacios.Filtros;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.ControlTower
{
    public partial class VaciosControlTowerPage
    {

        [Inject] private SweetAlertService Swal { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private IReferenciaService ReferenciaService { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Inject] private UsuarioTokenDTO UsuarioTokenDTO { get; set; }
        [Inject] private ILoginService LoginService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private List<PeticionesContenedores> _contenedoresPendientes;
        private List<PeticionesContenedores> _contenedoresPendientesToInsert;
        private IList<PeticionesContenedores> _contenedoresPendientesSeleccionados;
        private List<Ordenes> _ordenes;
        private List<Ordenes> _ordenesProceso;
        private List<Ordenes> _ordenesFiltradas;
        private IList<Ordenes> _ordenesSeleccionadas;

        private List<PeticionesContenedores> _contenedoresProceso;
        private IList<PeticionesContenedores> _contenedoresProcesoSeleccionados;
        private RadzenDataGrid<PeticionesContenedores> _contenedoresProcesoGrid;
        private DateTime _fechaInicio = DateTime.Now;
        private string _contenedor = "";
        private bool _mostrarFiltros;
        private bool _mostrarBotonesInicioProceso;
        private bool _mostrarBotonServicios;
        private bool _cargando;
        private int _indexTab;
        private bool _isBusyAsignarPatio;
        private bool _isBusyInciarProceso;

        public GridContenedoresPendientesCmp _gridContenedoresPendientes { get; set; }
        public GridOrdenesCmp _gridOrdenesCmp { get; set; }
        public GridOrdenesCmp _gridOrdenesProcesoCmp { get; set; }
        public FiltroContenedoresCmp _filtroContenedores { get; set; }

        private int _totalContenedoresPendientesSeleccionados;

        protected override async Task OnInitializedAsync()
        {

            _totalContenedoresPendientesSeleccionados = 0;
            _mostrarBotonesInicioProceso = true;
            _mostrarFiltros = false;
            _isBusyAsignarPatio = false;
            _isBusyInciarProceso = false;
            _contenedoresPendientesSeleccionados = new List<PeticionesContenedores>();
            _contenedoresPendientesToInsert = new List<PeticionesContenedores>();

            _contenedoresProceso = new List<PeticionesContenedores>();
            _contenedoresProcesoSeleccionados = new List<PeticionesContenedores>();
            _ordenesSeleccionadas = new List<Ordenes>();

            _ordenes = new List<Ordenes>();

            await ObtenerDatosUsuarioAsync();
            //await GetOrdenesPendientes();
            SetFiltro();
            await GetOrdenesAsync();
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
                        //_esClienteExterno = roles.Any(r => r.CatRoles.Nombre.Equals("CLIENTE"));
                        var c = await LoginService.validarAccesoPagina(2, UsuarioTokenDTO); ;
                        var r = !await LoginService.validarAccesoPagina(2, UsuarioTokenDTO);
                        var a = (!UsuarioTokenDTO.Rol.Equals("ADMIN") || !UsuarioTokenDTO.Rol.Equals("ADMINUSER"));
                        if (!await LoginService.validarAccesoPagina(2, UsuarioTokenDTO) && (!UsuarioTokenDTO.Rol.Equals("ADMIN") || !UsuarioTokenDTO.Rol.Equals("ADMINUSER")))
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

        #region Métodos para dar inicio al proceso operativo a contenedores

        //private async Task GetOrdenesPendientes() {
        //    _cargando = true;
        //    _filtroContenedores.Filtro.IdLNegocio = 1;
        //    _cargando = false;
        //}

        //private async Task GetOrdenesPendientes() {

        //    var filtro = new FiltroOrdenesReferenciasDTO();
        //    filtro.IdCatEstadoContenedor = 7;
        //    filtro.IdCatEstadoReferencia = 7;
        //    filtro.IdCatEstadoOrden = 5;

        //    _cargando = true;

        //    var peticionesReferencias = await ReferenciaService!.GetReferencias(filtro)?? new List<RespObtenerReferenciasDTO>();
        //    var result = peticionesReferencias.AsEnumerable();

        //    _ordenes.Clear();

        //    foreach (var o in peticionesReferencias) {
        //        _ordenes.Add(new Ordenes {
        //            IdOrden = o.IdOrden,
        //            catClientes = new CatClientes { RazonSocial = o.RazonSocialCliente },
        //            FechaRegistro = o.FechaSolicitud,
        //            ReferenciaALO = o.ReferenciaALO,
        //            catAduana = new CatAduana {
        //                IdCatAduana = o.IdCatAduana,
        //                Nombre = o.Aduana
        //            },

        //            catUsuario = new CatUsuarios {
        //                IdCatUsuarios = o.IdUsuario,
        //                Nombre = o.Usuario
        //            },
        //            peticionesReferencias = new List<PeticionesReferencias> {
        //                new PeticionesReferencias {
        //                    Contenedores = o.peticionesContenedores
        //                }
        //            }
        //        });
        //    }
        //    _cargando = false;
        //}
        private void GetOrdenesSeleccionadas(IList<Ordenes> ordenes)
        {
            _ordenesSeleccionadas = ordenes;
        }
        private async Task GetOrdenesFiltradas(List<Ordenes> ordenesFiltradas)
        {
            _ordenesFiltradas = ordenesFiltradas;
            await RecargarTablaAsync();
        }
        private async Task AsignarPatios()
        {
            _isBusyAsignarPatio = true;
            if (_gridContenedoresPendientes != null)
            {
                await _gridContenedoresPendientes.AsignarPatio();
            }
            _isBusyAsignarPatio = false;
        }
        private async Task IniciarProceso()
        {
            _isBusyInciarProceso = true;
            if (_gridContenedoresPendientes != null)
            {
                await _gridContenedoresPendientes.IniciarProceso(_ordenesSeleccionadas);
                //await GetOrdenesPendientes();
                SetFiltro();
                await GetOrdenesAsync();
            }
            _isBusyInciarProceso = false;
        }
        #endregion Métodos para dar inicio al proceso operativo a contenedores

        private async Task OnChange(int index)
        {
            _mostrarFiltros = index == 2;
            _mostrarBotonServicios = _mostrarFiltros;
            _mostrarBotonesInicioProceso = index == 0;
            _indexTab = index;
            //Console.WriteLine($"OnChange => indexTab {_indexTab}");
            SetFiltro();
            await GetOrdenesAsync();
        }

        private async Task MostrarModalServicios()
        {
            var resultado = await DialogService.OpenAsync<ModalServiciosCmp>(
                    title: "Asignación de servicios",
                    options: new DialogOptions
                    {
                        Width = "800px",
                        Height = "600px",
                        CloseDialogOnOverlayClick = false,
                        ShowClose = true // Oculta la equis del modal
                    }
                );


            // Verifica si se devolvió algo y cámbialo a tu tipo
            if (resultado is IList<ModalServiciosCmp.Servicio> serviciosSeleccionados)
            {
                // Aquí puedes trabajar con la lista seleccionada
                foreach (var s in serviciosSeleccionados)
                {
                    foreach (var c in _contenedoresProcesoSeleccionados)
                    {
                        foreach (var cc in _contenedoresProceso)
                        {
                            if (c.Contenedor.Equals(cc.Contenedor))
                            {
                                cc.Servicios.Add(new PeticionesServicios
                                {
                                    catServicios = new CatServicios { Nombre = s.Nombre }
                                });
                            }
                        }

                    }
                }
                await _contenedoresProcesoGrid.Reload();
            }
        }

        private void ActualizarSeleccionados(int total)
        {
            _totalContenedoresPendientesSeleccionados = total;
        }

        private void SetFiltro()
        {
            _filtroContenedores.Filtro.IdLNegocio = 1;
            _filtroContenedores.Filtro.IdEmpresa = new List<string> { "2" };
            _filtroContenedores.Filtro.IdCatEstadoOrden = 0;
            switch (_indexTab)
            {
                case 0:
                    _filtroContenedores.Filtro.IdCatEstadoReferencia = null;
                    _filtroContenedores.Filtro.IdCatEstadoOrden = null;
                    _filtroContenedores.Filtro.IdCatEstadoContenedor = 7;
                    break;
                case 1:
                    //_filtroContenedores.Filtro.IdCatEstadoContenedor = 4;
                    //_filtroContenedores.Filtro.IdCatEstadoReferencia = 4;
                    _filtroContenedores.Filtro.IdCatEstadoContenedor = 4;
                    _filtroContenedores.Filtro.IdCatEstadoReferencia = null;
                    _filtroContenedores.Filtro.IdCatEstadoOrden = 2;
                    break;
            }
        }

        private async Task GetOrdenesAsync()
        {
            await _filtroContenedores.GetOrdenes();
            await RecargarTablaAsync();
        }

        private async Task RecargarTablaAsync()
        {
            switch (_indexTab)
            {
                case 0:
                    _ordenes = new List<Ordenes>(_ordenesFiltradas);
                    await _gridOrdenesCmp.RecargarTabla();
                    StateHasChanged();
                    break;
                case 1:
                    _ordenesProceso = new List<Ordenes>(_ordenesFiltradas);
                    await _gridOrdenesProcesoCmp.RecargarTabla();
                    StateHasChanged();
                    break;
            }
        }

        private async Task CancelarOrden(int idOrden)
        {

            if (idOrden > 0)
            {

                var orden = _ordenes.Where(o => o.IdOrden == idOrden).FirstOrDefault();


                if (_ordenes.Contains(orden))
                {
                    _ordenes.Remove(orden);
                    await _gridOrdenesCmp.RecargarTabla();
                }
            }
        }

        private async Task GetIdOrdenCancelar(int idOrdenCancelar)
        {
            await _gridOrdenesCmp.OnIdOrdenCancelar.InvokeAsync(idOrdenCancelar);
        }

        private async Task RecargarDatosAsync(bool recargarDatos)
        {

            if (recargarDatos)
            {
                SetFiltro();
                await GetOrdenesAsync();
            }

        }
    }
}
