using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatProveedoresTarifaPatio
    {
        [Key]
        public int IdCatProvTarifaPatio { get; set; }
        [ForeignKey("catProveedoresTarifas")]
        public int IdCatProvTarifas { get; set; }
        public virtual CatProveedoresTarifas catProveedoresTarifas { get; set; }

        [ForeignKey("catPatio")]
        public int IdCatPatio { get; set; }
        public virtual CatPatios catPatio { get; set; }
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
