
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatTipoEstados 
    {
        [Key]
        public int IdCatTipoEstados { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required, MaxLength(50)]
        public string TipoEstado { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;


    }
}
