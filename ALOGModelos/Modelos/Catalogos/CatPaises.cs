using ALOG.Modelos.Modelos.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatPaises
    {
        [Key]
        public int IdCatPaises { get; set; }
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        [MaxLength(15)]
        public string ClavePaisSAT { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public virtual ICollection<CatPaisEstados> GetCatPaisEstados { get; set; }
    }
}
