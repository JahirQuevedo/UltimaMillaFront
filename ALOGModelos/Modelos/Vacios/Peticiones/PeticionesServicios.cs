using ALOGModelos.Modelos.Catalogos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOGModelos.Modelos.Vacios.Peticiones
{
    public class PeticionesServicios
    {
        [Key]
        [JsonProperty("idServicio")] public int IdServicio { get; set; } = 0;
        [ForeignKey("catServicios")]
        [Column("IdCatServicio")]
        [JsonProperty("idTipoServicio")] public int IdTipoServicio { get; set; } = 0;
        public CatServicios catServicios { get; set; }
        [JsonProperty("descServicio")] public string DescServicio { get; set; } = string.Empty;
        [JsonProperty("fechaRegistro")] public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [JsonProperty("fechaCierre")] public DateTime FechaCierre { get; set; } = DateTime.Now;
        [ForeignKey("PeticionesContenedores")]
        [JsonProperty("idContenedor")] public int IdContenedor { get; set; } = 0;
        public PeticionesContenedor PeticionesContenedores { get; set; }
        [JsonProperty("documentos")] public List<CatDocumentos> Documentos { get; set; } = new List<CatDocumentos>();
        [JsonProperty("estadoServicio")] public string EstadoServicio { get; set; } = string.Empty;
        [ForeignKey("catReferenciaEstado")]
        public int IdEstadoServicio { get; set; }
        public CatReferenciaEstado catReferenciaEstado { get; set; }

        [JsonProperty("activo")] public bool Activo { get; set; } = true;
        [ForeignKey("catClientesFacturarA")]
        public int? IdClienteFacturarA { get; set; }
        public CatClientes catClientesFacturarA { get; set; }
    }
}
