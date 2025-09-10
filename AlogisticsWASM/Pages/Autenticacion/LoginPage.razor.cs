

using ALOG.Modelos.Modelos.DTO.Control;
using ALOGRepositorios.Services.Login.ILogin;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Web;

namespace AlogisticsWASM.Pages.Autenticacion
{
    public partial class LoginPage
    {

        private SistemaLoginDTO usuarioAutenticacion { get; set; } = new SistemaLoginDTO();
        [Inject] private ILoginService _loginService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }

        private SistemaLoginRespuestaDTO sistemaLoginRespuestaDTO;
        public bool EstaProcesando { get; set; } = false;
        public bool MostrarErroresAutenticacion { get; set; }

        public string UrlRetorno { get; set; }
        private string userName;
        private string password;
        private bool rememberMe = true;
        public string Errores { get; set; }
        private async Task OnLogin(LoginArgs args, string name)
        {
            if (!string.IsNullOrEmpty(args.Username) && !string.IsNullOrEmpty(@args.Password))
            {
                usuarioAutenticacion = new SistemaLoginDTO();
                usuarioAutenticacion.sUsuario = args.Username;
                usuarioAutenticacion.sPass = args.Password;
                MostrarErroresAutenticacion = false;
                EstaProcesando = true;
                var result = await _loginService.Acceder(usuarioAutenticacion);
                if (result.Exito)
                {
                    EstaProcesando = false;
                    var urlAbsoluta = new Uri(_navigationManager.Uri);
                    var parametrosQuery = HttpUtility.ParseQueryString(urlAbsoluta.Query);
                    UrlRetorno = parametrosQuery["returnUrl"];
                    if (string.IsNullOrEmpty(UrlRetorno))
                    {
                        _navigationManager.NavigateTo("/");
                    }
                    else
                    {
                        _navigationManager.NavigateTo("/" + UrlRetorno);
                    }

                }
                else
                {
                    EstaProcesando = false;
                    MostrarErroresAutenticacion = true;
                    Errores = "Usuario y/o contraseña son incorrectos";
                    _navigationManager.NavigateTo("/login");
                }
            }
            else
            {
                MostrarErroresAutenticacion = true;
                Errores = "Usuario y/o contraseña están vacíos";
            }
        }

        //void OnLogin(LoginArgs args, string name)
        //{
        //    //Console.WriteLine($"{name} -> Username: {args.Username}, password: {args.Password}, remember me: {args.RememberMe}");
        //}

        void OnRegister(string name)
        {
            ////Console.WriteLine($"{name} -> Register");
        }

        void OnResetPassword(string value, string name)
        {
            ////Console.WriteLine($"{name} -> ResetPassword for user: {value}");
        }
    }
}
