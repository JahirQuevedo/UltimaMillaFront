using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatClientesLNegocioDTO
    {
        public int IdCatClientesLNegocio { get; set; }
        [Required]        
        public int IdCliente { get; set; }
        
        [Required]        
        public int IdLineaNegocio { get; set; }
        
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
              
        public int IdUsuarioRegistro { get; set; }
        
    }
}
