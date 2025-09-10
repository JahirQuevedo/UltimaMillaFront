using ALOGRepositorios.Services.Autenticacion;
using Blazored.LocalStorage;
using ClienteBlazorWASM.Helpers;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ALOGRepositorios.Services.Login
{
    public class UserService
    {

        // private readonly HttpClient _httpClient;

        private readonly IJSRuntime _jsRuntime;

        private readonly ILocalStorageService _localStorageService;
        private readonly AuthStateProvider _authenticationStateProvider;

        //public UsuarioTokenDTO? usuarioTokenDTO { get; private set; }
        public event Action? OnUserChanged;
        //ILocalStorageService localStorageService,
        //HttpClient httpClient,
        public UserService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            //_httpClient = httpClient;
            //_jsRuntime = jsRuntime;
        }

        //public async Task InitializeAsync()
        //{
        //    await LoadUserData();
        //}

        //public async Task LoadUserDataLS()
        //{
        //    #region ValidarAcceso
        //    try
        //    {

        //        if (usuarioTokenDTO.catUsuarios is null)
        //        {
        //            ////Console.WriteLine($"ENTRO APP VACIO");
        //            usuarioTokenDTO = await _loginService.ObtenerdatosToken();
        //            ////Console.WriteLine($"SALIO APP {usuarioTokenDTO.catUsuarios.Nombre}");
        //            OnUserChanged?.Invoke();
        //        }
        //        else
        //        {
        //            ////Console.WriteLine($"INIT APP {usuarioTokenDTO?.Email}");
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        ////Console.WriteLine("APP.RAZOR:" + ex.Message);
        //    }

        //    #endregion ValidarAcceso
        //}
        //public async Task LoadUserData()
        //{
        //    var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "JWT Token");
        //    ////Console.WriteLine("Userservice:" + token);
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        try
        //        {
        //            string claveSecreta = Inicializar.ClaveSecreta;

        //            var key = Encoding.ASCII.GetBytes(claveSecreta);

        //            var handler = new JwtSecurityTokenHandler();
        //            var validations = new TokenValidationParameters
        //            {
        //                ValidateIssuerSigningKey = true,
        //                IssuerSigningKey = new SymmetricSecurityKey(key),
        //                ValidateIssuer = false,
        //                ValidateAudience = false,
        //                RoleClaimType = ClaimTypes.Role // Asegúrate de que esto esté configurado 
        //            };
        //            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
        //            usuarioTokenDTO.IdCatUsuario = int.Parse(claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        //            usuarioTokenDTO.Email = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
        //            usuarioTokenDTO.Rol = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        //            usuarioTokenDTO.Nombre = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

        //            /*CARGAMOS USUARIO*/
        //            SolDatosUsuariosDTO solDatosUsuariosDTO = new SolDatosUsuariosDTO();
        //            solDatosUsuariosDTO.IdCatUsuario = (int)usuarioTokenDTO.IdCatUsuario;
        //            solDatosUsuariosDTO.Activo = true;
        //            var catUsuario = await _loginService.ObtenerCatUsuarios(solDatosUsuariosDTO);
        //            usuarioTokenDTO.catUsuarios = catUsuario;


        //            OnUserChanged?.Invoke();
        //        }
        //        catch (Exception ex)
        //        {
        //            ////Console.WriteLine("Userservice:" + ex.Message);
        //            usuarioTokenDTO = null;
        //        }
        //    }
        //}

        public async Task<string> GetTokenAsync()
        {
            try
            {
                return await _localStorageService.GetItemAsync<string>(Inicializar.Token_Local);
            }
            catch (Exception ex)
            {

                ////Console.WriteLine("GetTokenAsync:" + ex.Message);
                return null;
            }


        }

        public async Task<string> GetUserAsync()
        {
            return await _localStorageService.GetItemAsync<string>(Inicializar.Datos_Usuario_Local);

        }
        public async Task<IEnumerable<Claim>> GetUserClaimsAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return Enumerable.Empty<Claim>();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims;
        }

        //public void ClearUserData()
        //{
        //    usuarioTokenDTO = null;
        //    OnUserChanged?.Invoke();
        //}

    }
}
