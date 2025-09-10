
using ALOG.Modelos.Modelos.Dtos.Catalogos;
using System.Text.Json.Serialization;

namespace ALOG.Modelos.Modelos.Dtos.Solicitudes {
    public class SolTicketDTO {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("idCatEmpresa")]
        public int IdCatEmpresa { get; set; } = 1;

        [JsonPropertyName("idCatLineaNegocio")]
        public int IdCatLineaNegocio { get; set; } = 1;

        [JsonPropertyName("idCatAduana")]
        public int IdCatAduana { get; set; } = 1;
        
        [JsonPropertyName("idCatProyecto")]
        public int IdCatProyecto { get; set; } = 0;

        [JsonPropertyName("idCatClienteSolicitante")]
        public int IdCatClienteSolicitante { get; set; } = 0;

        [JsonPropertyName("idCatClienteFacturar")]
        public int IdCatClienteFacturar { get; set; }

        [JsonPropertyName("idCatServicio")]
        public int IdCatServicio { get; set; }

        [JsonPropertyName("idCatUsuario")]
        public int IdCatUsuario { get; set; } = 1;

        [JsonPropertyName("ticket")]
        public int Ticket { get; set; } = 0;

        [JsonPropertyName("moneda")]
        public string Moneda { get; set; } = string.Empty;

        [JsonPropertyName("referenciaCliente")]
        public string ReferenciaCliente { get; set; }

        [JsonPropertyName("referenciaClienteFacturar")]
        public string ReferenciaClienteFacturar { get; set; }

        [JsonPropertyName("contenedor")]
        public string Contenedor { get; set; }

        [JsonPropertyName("RfcClienteFacturar")]
        public string RfcClienteFacturar { get; set; }
        public string RazonSocialClienteFacturar { get; set; } = string.Empty;

        [JsonPropertyName("claveTipoContenedor")]
        public string ClaveTipoContenedor { get; set; }
        public string ReferenciaAlo { get; set; } = string.Empty;
        public string Estatus { get; set; }
        public bool Seleccionado { get; set; }
        public List<CatServiciosDTO> Servicios { get; set; }
        public List<string> Errores { get; set; } = new List<string>();
    }
}
