using ALOGModelos.Modelos.Catalogos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ALOGModelos.Modelos.Vacios.Peticiones
{
    public class PeticionesContenedor1
    {
        public int IdContenedor { get; set; } = 0;
        [JsonProperty("contenedor")]
        [Required(ErrorMessage = "El número de contenedor no puedo estar vacío")]
        [RegularExpression(@"^[A-Z]{4}\d{7}$", ErrorMessage = "El número de contenedor debe contener 4 letras seguidas de 7 números.")]
        public string NumeroContenedor { get; set; } = string.Empty;
        [Required(ErrorMessage = "No has indicado la referencia del cliente")]
        public string RefenciaCliente { get; set; } = string.Empty;
        public string ClaveTipoContenedor { get; set; } = string.Empty;
        public int PatioId { get; set; } = 0;
        public CatPatios catPatios { get; set; }
        public string Patio_RazonSocial { get; set; } = string.Empty;
        public string Moneda { get; set; } = string.Empty;
        public DateTime FechaTocaPiso { get; set; } = DateTime.Now;
        public string Bl { get; set; } = string.Empty;
        public string Buque { get; set; } = string.Empty;
        public int ClienteId { get; set; } = 0;
        public string Cliente_RazonSocial { get; set; } = string.Empty;
        public string Cliente_RFC { get; set; } = string.Empty;
        public string Cliente_Solicitante { get; set; } = string.Empty;
        public int ConsignadoId { get; set; } = 0;
        public string Consignado_RazonSocial { get; set; } = string.Empty;
        public string Consignado_RFC { get; set; } = string.Empty;
        public string FondoFinanciamiento { get; set; } = string.Empty;
        public decimal MontoSolicitud { get; set; } = 0;
        public decimal MontoTotal { get; set; } = 0;
        public int AduanaId { get; set; } = 0;
        public DateTime FechaSolDevolucion { get; set; }
        public DateTime FechaPagoGarantiaNav { get; set; }
        public string Naviera_RFC { get; set; } = string.Empty;
        public int Naviera_Id { get; set; } = 0;
        public string Naviera_RazonSocial { get; set; } = string.Empty;
        public string Aduana { get; set; } = string.Empty;
        public List<PeticionesServicio> Servicios { get; set; } = new List<PeticionesServicio>();
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaCierre { get; set; }
        public string EstadoContenedor { get; set; }
        public bool Activo { get; set; } = true;
        public int IdClienteFacturarA { get; set; }
        public bool IsSelected { get; set; } = false;
        public int IdReferencia { get; set; }
        public string FolioManiobra { get; set; }
        public int IdEstadoContenedor { get; set; }


    }
}
