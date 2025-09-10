using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatAduanaDTO
    {

        public int IdCatAduana { get; set; }
        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; }
        [Required]
        public int Aduana { get; set; }
        [Required]
        public int Seccion { get; set; }
        [Required]
        [MaxLength(20)]
        public string Acronimo { get; set; }
        [Required]
        public bool Activo { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        public int IdUsuarioRegistro { get; set; }
    }
}
