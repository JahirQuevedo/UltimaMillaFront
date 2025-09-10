using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatRolesPermisosDTO
    {
        public int IdCatRolesPermisos { get; set; }
        
        [Required]
        public int IdCatRoles { get; set; }
        public CatRoles CatRoles { get; set; }
        
        [Required]
        public int IdCatPermisos { get; set; }        
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
