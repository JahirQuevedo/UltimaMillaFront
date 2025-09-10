using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatTipoPuesto 
    {
        [Key]
        public int IdCatTipoPuesto { get; set; }
        [Required]
        [MaxLength(150)]
        public string Puesto { get; set; }
        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }= DateTime.Now;

        //[ForeignKey("catUsuarios")]
        //public int IdUsuarioRegistro { get; set; }
        //public CatUsuarios catUsuarios { get; set;} 
    }
}
