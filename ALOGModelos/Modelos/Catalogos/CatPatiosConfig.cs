using ALOG.Modelos.Modelos.Dtos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatPatiosConfig
    {
        [Key]
        public int IdCatPatiosConfig { get; set; }
        [Required]
        [ForeignKey("catPatios")]
        public int IdCatPatios { get; set; }
        public CatPatios catPatios { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("catTiposConfig")]
        public int IdCatTipoConfig { get; set; }
        public CatTiposConfig catTiposConfig { get; set; }
        public Double Valor1 { get; set; }
        [MaxLength(1500)]
        public String Valor2 { get; set; }

        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }



    }
}
