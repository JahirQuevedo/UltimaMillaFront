using ALOG.Modelos.Modelos.Vacios.Peticiones;

using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.Dtos.Respuestas
{
    public class RespObtenerReferenciasDTO
    {
        [JsonProperty("idReferencia")] public int IdReferencia { get; set; } = 0;
        [JsonProperty("ticket")] public int Ticket { get; set; } = 0;
        [JsonProperty("transporte_RFC")] public string Transporte_RFC { get; set; }
        [JsonProperty("transporte_RazonSocial")] public string Transporte_RazonSocial { get; set; }
        [JsonProperty("trasporteId")] public int TrasporteId { get; set; } = 0;
        [JsonProperty("transporte_Usuario")] public string Transporte_Usuario { get; set; }
        [JsonProperty("transporte_UsuarioEmail")] public string Transporte_UsuarioEmail { get; set; }
        [JsonProperty("comentarios")] public string Comentarios { get; set; }
        [JsonProperty("tipoReferencia")] public int TipoReferencia { get; set; }

        [JsonProperty("procesado")] public string Procesado { get; set; }
        [JsonProperty("fechaProcesado")] public DateTime FechaProcesado { get; set; }
        [JsonProperty("fechaSolicitud")] public DateTime FechaSolicitud { get; set; }
        [JsonProperty("estadoReferencia")] public string EstadoReferencia { get; set; }
        [JsonProperty("idCatReferenciaEstado")] public int IdCatReferenciaEstado { get; set; }
        [JsonProperty("activo")] public bool Activo { get; set; }
        [JsonProperty("idOrden")] public int IdOrden { get; set; }

        [JsonProperty("idCatcliente")] public int IdCatcliente { get; set; }
        [JsonProperty("razonSocialCliente")] public string RazonSocialCliente { get; set; }
        [JsonProperty("idCatProveedor")] public int IdCatProveedor { get; set; }
        [JsonProperty("razonSocialProveedor")] public string RazonSocialProveedor { get; set; }
        [JsonProperty("idCatAduana")] public int IdCatAduana { get; set; }
        [JsonProperty("aduana")] public string Aduana { get; set; }
        [JsonProperty("idCatSistema")] public int IdCatSistema { get; set; }
        [JsonProperty("sistema")] public string Sistema { get; set; }

        [JsonProperty("idCatEmpresa")] public int IdCatEmpresa { get; set; }
        [JsonProperty("razonSocialEmpresa")] public string RazonSocialEmpresa { get; set; }

        [JsonProperty("idCatsucursal")] public int IdCatsucursal { get; set; }
        [JsonProperty("sucursal")] public string Sucursal { get; set; }
        [JsonProperty("idCatTransporte")] public int IdCatTransporte { get; set; }

        [JsonProperty("idLNegocio")] public int IdLNegocio { get; set; }
        [JsonProperty("lineaNegocio")] public string LineaNegocio { get; set; }

        [JsonProperty("idUsuario")] public int IdUsuario { get; set; }
        [JsonProperty("usuario")] public string Usuario { get; set; }
        [JsonProperty("idCatEstadoOrden")] public int IdCatEstadoOrden { get; set; }
        [JsonProperty("estadoOrden")] public int EstadoOrden { get; set; }

        [JsonProperty("idCatEstadoReferencia")] public int IdCatEstadoReferencia { get; set; }
        [JsonProperty("activoOrden")] public bool ActivoOrden { get; set; }
        [JsonProperty("fechaRegistroOrden")] public DateTime FechaRegistroOrden { get; set; }
        [JsonProperty("fechaRegistroReferencia")] public DateTime FechaRegistroReferencia { get; set; }
        [JsonProperty("fechaCierreOrden")] public DateTime FechaCierreOrden { get; set; }
        [JsonProperty("fechaCierreReferencia")] public DateTime FechaCierreReferencia { get; set; }

        [JsonProperty("referenciaALO")] public string ReferenciaALO { get; set; }
        [JsonProperty("referenciaCliente")] public string ReferenciaCliente { get; set; }
        [JsonProperty("peticionesContenedores")] public List<PeticionesContenedor> PeticionesContenedores { get; set; }


    }
}
