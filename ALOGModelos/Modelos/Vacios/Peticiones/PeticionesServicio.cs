using ALOG.Modelos.Modelos.Catalogos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos.Modelos.Vacios.Peticiones
{
    public class PeticionesServicio
    {
        [Key]
        [JsonProperty("idServicio")] public int IdServicio { get; set; }

        [JsonProperty("descServicio")] public string DescServicio { get; set; }
        [JsonProperty("fechaRegistro")] public DateTime FechaRegistro { get; set; }
        [JsonProperty("fechaCierre")] public DateTime FechaCierre { get; set; }
        [JsonProperty("idContenedor")] public int IdContenedor { get; set; }
        [JsonProperty("documentos")] public List<PeticionesDocumentos> Documentos { get; set; }

        [ForeignKey("catServicios")]
        [JsonProperty("idTipoServicio")] public int IdTipoServicio { get; set; }
        public CatServicios catServicios { get; set; }

        [JsonProperty("estadoServicio")] public string EstadoServicio { get; set; }
        [JsonProperty("activo")] public bool Activo { get; set; } = true;

        [ForeignKey("catReferenciaEstado")]
        public int IdEstadoServicio { get; set; } = 1;
        public CatReferenciaEstado catReferenciaEstado { get; set; }
    }
}
