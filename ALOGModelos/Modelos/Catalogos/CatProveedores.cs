
using ALOG.Modelos.Modelos.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatProveedores
    {
        [Key]
        public int IdCatProveedor { get; set; }
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
        [MaxLength(20)]
        public string NumeroExterior { get; set; }
        [Required]
        [MaxLength(100)]
        public string Colonia { get; set; }
        [Required]
        [MaxLength(10)]
        public string CodigoPostal { get; set; }
        [Required]
        [MaxLength(50)]
        public string Ciudad { get; set; }
        [Required]
        [MaxLength(50)]
        public string Estado { get; set; }

        [MaxLength(20)]
        public string NumeroInterior { get; set; }
        public bool Activo { get; set; } = true;

        [MaxLength(20)]
        public string Acronimo { get; set; }
        [MaxLength(50)]
        public string Clave1G { get; set; }

        [ForeignKey("CatPaises")]
        public int IdCatPaises { get; set; }
        public virtual CatPaises CatPaises { get; set; }


        [ForeignKey("catEstados")]
        public int IdCatPaisEstados { get; set; }
        public CatPaisEstados catEstados { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }


        public virtual ICollection<CatProveedoresConfig> GetCatPatiosConfigs { get; set; }

        public virtual ICollection<CatProveedoresTarifas> GetCatProveedoresTarifas { get; set; }

        public virtual ICollection<CatPatios> GetCatPatios { get; set; }



    }
}
