using ALOGRepositorios.Services.Login.ILogin;
using ALOGRespositorios.Modelos.Dtos.Control;
using Microsoft.AspNetCore.Components;

namespace ALOGRepositorios.Services.Login
{

    public class UsuarioTokenService : IUsuarioTokenService
    {
        [Inject] UsuarioTokenDTO usuarioTokenDTO { get; set; }
        private ILoginService _iLoginService { get; set; }
        public UsuarioTokenService(ILoginService loginService)
        {
            _iLoginService = loginService;
        }

        public async Task<UsuarioTokenDTO> getUsuarioTokenService()
        {
            var objrespuesta = await _iLoginService.ObtenerdatosToken();
            usuarioTokenDTO = objrespuesta;
            return objrespuesta;
        }
    }
}
