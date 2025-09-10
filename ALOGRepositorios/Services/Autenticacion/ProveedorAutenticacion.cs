using ALOGModelos.Modelos;
using ALOGModelos.Modelos.Autenticacion;
using ALOGModelos.Modelos.Autenticacion.Registro;
using ALOGModelos.Modelos.Autenticacion.TokenUsuario;
using ALOGModelos.Modelos.Dtos;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services.Login;
using ALOGRepositorios.Services.Login.ILogin;
//using Blazored.LocalStorage;
using ClienteBlazorWASM.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;


namespace Alogist.Services.Autenticacion
{
    public class ProveedorAutenticacion : AuthenticationStateProvider
    {
        [Inject] private ILoginService _loginService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        // Crea un Claim anonimo
        private AuthenticationState Anonimo => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        public static readonly string TOKEN_KEY = "TOKEN_KEY";
        private readonly HttpClient _httpClient;
        //private readonly ILocalStorageService _localStorageService;
        private readonly IJSRuntime _js;
        private readonly AuthenticationStateProvider _estadoProveedorAutenticacion;


        public ProveedorAutenticacion(IJSRuntime js, HttpClient httpClient,
            // ILocalStorageService localStorageService, 
            AuthenticationStateProvider estadoProveedorAutenticacion)
        {
            _httpClient = httpClient;
            _js = js;
            //_localStorageService = localStorageService;
            _estadoProveedorAutenticacion = estadoProveedorAutenticacion;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var token = await _js.ObtenerDeLocalStorage(TOKEN_KEY);
            //var token = await _localStorageService.GetItemAsync<string>(Inicializar.Token_Local);


            if (token is null)
            {
                return Anonimo;
            }
            return ConstruirAuthenticationState(token.ToString());
            //var token = await _localStorageService.GetItemAsync<string>(Inicializar.Token_Local);
            //if (token == null)
            //{
            //    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            //}
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            //return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));

        }

        private AuthenticationState ConstruirAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var claims = ParsearClaimDelJwt(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        private IEnumerable<Claim> ParsearClaimDelJwt(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenDeserializado = jwtSecurityTokenHandler.ReadJwtToken(token);
            return tokenDeserializado.Claims;
        }

        public async Task Login(string token)
        {
            await _js.GuardarEnLocalStorage(TOKEN_KEY, token);
            var authState = ConstruirAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task LogOut()
        {
            await _js.RemoverDelLocalStorage(TOKEN_KEY);
            // Remueve el token del HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }

        public async Task<SistemaLoginRespuestaDTO> Acceder(SistemasLoginDTO pSistemasLoginDTO)
        {
            var jsonObject = JsonConvert.SerializeObject(pSistemasLoginDTO);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            string url = $"{ClienteBlazorWASM.Helpers.Inicializar.UrlApi}APIControl/Control/loginUser";
            Console.WriteLine(url);
            var response = await _httpClient.PostAsync(url, content);
            Console.WriteLine($"response {response}");
            var responseTemp = await response.Content.ReadAsStringAsync();
            //var jsonResponse = (JObject)JsonConvert.DeserializeObject(responseTemp);

            if (response.IsSuccessStatusCode)
            {
                //var contentToken = await response.Content.ReadAsStringAsync();
                var resultToken = JsonConvert.DeserializeObject<SistemaLoginRespuestaDTO>(responseTemp);
                //var Token = jsonResponse["result"]["token"].Value<string>();
                //var Token = jsonResponse["result"]["token"].Value<string>();
                if (!string.IsNullOrEmpty(resultToken.Token))
                {
                    //await _localStorageService.SetItemAsync(Inicializar.Token_Local, resultToken.Token);
                    //await _localStorageService.SetItemAsync(Inicializar.Datos_Usuario_Local, resultToken.Usuario);
                    //NotificarUsuarioLogueado(resultToken.Token);
                    //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", resultToken.Token);
                    //return resultToken;
                    await _loginService.Login(resultToken.Token);
                    await _js.GuardarEnLocalStorage("usuario", resultToken.Usuario);
                    //.NavigateTo("/");
                    return resultToken;
                }
                else
                {
                    return new SistemaLoginRespuestaDTO { Exito = false };
                }


            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var errorModel = System.Text.Json.JsonSerializer.Deserialize<ErrorResponse>(contentTemp);
                //throw new Exception(errorModel.ErrorMessage);
                return new SistemaLoginRespuestaDTO { Exito = false };
            }
        }


        public void NotificarUsuarioLogueado(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotificarUsuarioSalir()
        {
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }

        public Task<RespuestaRegistro> ResgistraUsuario(UsuarioRegistro pusuarioRegistro)
        {
            throw new NotImplementedException();
        }

        public Task Salir()
        {
            throw new NotImplementedException();
        }
    }
}
