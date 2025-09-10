using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatAduana
    {
        [Key]
        public int IdCatAduana { get; set; }
        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; }
        [Required]
        public int Aduana { get; set; }
        [Required]
        public int Seccion { get; set; }
        [Required]
        [MaxLength(20)]
        public string Acronimo { get; set; }
        [Required]
        public bool Activo { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        public int IdUsuarioRegistro { get; set; }
    }
}
