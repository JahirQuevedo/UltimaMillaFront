using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatRoles

    {
        [Key]
        public int IdCatRoles { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }
    }
}
