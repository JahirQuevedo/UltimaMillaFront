using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AlogisticsWASM.Pages
{
    public partial class RedireccionAlAcceso
    {

        [Inject]
        private NavigationManager navigationManager { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> estadoProveedorAutenticacion { get; set; }
        bool noAutorizado { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var estadoAutorizacion = await estadoProveedorAutenticacion;

            if (estadoAutorizacion.User == null)
            {
                var returnUrl = navigationManager.ToBaseRelativePath(navigationManager.Uri);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    navigationManager.NavigateTo("Acceder", true);
                }
                else
                {
                    navigationManager.NavigateTo($"Acceder?returnUrl={returnUrl}", true);
                }
            }
            else
            {
                noAutorizado = true;
            }
        }
    }
}
