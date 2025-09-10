using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatProyectosDTO
    {
        public int IdProyectos { get; set; }
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }


        
        public int IdCatLineaNegocio { get; set; }
        

        
        public int IdCatEmpresas { get; set; } = 1;
        

        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]        
        public int IdUsuarioRegistro { get; set; }
        
    }
}
