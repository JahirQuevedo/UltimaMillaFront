using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatTransportistasDTO
    {
        public int IdCatTransportista { get; set; }


        [Required]
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

        [Required]
        public int IdPais { get; set; }
        public CatPaises CatPaises { get; set; }


        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        
        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuarios { get; set; }

        [Required]
        public int IdCatEmpresas { get; set; } = 1;
        public  CatEmpresas catEmpresas { get; set; }
    }
}
