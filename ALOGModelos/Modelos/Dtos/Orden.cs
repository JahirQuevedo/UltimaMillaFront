using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Vacios.Peticiones;
using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.Dtos
{
    public class Orden
    {
        [JsonProperty("idOrden")] public int IdOrden { get; set; } = 0;
        [JsonProperty("idCatCliente")] public int IdCatCliente { get; set; } = 0;
        public CatClientes catClientes { get; set; }


        [JsonProperty("idCatSistema")] public int IdCatSistema { get; set; } = 0;
        [JsonProperty("idCatAduana")] public int IdCatAduana { get; set; } = 0;
        public CatAduana catAduana { get; set; }
        [JsonProperty("idCatEmpresa")] public int IdCatEmpresa { get; set; } = 0;
        public CatEmpresas catEmpresas { get; set; }
        [JsonProperty("idCatSucursal")] public int IdCatSucursal { get; set; } = 0;
        public CatSucursales catSucursales { get; set; }
        [JsonProperty("idCatLineaNegocio")] public int IdCatLineaNegocio { get; set; } = 0;
        public CatLineaNegocio CatLineaNegocio { get; set; }
        [JsonProperty("idCatProyecto")] public int? IdCatProyecto { get; set; }
        [JsonProperty("idUsuario")] public int IdUsuario { get; set; }
        public CatUsuarios catUsuario { get; set; }
        [JsonProperty("fechaRegistro")] public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [JsonProperty("activo")] public bool Activo { get; set; } = true;
        [JsonProperty("ReferenciaALO")] public string ReferenciaAlo { get; set; } = string.Empty;
        public List<PeticionesReferencias> peticionesReferencias { get; set; }
        public List<DtAcarreos> dtAcarreos { get; set; }
        public List<DtUltimaMillaEnc> dtUltimaMillaEnc { get; set; }





    }
}
