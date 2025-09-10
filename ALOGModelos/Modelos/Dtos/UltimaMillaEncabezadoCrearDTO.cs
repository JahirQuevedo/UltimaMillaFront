using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.Logisticos;
using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.Dtos
{
    public class UltimaMillaEncabezadoCrearDTO
    {

        [JsonProperty("idDtUltMillaEnc")] public int Id { get; set; }
        [JsonProperty("fechaSolicitud")] public DateTime FechaSolicitud { get; set; }
        [JsonProperty("viaje")] public int Viaje { get; set; }
        [JsonProperty("idCliente")] public int IdCliente { get; set; }
        [JsonProperty("cliente")] public string NombreCliente { get; set; }
        [JsonProperty("facturaCliente")] public string FacturaCliente { get; set; }
        [JsonProperty("bodega")] public string Bodega { get; set; }
        [JsonProperty("idCatEmpresa")] public int IdEmpresa { get; set; }
        [JsonProperty("fechaSalida")] public DateTime FechaSalida { get; set; }
        [JsonProperty("idTipoEstado")] public int IdTipoEstado { get; set; }
        [JsonProperty("idOrden")] public int IdOrden { get; set; }
        [JsonProperty("idCatServicio")] public int IdServicio { get; set; }
        [JsonProperty("dtUltimaMillaDets")] public ICollection<DtUltimaMillaDet> Detalles { get; set; }

    }
}
