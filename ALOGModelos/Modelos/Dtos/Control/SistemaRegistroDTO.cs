using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.DTO.Control
{
    public class SistemaRegistroDTO
    {
        [Required]
        public string nombre { get; set; }
        [Required]
        public bool activo { get; set; } = true;

        [Required]
        public int IdCatCliente { get; set; }

        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string rol { get; set; }
    }
}
