using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatTipoClasificacion
    {
        [Key]
        public int IdCatTipoClasificacion { get; set; }
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }
        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
