using ALOG.Modelos.Modelos.Vacios;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
using ALOG.Modelos.Modelos.Catalogos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatPatiosNavieras
    {
        [Key]
        public int IdCatPatiosNavieras { get; set; }
        [Required]
        [ForeignKey("CatNavieras")]
        public int IdCatNaviera { get; set; }
        public CatNavieras CatNavieras { get; set; }

        [Required]
        [ForeignKey("CatPatios")]
        public int IdCatPatios { get; set; }
        public CatPatios CatPatios { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]

        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuarios { get; set; }

    }
}
