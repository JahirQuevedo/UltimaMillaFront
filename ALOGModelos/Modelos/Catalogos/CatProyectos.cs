using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatProyectos 
    {

        [Key]
        public int IdProyectos { get; set; }
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }
        public string Acronimo { get; set; }


        [ForeignKey("catLineaNegocio")]
        public int IdCatLineaNegocio { get; set; }
        public CatLineaNegocio catLineaNegocio { get; set; }

        [ForeignKey("catEmpresas")]
        public int IdCatEmpresas { get; set; } = 1;
        public virtual CatEmpresas catEmpresas { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }




    }
}
