using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.Autenticacion {
    public class Token {

        [JsonProperty("token")] public string TokenBearer { get; set; } = string.Empty;
        [JsonProperty("usuario")]  public string Usuario { get; set; } = string.Empty;
        [JsonProperty("statusCode")]  public int StatusCode { get; set; } = 0;
    }
}
