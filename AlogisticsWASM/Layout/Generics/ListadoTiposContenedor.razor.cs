using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;

namespace AlogisticsWASM.Layout.Generics
{
    public partial class ListadoTiposContenedor
    {

        [Inject] private ICatTipoContenedorService tipoContenedorService { get; set; } = null!;
        private ICollection<CatTipoContenedor> tiposContenedor { get; set; } = new List<CatTipoContenedor>();
        [Parameter] public EventCallback<CatTipoContenedor> OnTipoContenedorSeleccionado { get; set; }
        public CatTipoContenedor TipoContenedorSeleccionado { get; set; } = new CatTipoContenedor();

        protected override void OnInitialized()
        {
            //tiposContenedor = tipoContenedorService.GetTiposContenedor();
        }

        private async Task ObtenerTipoContenedorSeleccionado(CatTipoContenedor tipoContenedor)
        {
            //TipoContenedorSeleccionado = tiposContenedor.FirstOrDefault(t => t.Id == tipoContenedor.Id)!;
            //await OnTipoContenedorSeleccionado.InvokeAsync(tipoContenedor);
        }

    }
}
