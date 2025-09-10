using Newtonsoft.Json;

namespace ALOG.Modelos.Modelos.FiltrosBusqueda
{
    public class FiltroDtAcarreosDTO
    {
        public DateTime? FechaInicial { get; set; }

        [JsonProperty("servicio")] public string Servicio { get; set; } = string.Empty;
        [JsonProperty("contenedor")] public string Contenedor { get; set; } = string.Empty;
        [JsonProperty("cliente")] public string Cliente { get; set; } = string.Empty;
        public int? IdCliente { get; set; }
        public int? IdCatTipoEstados { get; set; }
        public int? IdCatProveedor { get; set; }

        public int? IdEmpresa { get; set; }

        public int? IdOrden { get; set; }
        public int NumeroPagina { get; set; } = 1;
        public int NumeroRegistros { get; set; } = 10;
        public bool? Activo { get; set; }

        public DateTime FSolicitudIni { get; set; }
        public DateTime FSolicitudFin { get; set; }
        public DateTime FEntregaIni { get; set; }
        public DateTime FEntregaFin { get; set; }
        public int Mes { get; set; }
    }
}
