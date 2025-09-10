using ALOG.Modelos.Modelos.Catalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatLineaNegocioTarifa
    {
        [Key]
        public int IdCatLineaNegocioTarifa { get; set; }
        [Required]
        //Servicio

        public int IdCatServicio { get; set; }
        public CatServicios catServicios { get; set; }

        //LN
        [Required]

        public int IdCatLineaNegocio { get; set; }
        public CatLineaNegocio catLineaNegocio { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("catUsuario")]
        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuario { get; set; }


        public virtual ICollection<CatLineaNegocioTariPrecio> GetCatLineaNegocioTariPrecios { get; set; }
    }
}
