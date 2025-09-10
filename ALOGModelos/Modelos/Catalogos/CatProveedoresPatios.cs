using ALOG.Modelos.Modelos.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatProveedoresPatios
    {

        [Key]
        public int IdProveedorPatio { get; set; }


        [Required]
        [ForeignKey("catAduana")]
        public int IdCatAduana { get; set; }
        public virtual CatAduana catAduana { get; set; }

        [Required]
        [ForeignKey("catProveedores")]
        public int IdCatProveedor { get; set; }
        public virtual CatProveedores catProveedores { get; set; }

        [Required]
        [ForeignKey("catPatios")]
        public int IdCatPatio { get; set; }
        public virtual CatPatios catPatios { get; set; }

        public DateTime FechaRegistro { get; set; }

        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }


        public bool Activo { get; set; }
    }
}
