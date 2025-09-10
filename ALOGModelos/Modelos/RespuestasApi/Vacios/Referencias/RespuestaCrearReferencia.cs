using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.RespuestasApi.Vacios.Referencias {
    public class RespuestaCrearReferencia {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("errorMessages")]
        public List<string> ErrorMessages { get; set; }

        [JsonProperty("idReferenciaALO")]
        public int IdReferenciaALO { get; set; }

        [JsonProperty("idOrdenServicio")]
        public int IdOrdenServicio { get; set; }
    }
}
