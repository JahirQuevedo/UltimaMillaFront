using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatPaisCP
    {

        [Key]
        public int IdCodigoPostal { get; set; }
        [Required]
        [MaxLength(20)]
        public string CodigoPostal { get; set; }
        [Required]
        [MaxLength(20)]
        public string ClaveEstado { get; set; }
        [Required]
        [MaxLength(20)]
        public string ClaveMunicipio { get; set; }
        [Required]
        [MaxLength(20)]
        public string ClaveMunDel { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

    }
}
