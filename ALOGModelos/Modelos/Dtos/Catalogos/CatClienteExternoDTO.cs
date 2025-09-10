using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatClienteExternoDTO
    {

        public int IdCatClienteExterno { get; set; }
        [Required]
        
        public int IdCatClientes { get; set; }       

        [Required]
       
        public int IdCatClientesExterno { get; set; }
               
        public bool Activo { get; set; } = true;
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;        
        public int IdUsuarioRegistro { get; set; }
       
    }
}
