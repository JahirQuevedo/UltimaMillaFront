using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatContenedor  
    {

        [Key]
        public int IdCatContenedor { get; set; }

        [Required]
        [MaxLength(10)]
        public string Nomenclatura { get; set; }
        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; }=DateTime.Now;
        [Required]
        [ForeignKey("catUsuario")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuario { get; set; }

    }
}
