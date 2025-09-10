
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatReferenciaEstado
    {
        [Key]
        public int IdCatReferenciaEstado { get; set; }
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }
        [Required]
        [MaxLength(15)]
        public string Clave { get; set; }
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
