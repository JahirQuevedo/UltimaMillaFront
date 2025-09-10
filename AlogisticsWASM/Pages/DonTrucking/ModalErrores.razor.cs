using Microsoft.AspNetCore.Components;

namespace AlogisticsWASM.Pages.DonTrucking
{
    public partial class ModalErrores
    {
        [Parameter] public List<string> Errores { get; set; } = new List<string>();

        protected override void OnInitialized()
        {

        }

        protected override void OnParametersSet()
        {

        }

        private void LimpiarLista()
        {
            Errores.Clear();
        }


    }
}
