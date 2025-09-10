
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Dtos.Vacios
{
    public class PeticionesContenedoresDTO
    {

        public int IdContenedor { get; set; }
        [Required]
        public string Contenedor { get; set; }
        [Required]
        public string RefenciaCliente { get; set; }
        [Required]
        public string ClaveTipoContenedor { get; set; }
        public int PatioId { get; set; }

        public string Patio_RazonSocial { get; set; }

        public string Patio_RFC { get; set; }

        public string Moneda { get; set; }
        public DateTime FechaTocaPiso { get; set; }
        public string BL { get; set; }
        public string Buque { get; set; }
        public int ClienteId { get; set; }
        [Required]
        public string Cliente_RazonSocial { get; set; }
        [Required]
        public string Cliente_RFC { get; set; }
        [Required]
        public string Cliente_Solicitante { get; set; }
        public int ConsignadoId { get; set; }
        public string Consignado_RazonSocial { get; set; }
        public string Consignado_RFC { get; set; }
        public string FondoFinanciamiento { get; set; }
        public double MontoSolicitud { get; set; }
        public double MontoTotal { get; set; }
        [Required]
        public int AduanaId { get; set; }
        public DateTime FechaSolDevolucion { get; set; }
        public DateTime FechaPagoGarantiaNav { get; set; }
        [Required]
        public string Naviera_RFC { get; set; }
        public int Naviera_Id { get; set; }
        [Required]
        public string Naviera_RazonSocial { get; set; }
        public string Aduana { get; set; }

        //public ICollection<CatPatios> catPatios { get; set; }
        public ICollection<PeticionesServiciosDTO> Servicios { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaCierre { get; set; }

        public string EstadoContenedor { get; set; } = "A";
        public bool Activo { get; set; } = true;
        public string FolioManiobra { get; set; }

    }
}
