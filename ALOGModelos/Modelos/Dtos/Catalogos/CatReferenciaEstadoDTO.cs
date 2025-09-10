using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatReferenciaEstadoDTO
    {
        public int IdCatReferenciaEstado { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string Clave { get; set; }
        
        public bool Activo { get; set; } = true;

        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        
        
        public int IdUsuarioRegistro { get; set; }
        
    }
}
