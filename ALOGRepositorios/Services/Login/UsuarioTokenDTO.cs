
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRespositorios.Modelos.Dtos.Control
{
    public class UsuarioTokenDTO
    {

        public int IdCatUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public CatUsuarios catUsuarios { get; set; }

    }
}
