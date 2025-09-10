using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatClientesProyectos  
    {
        [Key]
        public int IdCatClientesProyectos { get; set; }

        [Required]
        [ForeignKey("CatClientes")]
        public int IdCatCliente { get; set; }
        public virtual CatClientes CatClientes { get; set; }

        [Required]
        [ForeignKey("CatProyectos")]
        public int IdCatProyecto { get; set; }
        public virtual CatProyectos CatProyectos { get; set; }
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
