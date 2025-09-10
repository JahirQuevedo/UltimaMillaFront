using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatTipoContacto 
    {
        [Key]
        public int IdCatTipoContacto  { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } 
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

    }
}
