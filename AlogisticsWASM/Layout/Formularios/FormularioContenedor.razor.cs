

using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.IServices;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AlogisticsWASM.Layout.Formularios
{
    public partial class FormularioContenedor
    {

        [Parameter][EditorRequired] public PeticionesContenedores Contenedor { get; set; }
        [Inject] private IContenedorService contenedorService { get; set; }
        [Inject] private ICatClientesService clienteService { get; set; }
        [Inject] private ICatNavieraService navieraService { get; set; }
        [Inject] private IPatiosService patioService { get; set; }

        public PeticionesContenedores _contenedor { get; set; } = new PeticionesContenedores();
        private ICollection<CatNavieras> _navieras;
        private ICollection<CatClientes> _clientes;
        private ICollection<CatPatios> _patios;
        private EditContext _editContext;
        protected override async Task OnInitializedAsync()
        {
            Contenedor = new PeticionesContenedores();
            _editContext = new EditContext(Contenedor);
            _navieras = await navieraService!.GetNavieras();
            _clientes = await clienteService!.GetClientes();
            _patios = patioService!.GetPatios();
        }

        private void ObtenerNavieraSeleccionada(CatNavieras naviera)
        {
            Contenedor!.Naviera_RazonSocial = naviera.RazonSocial;
            Contenedor!.Naviera_Id = naviera.IdCatNaviera;
            Contenedor!.Naviera_RFC = naviera.RFC;
        }

        private void ObtenerClienteSeleccionado(CatClientes cliente)
        {
            Contenedor!.Cliente_RazonSocial = cliente.RazonSocial;
            Contenedor!.Cliente_Solicitante = cliente.RazonSocial;
            Contenedor!.ClienteId = cliente.IdCatCliente;
            Contenedor!.Cliente_RFC = cliente.RFC;
        }

        private void ObtenerTipoContenedorSeleccionado(CatTipoContenedor tipoContenedor)
        {
            //Contenedor!.ClaveTipoContenedor = tipoContenedor.Acronimo;
        }

        private void ObtenerPatioSeleccionado(CatPatios patio)
        {
            Contenedor!.Patio_RazonSocial = patio.RazonSocial;
            Contenedor!.PatioId = patio.IdCatPatios;
        }

        public bool DatosValidos()
        {
            if (_editContext != null && _editContext.Validate())
            {
                return true;
            }
            return false;
        }
    }
}
