using ALOGRepositorios.Services.Login.ILogin;
using Microsoft.AspNetCore.Components;

namespace AlogisticsWASM.Pages.Autenticacion
{
    public partial class SalirPage : ComponentBase
    {
        [Inject]
        public ILoginService servicioAutenticacion { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await servicioAutenticacion.Salir();
            navigationManager.NavigateTo("/login");
        }
    }
}
