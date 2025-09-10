using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatPermisos
    {
        [Key]
        public int IdCatPermisos { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public bool Activo { get; set; } = true;

        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

    }
}
