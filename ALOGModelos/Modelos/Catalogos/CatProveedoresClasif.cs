using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatProveedoresClasif
    {
        [Key]
        public int IdCatProvClasif { get; set; }
        [Required]
        [ForeignKey("catProveedores")]
        public int IdCatProveedor { get; set; }
        public CatProveedores catProveedores { get; set; }

        [Required]
        [ForeignKey("catTipoClasificacion")]
        public int IdCatTipoClasificacion { get; set; }
        public CatTipoClasificacion catTipoClasificacion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
