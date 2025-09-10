using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatServiciosDTO
    {
        public int IdCatServicio { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        
        public int IdCatEmpresas { get; set; } = 1;
        

        [Required]
        
        public int IdUsuarioRegistro { get; set; }
        
    }
}
