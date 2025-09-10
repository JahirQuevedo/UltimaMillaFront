using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatContenedorDTO
    {
        public int IdCatContenedor { get; set; }
        [Required]
        [MaxLength(10)]
        public string Nomenclatura { get; set; }
        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        
        public int IdUsuarioRegistro { get; set; }
        
    }
}
