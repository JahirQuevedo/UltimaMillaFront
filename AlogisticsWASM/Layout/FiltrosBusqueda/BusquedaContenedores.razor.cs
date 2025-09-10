using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace AlogisticsWASM.Layout.FiltrosBusqueda
{
    public partial class BusquedaContenedores
    {
        private string ReferenciaCliente { get; set; } = string.Empty;
        private int IdCliente { get; set; } = 0;
        private int IdNaviera { get; set; } = 0;
        private int IdPatio { get; set; } = 0;
        [Inject] private ICatClientesService clientesService { get; set; }
        [Inject] private ICatNavieraService navieraService { get; set; }
        [Inject] private IPatiosService patiosService { get; set; }
        private IEnumerable<CatClientes> clientes { get; set; } = new List<CatClientes>();
        private IEnumerable<CatNavieras> navieras { get; set; } = new List<CatNavieras>();
        private ICollection<CatPatios> patios { get; set; } = new List<CatPatios>();

        protected override async Task OnInitializedAsync()
        {
            clientes = await clientesService.GetClientes();
            navieras = await navieraService.GetNavieras();
            patios = patiosService.GetPatios();
        }

        public Dictionary<string, object> GetFiltros()
        {
            return new Dictionary<string, object>{
                { "ReferenciaCliente", ReferenciaCliente },
                { "IdCliente", IdCliente },
                { "IdNaviera", IdNaviera },
                { "IdPatio", IdPatio }
             };
        }


        public void SetFiltros(Dictionary<string, object> filtros)
        {
            if (filtros.Count() == 0)
            {
                ReferenciaCliente = string.Empty;
                IdCliente = 0;
                IdNaviera = 0;
                IdPatio = 0;
                InvokeAsync(StateHasChanged);
            }
        }

        public Task SetReferenciaCliente(string referenciaCliente)
        {

            ReferenciaCliente = referenciaCliente.ToUpper();

            return Task.CompletedTask;
        }

    }
}
