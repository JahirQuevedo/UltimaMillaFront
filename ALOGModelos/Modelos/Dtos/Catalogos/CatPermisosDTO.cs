using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatPermisosDTO
    {
        public int IdCatPermisos { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public bool Activo { get; set; } = true;
        
        public int IdUsuarioRegistro { get; set; }        

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
