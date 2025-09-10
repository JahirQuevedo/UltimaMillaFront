using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatEmpresasDTO
    {
        public int IdCatEmpresa { get; set; }
        [Required]
        public string RazonSocial { get; set; }
        [Required]
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
        [MaxLength(50)]
        public string Ciudad { get; set; }
        [Required]
        [MaxLength(50)]
        public string Estado { get; set; }

        [MaxLength(10)]
        public string NumeroInterior { get; set; }
        
        public bool Activo { get; set; } = true;


        public DateTime FechaRegistro { get; set; } = DateTime.Now;        
        
        public int IdUsuarioRegistro { get; set; }

        public ICollection<CatSucursalesDTO> GetCatSucursales { get; set; }



    }
}
