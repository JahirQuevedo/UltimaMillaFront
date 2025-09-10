using ALOG.Modelos.Modelos.Catalogos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatPatiosTarifaCliente
    {
        [Key]
        public int IdCatPatiosTarifaCte { get; set; }
        [Required]
        [ForeignKey("catPatios")]
        public int IdCatPatios { get; set; }
        public CatPatios catPatios { get; set; }
        [Required]
        [ForeignKey("catClientes")]
        public int IdCatClientes { get; set; }
        public CatClientes catClientes { get; set; }
        [Required]
        public double Tarifa { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("catUsuarios")]
        public int IdUsuario { get; set; }
        public CatUsuarios catUsuarios { get; set; }

        [ForeignKey("catAduana")]
        public int IdCatAduana { get; set; }
        public virtual CatAduana catAduana { get; set; }

    }
}
