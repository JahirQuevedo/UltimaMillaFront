using ALOGRespositorios.Modelos.Dtos.Control;

namespace ALOGRepositorios.Services.Login.ILogin
{
    public interface IUsuarioTokenService
    {
        Task<UsuarioTokenDTO> getUsuarioTokenService();
    }
}
