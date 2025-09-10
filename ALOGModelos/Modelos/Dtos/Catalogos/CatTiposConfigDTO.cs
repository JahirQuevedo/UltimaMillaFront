using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatTiposConfigDTO
    {
        public int IdCatTiposConfig { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Clave { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
