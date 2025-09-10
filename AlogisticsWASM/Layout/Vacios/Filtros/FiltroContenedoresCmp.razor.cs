using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Layout.Vacios.Filtros
{
    public partial class FiltroContenedoresCmp
    {

        private string strPcontenedor;
        private string strPcliente;
        private int intPcliente = 0;
        private int intPNaviera = 0;
        private int intPPatio = 0;
        private string strPrefalo;
        private string strPPatio;
        private string strPrefcliente;
        private DateTime dFSolicitudIni;
        private DateTime dFSolicitudFin;
        private string strPNaviera;
        private string strPAduana;
        private int intPAduana = 0;
        bool isLoading;

        private ICollection<RespListarCoincidenciasDTO> respListarCoincidencias;
        private ICollection<RespuestaGenericaCatalogosDTO> respListarCoincidenciasNavGen;
        private ICollection<RespuestaGenericaCatalogosDTO> respListarCoincidenciasGen;
        private ICollection<CatAduana> lstAduanas = new List<CatAduana>();
        private RespuestaGenericaCatalogosDTO selectedGen;
        private RespListarCoincidenciasDTO selectedCliente;
        private RespuestaGenericaCatalogosDTO selectedGenNaviera;

        private ICollection<RespObtenerReferenciasDTO> _lstReferencias;
        ODataEnumerable<RespObtenerReferenciasDTO> _ODReferencias;
        private IEnumerable<RespObtenerReferenciasDTO> _IEReferencias;
        IEnumerable<PeticionesContenedores> _IEContenedores = new List<PeticionesContenedores>();
        IEnumerable<PeticionesServicios> _IEServicio = new List<PeticionesServicios>();
        // <=================================================================================================>
        private FiltroOrdenesReferenciasDTO _filtro;

        private ICollection<CatAduana> _aduanas;
        private ICollection<CatPatios> _patios;
        private CatAduana _aduana;
        private CatPatios _patio;
        private bool _isBusy;

        public FiltroOrdenesReferenciasDTO Filtro { get; set; }
        [Parameter] public EventCallback<List<Ordenes>> OrdenesFiltradas { get; set; }
        [Inject] private ICatClientesService ClienteService { get; set; }
        [Inject] private ICatNavieraService NavieraService { get; set; }
        [Inject] private ICatPatiosServices PatioServices { get; set; }
        [Inject] public IReferenciaService ReferenciaService { get; set; }
        [Inject] public ICatAduanaService CatAduanaService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Filtro = new FiltroOrdenesReferenciasDTO();
            Filtro.FechaSolicitudIni = DateTime.Now.AddDays(-7);
            Filtro.FechaSolicitudFin = DateTime.Now;
            dFSolicitudIni = (DateTime)Filtro.FechaSolicitudIni;
            dFSolicitudFin = (DateTime)Filtro.FechaSolicitudFin;
            _aduana = new CatAduana();
            _patio = new CatPatios();
            _aduanas = await CatAduanaService.GetAduanas();
            _patios = await PatioServices.GetPatios();
        }

        private void Change(string text)
        {
        }

        private void AutoCompleteOnClienteChange(object value)
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

        private async Task AutoCompleteOnLoadCliente(LoadDataArgs args)
        {
            var strQuery = args.Filter;
            respListarCoincidencias = await ClienteService.GetClientesConincidencia(strQuery);
        }

        private void AutoCompleteOnNavieraChange(object value)
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
            }
        }

        private async Task AutoCompleteOnLoadNaviera(LoadDataArgs args)
        {
            var strQuery = args.Filter;
            respListarCoincidenciasNavGen = await NavieraService.GetNavierasConincidencia(strQuery);
        }

        private void AutoCompleteOnPatioChange(object value)
        {
            intPPatio = 0;
            strPPatio = null;
            selectedGen = respListarCoincidenciasGen.FirstOrDefault(c => c.Nombre == value?.ToString());

            if (selectedGen != null)
            {
                var selectedId = selectedGen.Id;
                intPPatio = selectedId;
                strPPatio = selectedGen.Nombre;
            }
        }

        private async Task AutoCompleteOnLoadPatio(LoadDataArgs args)
        {
            var strQuery = args.Filter;
            if (!string.IsNullOrEmpty(strQuery))
            {
                strQuery = strQuery.ToUpper();
            }
            respListarCoincidenciasGen = await PatioServices.GetPatiosConincidencia(strQuery, 0);
        }

        public async Task GetOrdenes()
        {
            List<Ordenes> ordenes = new List<Ordenes>();

            Filtro.Contenedor = strPcontenedor;
            Filtro.FechaSolicitudIni = dFSolicitudIni;
            Filtro.FechaSolicitudFin = dFSolicitudFin;
            Filtro.ReferenciaALO = strPrefalo;
            Filtro.ReferenciaCliente = strPrefcliente;
            Filtro.IdCliente = intPcliente;
            Filtro.IdPatio = intPPatio;
            Filtro.IdNaviera = intPNaviera;
            Filtro.IdAduana = _aduana.IdCatAduana;
            _isBusy = true;
            var peticionesReferencias = await ReferenciaService!.GetReferencias(Filtro) ?? new List<RespObtenerReferenciasDTO>();
            var result = peticionesReferencias.AsEnumerable();

            foreach (var o in peticionesReferencias)
            {
                o.peticionesContenedores
                     .Where(c => c.catTipoContenedor != null)
                     .ToList()
                     .ForEach(c => c.ClaveTipoContenedor = c.catTipoContenedor.Nomenclatura);

                ordenes.Add(new Ordenes
                {
                    IdOrden = o.IdOrden,
                    catClientes = new CatClientes { RazonSocial = o.RazonSocialCliente },
                    FechaRegistro = o.FechaSolicitud,
                    ReferenciaALO = o.ReferenciaALO,
                    catAduana = new CatAduana
                    {
                        IdCatAduana = o.IdCatAduana,
                        Nombre = o.Aduana
                    },

                    catUsuario = new CatUsuarios
                    {
                        IdCatUsuarios = o.IdUsuario,
                        Nombre = o.Usuario
                    },
                    peticionesReferencias = new List<PeticionesReferencias> {
                        new PeticionesReferencias {
                            IdReferencia = o.IdReferencia,
                            Contenedores = o.peticionesContenedores
                        }
                    }
                });
            }
            _isBusy = false;
            await OrdenesFiltradas.InvokeAsync(ordenes);

        }

        private async Task ObtenerDatos()
        {
            _filtro.IdLNegocio = 1;
            _lstReferencias = new List<RespObtenerReferenciasDTO>();
            _lstReferencias = await ReferenciaService!.GetReferencias(_filtro);


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

        private async Task EnviarDatos()
        {
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
            _filtro.IdAduana = _aduana.IdCatAduana;

            await ObtenerDatos();
            await GetOrdenes();
            isLoading = false;
        }
    }
}
