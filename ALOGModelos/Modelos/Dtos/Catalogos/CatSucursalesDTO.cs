using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatSucursalesDTO
    {
        public int IdCatSucursal { get; set; }
        [Required]
        public string Nombre { get; set; }

        public string RFC { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]        
        public int IdUsuarioRegistro { get; set; }
                
        public int IdCatEmpresas { get; set; } = 1;
        
    }
}
