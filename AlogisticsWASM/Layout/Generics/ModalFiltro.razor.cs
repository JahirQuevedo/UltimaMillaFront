using AlogisticsWASM.Layout.FiltrosBusqueda;

using Microsoft.AspNetCore.Components;

namespace AlogisticsWASM.Layout.Generics
{
    public partial class ModalFiltro<TModel> : ComponentBase
    {
        [Parameter] public EventCallback<Dictionary<string, object>> OnFiltrosAplicados { get; set; }
        private Dictionary<string, object> FilterCriteria = new Dictionary<string, object>();
        // Referencia al componente hijo
        private BusquedaReferencias? busquedaReferencia;
        private BusquedaContenedores? busquedaContenedores;

        private void ApplyFilters(Dictionary<string, object> filtros)
        {
            FilterCriteria = filtros;
        }

        private async Task AplicarFiltros()
        {
            // Verifica si la referencia al componente hijo es válida
            if (busquedaReferencia != null)
            {
                // Obtiene los filtros del componente hijo
                FilterCriteria = busquedaReferencia.GetFiltros();
            }
            else if (busquedaContenedores != null)
            {
                FilterCriteria = busquedaContenedores.GetFiltros();
            }

            await OnFiltrosAplicados.InvokeAsync(FilterCriteria);
        }

        private void ReiniciarFiltros()
        {
            FilterCriteria.Clear();

            if (busquedaReferencia != null)
            {
                // Obtiene los filtros del componente hijo
                busquedaReferencia.SetFiltros(FilterCriteria);
            }
            else if (busquedaContenedores != null)
            {
                busquedaContenedores.SetFiltros(FilterCriteria);
            }
        }
    }
}
