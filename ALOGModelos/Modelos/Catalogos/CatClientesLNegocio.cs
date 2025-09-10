
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    /*
     * Tabla: CatClientesLNegocio
     * Descrpición: Relación de los clientes con la línea de negocio en la que participan
     */
    public class CatClientesLNegocio  
    {

        [Key]
        public int IdCatClientesLNegocio { get; set; }
        [Required]
        [ForeignKey("catCliente")]
        public int IdCliente { get; set; }
        public virtual CatClientes catCliente { get; set; }

        [Required]
        [ForeignKey("catLineaNegocio")]
        public int IdLineaNegocio { get; set; } 
        public CatLineaNegocio catLineaNegocio { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("catUsuario")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuario { get; set; }


    }
}
