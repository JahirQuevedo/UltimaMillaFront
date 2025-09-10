using ALOG.Modelos.Modelos.DTLogistico;

using Microsoft.AspNetCore.Components;

namespace AlogisticsWASM.Pages.DonTrucking
{
    public partial class ModalDT
    {

        [Parameter][EditorRequired] public ICollection<DtUltimaMillaDet> detalles { get; set; }

        protected override void OnInitialized()
        {
            detalles = new List<DtUltimaMillaDet>();
        }

        protected override void OnParametersSet()
        {

        }
    }
}
