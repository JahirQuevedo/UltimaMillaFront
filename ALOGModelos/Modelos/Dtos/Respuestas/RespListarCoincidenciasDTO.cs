using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.Dtos.Respuestas
{
    public class RespListarCoincidenciasDTO
    {
        [JsonProperty("idCatCliente")] public int Id { get; set; }
        [JsonProperty("razonSocial")] public string RazonSocial { get; set; }
        [JsonProperty("pais")] public string Pais { get; set; }
        [JsonProperty("rfc")] public string RFC { get; set; }
        [JsonProperty("estado")] public string Estado { get; set; }
        [JsonProperty("activo")] public bool Activo { get; set; }
        [JsonProperty("acronimo")] public string Acronimo { get; set; }

    }
}
