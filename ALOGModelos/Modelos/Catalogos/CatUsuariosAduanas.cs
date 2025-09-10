using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatUsuariosAduanas
    {
        [Key]
        public int IdCatUsuarioAduana { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("catUsuarios")]
        public int IdCatUsuario { get; set; }
        public CatUsuarios catUsuarios { get; set; }

        [Required]
        [ForeignKey("catAduana")]
        public int IdCatAduana { get; set; }
        public CatAduana catAduana { get; set; }
    }
}
