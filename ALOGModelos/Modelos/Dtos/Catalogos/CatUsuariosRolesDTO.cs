using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatUsuariosRolesDTO
    {
        public int IdCatUsuariosRoles { get; set; }
        [Required]
        public int IdCatUsuarios { get; set; }
        public CatUsuarios CatUsuarios { get; set; }
        [Required]
        public int IdCatRoles { get; set; }
        public CatRoles CatRoles { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        
        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuarios { get; set; }
    }
}
