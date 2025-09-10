using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.Vacios.Peticiones {
    public class RootObjectReferencia {
        [JsonProperty("$values")] public List<PeticionesReferencias> values { get; set; }
}
}
