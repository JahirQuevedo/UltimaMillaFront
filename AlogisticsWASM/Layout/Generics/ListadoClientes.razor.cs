
using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;

namespace AlogisticsWASM.Layout.Generics
{
    public partial class ListadoClientes
    {

        [Inject] private ICatClientesService clienteService { get; set; } = null;
        private IEnumerable<CatClientes> clientes = new List<CatClientes>();
        private CatClientes ClienteSeleccionado { get; set; } = new CatClientes();
        [Parameter] public int? IdClienteSeleccionado { get; set; }
        [Parameter] public EventCallback<CatClientes> onClienteSeleccionado { get; set; }

        protected override async Task OnInitializedAsync()
        {
            clientes = await clienteService.GetClientes();
        }

        public async Task ObtenerClienteSeleccionado(CatClientes cliente)
        {
            ClienteSeleccionado = clientes.FirstOrDefault(c => c.IdCatCliente == cliente.IdCatCliente)!;
            await onClienteSeleccionado.InvokeAsync(ClienteSeleccionado);
        }
    }
}
