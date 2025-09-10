using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatClientesDTO
    {
        public int IdCatCliente { get; set; }
        [Required]
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


        [MaxLength(20)]
        public string Acronimo { get; set; }



        public int IdCatPaises { get; set; }



        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]

        public int IdUsuarioRegistro { get; set; }

        public ICollection<CatClientesConfigDTO> GetcatClientesConfig { get; set; }
        //[NotMapped]
        //public virtual ICollection<CatClientesExterno> GetcatClienteExternos { get; set; }

        public ICollection<CatClientesLNegocioDTO> GetcatClientesLNegocios { get; set; }
        public ICollection<CatClientesServicioAduanaDTO> GetcatClientesServicioAduanas { get; set; }
        public ICollection<CatClienteTarifaDTO> GetcatClienteTarifas { get; set; }
        public ICollection<CatClientesProyectosDTO> GetClientesProyectos { get; set; }

    }
}
