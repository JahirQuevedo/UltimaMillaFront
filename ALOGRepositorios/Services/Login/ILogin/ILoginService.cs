using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Control;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOGRespositorios.Modelos.Dtos.Control;

namespace ALOGRepositorios.Services.Login.ILogin
{
    public interface ILoginService
    {



        Task<SistemaLoginRespuestaDTO> Acceder(SistemaLoginDTO pSistemasLoginDTO);
        Task<RespuestaRegistroUsuarioDTO> ResgistraUsuario(UsuarioRegistroDTO pusuarioRegistro);
        Task Salir();
        Task<UsuarioTokenDTO> ObtenerdatosToken();


        Task<CatUsuarios> ObtenerCatUsuarios(SolDatosUsuariosDTO pusuarioRegistro);
        Task<bool> validarAccesoPagina(int pIdPermiso, UsuarioTokenDTO pusuarioTokenDTO);
        Task<Dictionary<string, bool>> validarControlesPagina(int pIdPermiso, UsuarioTokenDTO pusuarioTokenDTO);
    }
}
