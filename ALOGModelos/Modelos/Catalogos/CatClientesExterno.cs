using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    /*
     * Clientes realacionados con el cliente principal. Clientes de nuestro cliente.
     */
    public class CatClientesExterno  
    {
        
        [Key]
        public int IdCatClienteExterno { get; set; }
        [Required]
        [ForeignKey("catClientes")]
        public int IdCatCliente { get; set; }
        public virtual CatClientes catClientes { get; set; }

        [Required]
        [ForeignKey("catClientesAsociado")]
        public int IdCatClientesAsociado { get; set; }
        public  CatClientes catClientesAsociado { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("catUsuario")]
        public int IdUsuarioRegistro { get; set; }
        public  CatUsuarios catUsuario { get; set; }

    }
}
