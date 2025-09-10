using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Layout.Utilerias
{
    public partial class AgregarComentariosCmp
    {

        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private string _comentarios;


        private bool ComentarioEsValido()
        {

            if (string.IsNullOrEmpty(_comentarios))
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Debes indicar un comentario",
                    Detail = ""
                });
                return false;
            }
            return true;
        }

        private void Aceptar()
        {
            if (ComentarioEsValido())
                DialogService.Close(_comentarios);
        }

        private void OnChange(string value, string name)
        {

        }
    }
}
