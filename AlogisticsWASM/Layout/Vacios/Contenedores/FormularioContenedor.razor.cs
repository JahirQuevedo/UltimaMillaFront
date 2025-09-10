using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.IServices;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using Microsoft.AspNetCore.Components;


namespace AlogisticsWASM.Layout.Vacios.Contenedores
{
    public partial class FormularioContenedor
    {
        [Parameter]
        [EditorRequired]
        public PeticionesContenedores Contenedor { get; set; } = new PeticionesContenedores();
        [Parameter]
        [EditorRequired]
        public int IdReferencia { get; set; } = 0;
        [Inject] private IPatiosService patioService { get; set; }
        [Inject] private ICatClientesService clienteService { get; set; }
        [Inject] private ICatNavieraService navieraService { get; set; }
        [Inject] private IContenedorService contenedorService { get; set; }
        [Parameter] public EventCallback<PeticionesContenedores> OnContenedor { get; set; }
        private string TextBoton { get; set; } = string.Empty!;
        private ICollection<CatPatios> patios = new List<CatPatios>();
        private ICollection<CatNavieras> navieras = new List<CatNavieras>();
        private ICollection<CatClientes> clientes = new List<CatClientes>();
        private string TituloFormulario { get; set; } = string.Empty!;
        private CatNavieras Naviera { get; set; } = new CatNavieras();
        private CatClientes Cliente { get; set; } = new CatClientes();
        private CatTipoContenedor TipoContenedor { get; set; } = new CatTipoContenedor();

        protected override async Task OnInitializedAsync()
        {

            patios = patioService.GetPatios();
            navieras = await navieraService.GetNavieras();
            clientes = await clienteService.GetClientes();
        }

        protected override void OnParametersSet()
        {
            TextBoton = Contenedor.IdContenedor == 0 ? "Crear contenedor" : "Editar contenedor";
            TituloFormulario = Contenedor.IdContenedor == 0 ? "Creando contenedor" : "Editando contenedor";
            InvokeAsync(StateHasChanged);
        }

        private async Task CrearContenedor()
        {

            ////Console.WriteLine($"contenedor a crear {JsonConvert.SerializeObject(Contenedor, Formatting.Indented)}");

            if (Contenedor != null)
            {
                ////Console.WriteLine($"IdReferencia {IdReferencia}");
                ////Console.WriteLine($"Contenedor a punto de crear {JsonConvert.SerializeObject(Contenedor, Formatting.Indented)}");
                // Lo correcto sería que se regrese el objecto con su respectivo ID asignado en la base de datos
                await contenedorService.CrearContenedor(Contenedor, IdReferencia);
                //////Console.WriteLine($"");
                await OnContenedor.InvokeAsync(Contenedor);
            }

        }

        private void ObtenerNavieraSeleccionada(CatNavieras naviera)
        {
            ////Console.WriteLine($"Naviera seleccionado {JsonConvert.SerializeObject(naviera)}");
            Contenedor.Naviera_Id = naviera.IdCatNaviera;
            Contenedor.Naviera_RFC = naviera.RFC;
            Contenedor.Naviera_RazonSocial = naviera.RazonSocial;

        }

        private void ObtenerClienteSeleccionado(CatClientes cliente)
        {
            ////Console.WriteLine($"Cliente seleccionado {JsonConvert.SerializeObject(cliente)}");
            Contenedor.ClienteId = cliente.IdCatCliente;
            Contenedor.Cliente_RFC = cliente.RFC;
            Contenedor.Cliente_RazonSocial = cliente.RazonSocial;
            Contenedor.Cliente_Solicitante = cliente.RazonSocial;
        }

        private void ObtenerTipoContenedorSeleccionado(CatTipoContenedor tipoContenedor)
        {
            ////Console.WriteLine($"Tipo de contenedor RECIBIDO {JsonConvert.SerializeObject(tipoContenedor, Formatting.Indented)}");
            Contenedor.ClaveTipoContenedor = tipoContenedor.Nomenclatura;
        }

        private void ObtenerPatioSeleccionado(CatPatios patio)
        {
            Contenedor.PatioId = patio.IdCatPatios;
            Contenedor.Patio_RazonSocial = patio.RazonSocial;
        }

        private async Task CancelarAccion()
        {
            await OnContenedor.InvokeAsync(new PeticionesContenedores());
        }

    }
}
