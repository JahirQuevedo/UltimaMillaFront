using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatProveedoresConfig
    {
        [Key]
        public int IdCatProvConfig { get; set; }

        [ForeignKey("CatProveedores")]
        public int IdCatProveedor { get; set; }
        public virtual CatProveedores CatProveedores { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("catTiposConfig")]
        public int IdCatTipoConfig { get; set; }
        public virtual CatTiposConfig catTiposConfig { get; set; }

        public Double Valor1 { get; set; }
        [MaxLength(1500)]
        public String Valor2 { get; set; }

        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }
    }
}
