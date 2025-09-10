using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.Autenticacion.TokenUsuario {
    public class Response {
        
        [JsonProperty("$id")]
        public string InternalId { get; set; }
        public Result Result { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCompletedSuccessfully { get; set; }
        public int CreationOptions { get; set; }
        public bool IsFaulted { get; set; }
    }
}
