using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatNavierasDTO
    {
        public int IdCatNaviera { get; set; }

        [Required]
        [MaxLength(150)]
        public string RazonSocial { get; set; }
        [MaxLength(20)]
        public string Acronimo { get; set; }
        [Required]
        [MaxLength(15)]
        public string RFC { get; set; }
        
        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
