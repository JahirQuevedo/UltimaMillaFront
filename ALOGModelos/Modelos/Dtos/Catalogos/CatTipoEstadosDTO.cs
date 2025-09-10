using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatTipoEstadosDTO
    {
        public int IdCatTipoEstados { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required, MaxLength(50)]
        public string TipoEstado { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
