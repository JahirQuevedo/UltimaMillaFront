using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.Collections.Generic;

using ALOG.Modelos.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Catalogos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatEmpresas
    {
        [Key]
        public int IdCatEmpresa { get; set; }
        [Required]
        [MaxLength(200)]
        public string RazonSocial { get; set; }
        [Required]
        [MaxLength(20)]
        public string RFC { get; set; }

        [Required]
        [MaxLength(50)]
        public string Correo { get; set; }
        [Required]
        [MaxLength(20)]
        public string telefono { get; set; }
        [Required]
        [MaxLength(100)]
        public string Calle { get; set; }
        [Required]
        [MaxLength(10)]
        public string NumeroExterior { get; set; }
        [Required]
        [MaxLength(100)]
        public string Colonia { get; set; }
        [Required]
        [MaxLength(10)]
        public string CodigoPostal { get; set; }
        [Required]
        [MaxLength(20)]
        public string Acronimo { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Ciudad { get; set; }
        [Required]
        [MaxLength(50)]
        public string Estado { get; set; }

        [MaxLength(10)]
        public string NumeroInterior { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("catUsuario")]
        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuario { get; set; }

        public List<CatSucursales> GetCatSucursales { get; set; }

    }
}
