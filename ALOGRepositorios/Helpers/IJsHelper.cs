using Microsoft.JSInterop;
using Radzen;

namespace ALOGRepositorios.Helpers
{
    public static class IJsHelper
    {
        public static async ValueTask ToastrSuccess(this IJSRuntime JSRuntime, string message)
        {
            await JSRuntime.InvokeVoidAsync("ShowToastr", "success", message);
        }

        public static async ValueTask ToastrError(this IJSRuntime JSRuntime, string message)
        {
            await JSRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }

        public static async Task AbrirDocumentoEnNuevaPestana(this IJSRuntime JSRuntime, string? base64)
        {
            if (!string.IsNullOrWhiteSpace(base64))
            {
                await JSRuntime.InvokeVoidAsync("abrirPdfBase64", base64);
            }
        }

        public static void MostrarNotificacion(
                                           this NotificationService notificationService,
                                           string titulo,
                                           string mensaje,
                                           NotificationSeverity severity = NotificationSeverity.Info,
                                           int duration = 4000)
        {
            notificationService.Notify(new NotificationMessage
            {
                Severity = severity,
                Summary = titulo,
                Detail = mensaje,
                Duration = duration <= 0 ? 4000 : duration
            });
        }
    }
}
