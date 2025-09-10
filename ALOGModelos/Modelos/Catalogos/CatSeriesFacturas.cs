using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Dtos;
using ALOG.Modelos.Modelos.Modelos.Catalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatSeriesFacturas
    {
        [Key]
        public int IdCatSeriesFact { get; set; }

        [Required]
        [ForeignKey("catLineaNegocio")]
        public int IdCatLineaNegocio { get; set; }
        public CatLineaNegocio catLineaNegocio { get; set; }

        [Required]
        [ForeignKey("catAduana")]
        public int IdCatAduana { get; set; }
        public CatAduana catAduana { get; set; }

        [Required]
        [ForeignKey("catEmpresas")]
        public int IdCatEmpresa { get; set; }
        public CatEmpresas catEmpresas { get; set; }

        [Required]
        [ForeignKey("catSucursal")]
        public int IdCatSucursal { get; set; }
        public CatSucursales catSucursales { get; set; }

        [Required]
        [StringLength(10)]
        public string Serie { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
