using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatClientesProyectosDTO
    {
        public int IdCatClientesProyectos { get; set; }

        [Required]       
        public int IdCatCliente { get; set; }        

        [Required]        
        public int IdCatProyecto { get; set; }
       
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]        
        public int IdUsuarioRegistro { get; set; }
       
    }
}
