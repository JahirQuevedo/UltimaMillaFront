using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatUsuariosDTO
    {
        public int IdCatUsuarios { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string ApellidoMaterno { get; set; }
        [Required]
        public string ApellidoPaterno { get; set; }
        [Required]
        public string RFC { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public string Puesto { get; set; }
        [Required]
        public string Telefono { get; set; }

        public string Celular { get; set; }
        public string Usuario { get; set; }

        [Required]
        public bool activo { get; set; } = true;
        [Required]
        public string passSistema { get; set; }
        [Required]
        public string salt { get; set; }

        [Required]        
        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuarios { get; set; }
    }
}
