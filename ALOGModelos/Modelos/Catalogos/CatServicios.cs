using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Catalogos {
    public class CatServicios {

        [Key]
        public int IdCatServicio { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("catEmpresas")]
        public int IdCatEmpresas { get; set; } = 1;
        public virtual CatEmpresas catEmpresas { get; set; }

        [Required]
        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }
    }
}
