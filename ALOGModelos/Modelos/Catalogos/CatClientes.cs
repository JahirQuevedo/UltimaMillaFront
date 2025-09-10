using ALOG.Modelos.Modelos.Dtos;


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatClientes
    {
        [Key]
        public int IdCatCliente { get; set; }
        [Required]
        [MaxLength(150)]
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


        [ForeignKey("CatPaises")]
        public int IdCatPaises { get; set; }
        public CatPaises CatPaises { get; set; }

        [Required]
        [ForeignKey("catEstados")]
        public int IdCatPaisEstados { get; set; }
        public CatPaisEstados catEstados { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("catUsuario")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuario { get; set; }


        public string RegimenFiscalSAT { get; set; }
        public string UsoCFDISAT { get; set; }



        public virtual ICollection<CatClientesConfig> GetcatClientesConfig { get; set; }
        //[NotMapped]
        //public virtual ICollection<CatClientesExterno> GetcatClienteExternos { get; set; }

        public virtual ICollection<CatClientesLNegocio> GetcatClientesLNegocios { get; set; }

        public virtual ICollection<CatClientesServicioAduana> GetcatClientesServicioAduanas { get; set; }

        public virtual ICollection<CatClienteTarifa> GetcatClienteTarifas { get; set; }

        public virtual ICollection<CatClientesProyectos> GetClientesProyectos { get; set; }





    }
}
