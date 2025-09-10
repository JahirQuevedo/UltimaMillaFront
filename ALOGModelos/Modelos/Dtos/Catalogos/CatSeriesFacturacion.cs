using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Catalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatSeriesFacturacion
    {
        [Key]
        public int IdCatSeriesFact { get; set; }

        [ForeignKey("catLineaNegocio")]
        public int IdCatLineaNegocio { get; set; }
        public CatLineaNegocio catLineaNegocio { get; set; }

        [ForeignKey("catAduana")]
        public int IdCatAduana { get; set; }
        public CatAduana catAduana { get; set; }

        public string Serie { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
