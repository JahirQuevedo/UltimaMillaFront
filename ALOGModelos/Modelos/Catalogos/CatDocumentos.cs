using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatDocumentos
    {
        [Key]
        public int IdCatDocumento { get; set; }

        public string Nombre { get; set; }

        public bool Activo { get; set; } = true;


        public DateTime FechaRegistro { get; set; }


        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuario { get; set; }
    }
}
