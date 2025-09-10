using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatPatiosConfigDTO
    {
        public int IdCatPatiosConfig { get; set; }
        [Required]        
        public int IdCatPatios { get; set; }        
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]        
        public int IdCatTipoConfig { get; set; }
        
        public Double Valor1 { get; set; }
        public String Valor2 { get; set; }
        
        public int IdUsuarioRegistro { get; set; }        
    }
}
