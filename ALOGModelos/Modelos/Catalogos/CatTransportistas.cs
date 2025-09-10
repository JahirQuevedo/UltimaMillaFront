using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatTransportistas 
    {
        [Key]
        public int IdCatTransportista { get; set; }

        
        [Required]
        [MaxLength(250)]
        public string RazonSocial { get; set; }
        [Required]
        [MaxLength(20)]

        public string RFC { get; set; }
        [Required]
        [MaxLength(50)]
        public string Correo { get; set; }
        
        [MaxLength(20)]
        public string telefono { get; set; }
        
        [MaxLength(100)]
        public string Calle { get; set; }
        
        [MaxLength(10)]
        public string NumeroExterior { get; set; }
        
        [MaxLength(100)]
        public string Colonia { get; set; }
        
        [MaxLength(10)]
        public string CodigoPostal { get; set; }
        
        [MaxLength(50)]
        public string Ciudad { get; set; }
        
        [MaxLength(50)]
        public string Estado { get; set; }
        
        [MaxLength(10)]
        public string NumeroInterior { get; set; }
        public bool Activo { get; set; } = true;

        [MaxLength(20)]
        public string Acronimo { get; set; }
        [MaxLength(50)]

        [ForeignKey("CatPaises")]
        public int IdCatPaises { get; set; }
        public CatPaises CatPaises { get; set; }

        [ForeignKey("catEstados")]
        public int IdCatPaisEstados { get; set; }
        public CatPaisEstados catEstados { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuarios { get; set; }

        [ForeignKey("catEmpresas")]
        public int IdCatEmpresas { get; set; } = 1;
        public virtual CatEmpresas catEmpresas { get; set; }
    }
}
