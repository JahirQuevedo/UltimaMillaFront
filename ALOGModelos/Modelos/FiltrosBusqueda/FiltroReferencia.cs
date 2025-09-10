using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.FiltrosBusqueda
{
    public class FiltroReferencia
    {
        public int IdOrden { get; set; }
        public int IdAduana { get; set; }
        public int IdLNegocio { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCliente { get; set; }
        public int IdServicio { get; set; }
        public string Contenedor { get; set; }
        public bool Activo { get; set; }
        public string Buque { get; set; }
        public string Ejecutivo { get; set; }
        public int Ticket { get; set; }
        public DateTime FechaCierreCont { get; set; }
        public int IdCatEstadoContenedor { get; set; }
        public int IdCatEstadoReferencia { get; set; }
        public DateTime FechaSolicitudIni { get; set; }
        public DateTime FechaSolicitudFin { get; set; }

        [JsonProperty("referenciaALO")] public string ReferenciaAlo { get; set; }
        [JsonProperty("referenciaCliente")] public string ReferenciaCliente { get; set; }
    }
}
