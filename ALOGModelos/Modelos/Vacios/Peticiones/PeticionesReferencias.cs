using ALOG.Modelos.Modelos.Dtos;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Utilerias;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Vacios.Peticiones
{
    public class PeticionesReferencias
    {
        [Key]
        [JsonProperty("idReferencia")] public int IdReferencia { get; set; } = 0;
        [JsonProperty("ticket")] public int Ticket { get; set; } = 0;
        [JsonProperty("transporte_RFC")] public string Transporte_RFC { get; set; } = string.Empty;
        [JsonProperty("transporte_RazonSocial")] public string Transporte_RazonSocial { get; set; } = string.Empty;
        [JsonProperty("trasporteId")] public int TrasporteId { get; set; } = 0;
        [JsonProperty("transporte_Usuario")] public string Transporte_Usuario { get; set; } = string.Empty;
        [JsonProperty("transporte_UsuarioEmail")] public string Transporte_UsuarioEmail { get; set; } = string.Empty;
        [JsonProperty("comentarios")] public string Comentarios { get; set; } = string.Empty;
        [JsonProperty("tipoReferencia")] public int TipoReferencia { get; set; } = 0;
        [JsonProperty("contenedores")] public List<PeticionesContenedor> Contenedores { get; set; } = new List<PeticionesContenedor>();
        public string Procesado { get; set; } = string.Empty;
        public DateTime FechaProcesado { get; set; }
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public string EstadoReferencia { get; set; } = "A";
        public int IdCatReferenciaEstado { get; set; } = 0;
        public bool Activo { get; set; } = true;
        public int IdOrden { get; set; } = 0;
        [JsonConverter(typeof(JsonConvertCustom<Ordenes>))]
        [JsonProperty("ordenes")]
        public Ordenes? Orden { get; set; }

    }
}
