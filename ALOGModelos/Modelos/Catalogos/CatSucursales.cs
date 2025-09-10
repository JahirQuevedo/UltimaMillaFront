using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Catalogos;


namespace ALOG.Modelos.Modelos.Modelos.Catalogos
{
    public class CatSucursales
    {
        [Key]
        public int IdCatSucursal { get; set; }
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }
        [MaxLength(20)]
        public string RFC { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]

        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuarios { get; set; }

        public int IdCatEmpresas { get; set; } = 1;
        public CatEmpresas catEmpresas { get; set; }
    }
}
