using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
using ALOG.Modelos.Modelos.Catalogos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatRecintos
    {
        [Key]
        public int IdCatRecinto { get; set; }
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }
        [MaxLength(20)]
        public string ClaveRecinto { get; set; }

        [Required]
        [ForeignKey("catAduana")]
        public int IdAduana { get; set; }
        public CatAduana CatAduana { get; set; }

        [Required]
        [ForeignKey("catPaises")]
        public int IdCatPais { get; set; }
        public CatPaises catPaises { get; set; }

        [Required]
        [ForeignKey("catEstados")]
        public int IdCatPaisEstados { get; set; }
        public CatPaisEstados catEstados { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [ForeignKey("catUsuarios")]
        public int IdCatUsuarioRegistro { get; set; }
        public CatUsuarios catUsuarios { get; set; }

    }
}
