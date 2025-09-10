using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.Dtos.Respuestas
{
    public class RespObtenerAcarreosDTO
    {
        [JsonProperty("idDtAcarreos")] public int IdDtAcarreos { get; set; }

        [JsonProperty("mes")] public int Mes { get; set; }



        [JsonProperty("fecha")] public DateTime Fecha { get; set; }

        [JsonProperty("fechaRegistro")] public DateTime FechaRegistro { get; set; }
        [JsonProperty("nombreServicio")] public string NombreServicio { get; set; }

        [JsonProperty("numeroContenedor")] public string NumeroContenedor { get; set; }
        [JsonProperty("idCliente")] public int IdCliente { get; set; } = 0;

        [JsonProperty("idCatTipoEstado")] public int IdCatTipoEstado { get; set; }
        [JsonProperty("tipoEstado")] public string TipoEstado { get; set; }
        [JsonProperty("nombreCliente")] public string NombreCliente { get; set; }
        [JsonProperty("idEmpresa")] public int IdEmpresa { get; set; }
        [JsonProperty("empresaRazonSocial")] public string EmpresaRazonSocial { get; set; }

        [JsonProperty("idCatAduana")] public int IdCatAduana { get; set; }
        [JsonProperty("nombreAduana")] public string NombreAduana { get; set; }
        [JsonProperty("idOrden")] public int IdOrden { get; set; }
        [JsonProperty("activo")] public bool Activo { get; set; }

        [JsonProperty("idCatServicio")] public int IdCatServicio { get; set; }
        [JsonProperty("idUsuarioRegistro")] public int IdUsarioRegistro { get; set; }
        public int IdCatProveedor { get; set; }
        [JsonProperty("usuario")] public string Usuario { get; set; }
    }
}
