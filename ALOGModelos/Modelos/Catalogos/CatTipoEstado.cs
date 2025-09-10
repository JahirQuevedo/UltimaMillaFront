using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Catalogos {
    public class CatTipoEstado {
       
        [JsonProperty("idCatTipoEstados")] public int Id { get; set; } = 0;
        [JsonProperty("nombre")] public string Nombre { get; set; } = string.Empty;
        [JsonProperty("tipoEstado")] public string Tipo { get; set; } = string.Empty;
        [JsonProperty("activo")] public bool Activo { get; set; } = true;
        [JsonProperty("fechaRegistro")] public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
