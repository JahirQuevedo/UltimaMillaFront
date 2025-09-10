using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.Autenticacion
{
    public class Login
    {

        [JsonProperty("sUsuario")] public string Usuario { get; set; } = string.Empty;
        [JsonProperty("sPass")] public string Password { get; set; } = string.Empty;
    }
}
