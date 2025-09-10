using System.ComponentModel.DataAnnotations;
using ALOG.Modelos.Modelos.Dtos;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatTipoMoneda 
    {
        [Key]
        public int IdTipoMoneda { get; set; }

        [Required]
        [MaxLength(20)]
        public string Descripcion { get; set; }

        [Required]
        public string ClaveSAT { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
    }
}
