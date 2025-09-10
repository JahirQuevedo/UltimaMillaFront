using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
using ALOG.Modelos.Modelos.Catalogos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatProveedoresTarifas
    {
        [Key]
        public int IdCatProvTarifas { get; set; }
        [Required]
        [ForeignKey("catProveedores")]
        public int IdCatProveedores { get; set; }
        public CatProveedores catProveedores { get; set; }



        [Required]
        [ForeignKey("catServicios")]
        public int IdCatServicio { get; set; }
        public CatServicios catServicios { get; set; }

        [Required]
        public double Tarifa { get; set; } = 0.0;
        public double tarifa_cotizacion { get; set; } = 0.0;
        [Required]
        public double Impuesto { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }

        [Required]
        [ForeignKey("catAduana")]
        public int IdCatAduana { get; set; }
        public CatAduana catAduana { get; set; }

        public virtual ICollection<CatProveedoresTarifaPatio> GetCatProveedoresTarifaPatios { get; set; }





    }
}
