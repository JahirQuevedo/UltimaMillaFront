using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatTiposConfig 
    {
        [Key]
        public int IdCatTiposConfig { get; set; }
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(50)]
        public string Clave { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

    }
}
