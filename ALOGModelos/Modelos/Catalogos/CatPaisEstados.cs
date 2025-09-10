
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatPaisEstados
    {
        [Key]
        public int IdCatPaisEstados { get; set; }
        [Required]
        [ForeignKey("catPaises")]
        public int IdCatPais { get; set; }
        public virtual CatPaises catPaises { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }=DateTime.Now;
        [MaxLength(15)]
        public string CodEstadoSAT { get; set; }
       


    }
}
