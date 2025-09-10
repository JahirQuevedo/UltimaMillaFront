using Microsoft.AspNetCore.Components;

namespace AlogisticsWASM.Layout.Generics
{
    public partial class ListadoGenerico<TItem>
    {

        // Lista de elementos genéricos
        [Parameter] public ICollection<TItem> Listado { get; set; } = new List<TItem>();
        // Funciones para obtener Id y RazonSocial dinámicamente
        [Parameter] public Func<TItem, int> IdSelector { get; set; }
        [Parameter] public Func<TItem, string> DisplaySelector { get; set; }
        // Parámetro para mantener el valor seleccionado
        [Parameter] public int? SelectedId { get; set; }
        [Parameter][EditorRequired] public string Etiqueta { get; set; } = string.Empty;
        // Parámetro para identificar si se requiere habilitar o deshabilitar el componente
        [Parameter][EditorRequired] public bool HabilitarComponente { get; set; } = false;

        private string SelectorEtiqueta = string.Empty;

        private async Task HandleSelection(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int selectedId))
            {
                SelectedId = selectedId;
                var selectedItem = Listado.FirstOrDefault(item => IdSelector(item) == selectedId);
                if (selectedItem != null)
                {
                    await OnSelected.InvokeAsync(selectedItem);
                }
            }
        }

        public string GetEtiqueta()
        {
            return Etiqueta;
        }

        public string GetSelectorEtiqueta()
        {
            return "floating" + SelectorEtiqueta;
        }

        // Método para obtener el Id del item
        private int GetIdValue(TItem item) => IdSelector(item);

        // Método para obtener el valor de RazonSocial del item
        private string GetDisplayValue(TItem item) => DisplaySelector(item);

        // Evento para devolver el objeto seleccionado al componente padre
        [Parameter] public EventCallback<TItem> OnSelected { get; set; }

    }
}


