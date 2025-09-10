using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatPaisEstadosDTO
    {
        public int IdCatPaisEstados { get; set; }
        [Required]        
        public int IdCatPais { get; set; }        
        [Required]
        [StringLength(100)]
        public string Estado { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
