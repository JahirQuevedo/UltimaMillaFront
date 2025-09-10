using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatProveedoresTarifasDTO
    {
        public int IdCatProvTarifas { get; set; }
        [Required]
        
        public int IdCatProveedores { get; set; }
        



        [Required]
        
        public int IdCatServicio { get; set; }
        

        [Required]
        public double Tarifa { get; set; } = 0.0;
        public double tarifa_cotizacion { get; set; } = 0.0;
        [Required]
        public double Impuesto { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        
        public int IdUsuarioRegistro { get; set; }
        

        [Required]
        
        public int IdCatAduana { get; set; }
        

        public  ICollection<CatProveedoresTarifaPatioDTO> GetCatProveedoresTarifaPatios { get; set; }
    }
}
