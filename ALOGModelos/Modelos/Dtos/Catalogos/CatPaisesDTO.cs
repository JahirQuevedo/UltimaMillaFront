using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatPaisesDTO
    {
        public int IdCatPaises { get; set; }
        [Required]
        public string Nombre { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public  ICollection<CatPaisEstadosDTO> GetCatPaisEstados { get; set; }
    }
}
