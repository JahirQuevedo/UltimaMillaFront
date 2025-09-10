using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatFormatoRefDetDTO
    {
        public int IdCatFormatoRefDet { get; set; }

        [Required]        
        public int IdCatFormatoRef { get; set; }
        
        [Required]
        public int OrdenCampo { get; set; }
        [Required]
        public string TipoDato { get; set; }
        [Required]
        public string Especificacion { get; set; }
        public string Valor { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
