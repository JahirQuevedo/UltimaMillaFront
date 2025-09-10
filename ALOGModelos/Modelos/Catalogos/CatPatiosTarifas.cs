using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatPatiosTarifas
    {
        [Key]
        public int IdCatPatiosTarifas { get; set; }

        [ForeignKey("CatPatios")]
        public int IdCatPatios { get; set; }
        public CatPatios CatPatios { get; set; }

        [ForeignKey("catServicios")]
        public int IdCatServicios { get; set; }
        public CatServicios CatServicios { get; set; }

        [Required]
        public double Tarifa { get; set; }
        [Required]
        public double Impuesto { get; set; }
        public bool Activo { get; set; }
    }
}
