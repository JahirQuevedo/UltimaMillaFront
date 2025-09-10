using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Control;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOGRepositorios.Services.Autenticacion;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRespositorios.Modelos.Dtos.Control;
using Blazored.LocalStorage;
using ClienteBlazorWASM.Helpers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ALOGRepositorios.Services.Login
{



    public class LoginService : ILoginService
    {

        private readonly HttpClient _httpClient;


        private readonly ILocalStorageService _localStorageService;
        private readonly AuthStateProvider _authenticationStateProvider;


        public LoginService(HttpClient httpClient, ILocalStorageService localStorageService,
            AuthStateProvider authenticationStateProvider


            )
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;

            _authenticationStateProvider = authenticationStateProvider;



        }

        public async Task<SistemaLoginRespuestaDTO> Acceder(SistemaLoginDTO pSistemasLoginDTO)
        {
            SistemaLoginRespuestaDTO sistemaLoginRespuestaDTO = new SistemaLoginRespuestaDTO();
            var jsonObject = JsonConvert.SerializeObject(pSistemasLoginDTO);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            string url = $"{ClienteBlazorWASM.Helpers.Inicializar.UrlApiControl}Control/loginUser";
            ////Console.WriteLine(url);
            ////Console.WriteLine(jsonObject);
            var response = await _httpClient.PostAsync(url, content);
            ////Console.WriteLine($"response {response}");
            var responseTemp = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(responseTemp);
            var jsonResponse2 = JsonConvert.DeserializeObject<SistemaLoginRespuestaDTO>(responseTemp);
            ////Console.WriteLine(jsonResponse);


            if (jsonResponse2.Exito)
            {

                var Token = jsonResponse2.Token;//jsonResponse["result"]?["token"]?.Value<string>();
                var Usuario = jsonResponse2.Usuario;//jsonResponse["result"]?["usuario"]?.Value<string>();
                var Error = jsonResponse2.Error;//jsonResponse["result"]?["error"]?.Value<string>();
                var statuscode = jsonResponse2.StatusCode;//jsonResponse["result"]?["statuscode"]?.Value<string>();
                ////Console.WriteLine("Token:" + Token);
                ////Console.WriteLine("Usuario:" + Usuario);
                ////Console.WriteLine("Error:" + Error);
                ////Console.WriteLine("statuscode:" + statuscode);
                if (!string.IsNullOrEmpty(Token))
                {

                    await _localStorageService.SetItemAsync(Inicializar.Token_Local, Token);
                    await _localStorageService.SetItemAsync(Inicializar.Datos_Usuario_Local, Usuario);
                    ((AuthStateProvider)_authenticationStateProvider).NotificarUsuarioLogueado(Token);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);

                    sistemaLoginRespuestaDTO.Exito = true;
                    sistemaLoginRespuestaDTO.Error = Error;
                    sistemaLoginRespuestaDTO.Token = Token;
                    sistemaLoginRespuestaDTO.Usuario = Usuario;


                    return sistemaLoginRespuestaDTO;
                }
                else
                {
                    return new SistemaLoginRespuestaDTO { Exito = false };
                }


            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var errorModel = System.Text.Json.JsonSerializer.Deserialize<ErrorResponseDTO>(contentTemp);
                //throw new Exception(errorModel.ErrorMessage);
                return new SistemaLoginRespuestaDTO { Exito = false };
            }
        }

        public Task Login(string token)
        {
            throw new NotImplementedException();
        }

        public Task LogOut()
        {
            throw new NotImplementedException();
        }

        public async Task<RespuestaRegistroUsuarioDTO> ResgistraUsuario(UsuarioRegistroDTO pusuarioRegistro)
        {
            var content = JsonConvert.SerializeObject(pusuarioRegistro);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiControl}usuarios/registro", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<RespuestaRegistroUsuarioDTO>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                return new RespuestaRegistroUsuarioDTO { registroCorrecto = true };
            }
            else
            {
                return resultado;
            }
        }

        public async Task Salir()
        {
            await _localStorageService.RemoveItemAsync(Inicializar.Token_Local);
            await _localStorageService.RemoveItemAsync(Inicializar.Datos_Usuario_Local);
            ((AuthStateProvider)_authenticationStateProvider).NotificarUsuarioSalir();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }


        public async Task<UsuarioTokenDTO> ObtenerdatosToken()
        {
            string token;
            string usuario;
            UsuarioTokenDTO _usuarioTokenDTO = new UsuarioTokenDTO();

            _usuarioTokenDTO.IdCatUsuario = 0;
            try
            {


                token = await GetTokenAsync();

                usuario = await GetUserAsync();

                string claveSecreta = Inicializar.ClaveSecreta;

                var key = Encoding.ASCII.GetBytes(claveSecreta);

                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RoleClaimType = ClaimTypes.Role // Asegúrate de que esto esté configurado 
                };
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);

                _usuarioTokenDTO.IdCatUsuario = int.Parse(claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                _usuarioTokenDTO.Email = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                _usuarioTokenDTO.Rol = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                _usuarioTokenDTO.Nombre = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                /*CARGAMOS USUARIO*/
                SolDatosUsuariosDTO solDatosUsuariosDTO = new SolDatosUsuariosDTO();
                solDatosUsuariosDTO.IdCatUsuario = (int)_usuarioTokenDTO.IdCatUsuario;
                solDatosUsuariosDTO.Activo = true;

                var catUsuario = await ObtenerCatUsuarios(solDatosUsuariosDTO);
                _usuarioTokenDTO.catUsuarios = catUsuario;

                return _usuarioTokenDTO;
            }
            catch (Exception ex)
            {
                ////Console.WriteLine("ObtenerdatosToken:" + ex.Message);
                return null;
            }
        }



        public async Task<CatUsuarios> ObtenerCatUsuarios(SolDatosUsuariosDTO pusuarioRegistro)
        {
            var content = JsonConvert.SerializeObject(pusuarioRegistro);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiControl}Control/ObtenerCatUsuarios", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<CatUsuarios>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                return resultado;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> validarAccesoPagina(int pIdPermiso, UsuarioTokenDTO pusuarioTokenDTO)
        {
            try
            {
                var permisos = pusuarioTokenDTO.catUsuarios.catUsuariosPermisos.Any(x => x.IdCatPermisos.Equals(pIdPermiso));
                if (permisos)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Dictionary<string, bool>> validarControlesPagina(int pIdPermiso, UsuarioTokenDTO pusuarioTokenDTO)
        {

            Dictionary<string, bool> diccionario = new Dictionary<string, bool>();
            try
            {

                if (pusuarioTokenDTO.catUsuarios.catUsuariosPermisos.Any(x => x.IdCatPermisos.Equals(pIdPermiso) && x.Activo == true && x.Actualizar == true))
                    diccionario.Add("ACTUALIZAR", true);
                else
                    diccionario.Add("ACTUALIZAR", false);

                if (pusuarioTokenDTO.catUsuarios.catUsuariosPermisos.Any(x => x.IdCatPermisos.Equals(pIdPermiso) && x.Activo == true && x.Eliminar == true))
                    diccionario.Add("ELIMINAR", true);
                else
                    diccionario.Add("ELIMINAR", false);

                if (pusuarioTokenDTO.catUsuarios.catUsuariosPermisos.Any(x => x.IdCatPermisos.Equals(pIdPermiso) && x.Activo == true && x.Crear == true))
                    diccionario.Add("CREAR", true);
                else
                    diccionario.Add("CREAR", false);

                if (pusuarioTokenDTO.catUsuarios.catUsuariosPermisos.Any(x => x.IdCatPermisos.Equals(pIdPermiso) && x.Activo == true && x.Autorizar == true))
                    diccionario.Add("AUTORIZAR", true);
                else
                    diccionario.Add("AUTORIZAR", false);

                if (pusuarioTokenDTO.catUsuarios.catUsuariosPermisos.Any(x => x.IdCatPermisos.Equals(pIdPermiso) && x.Activo == true && x.Enviar == true))
                    diccionario.Add("ENVIAR", true);
                else
                    diccionario.Add("ENVIAR", false);

                if (pusuarioTokenDTO.catUsuarios.catUsuariosPermisos.Any(x => x.IdCatPermisos.Equals(pIdPermiso) && x.Activo == true && x.Exportar == true))
                    diccionario.Add("EXPORTAR", true);
                else
                    diccionario.Add("EXPORTAR", false);

                if (pusuarioTokenDTO.catUsuarios.catUsuariosPermisos.Any(x => x.IdCatPermisos.Equals(pIdPermiso) && x.Activo == true && x.Guardar == true))
                    diccionario.Add("GUARDAR", true);
                else
                    diccionario.Add("GUARDAR", false);



                return diccionario;

            }
            catch (Exception)
            {

                return null;
            }

        }

        #region TOKEN
        public async Task<string> GetTokenAsync()
        {
            try
            {
                return await _localStorageService.GetItemAsync<string>(Inicializar.Token_Local);
            }
            catch (Exception ex)
            {

                //////Console.WriteLine("GetTokenAsync:" + ex.Message);
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
        #endregion TOKEN



    }
}
