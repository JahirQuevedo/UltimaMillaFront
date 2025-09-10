namespace ALOG.Modelos.Modelos.DTO.Consultas
{
    public class FiltroOrdenesReferenciasDTO
    {
        public int? IdOrden { get; set; }
        public int? IdAduana { get; set; }
        public int? IdLNegocio { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdNaviera { get; set; }
        public int? IdPatio { get; set; }
        public int? IdCliente { get; set; }
        public int? IdTransporte { get; set; }
        public int? Ticket { get; set; }

        public int? IdServicio { get; set; }
        public string Contenedor { get; set; }
        public string ReferenciaALO { get; set; }
        public string ReferenciaCliente { get; set; }
        public string Buque { get; set; }
        public string Ejecutivo { get; set; }

        public DateTime FechaSolicitudIni { get; set; }
        public DateTime FechaSolicitudFin { get; set; }
        public DateTime FechaCierreCont { get; set; }

        public int IdCatEstadoContenedor { get; set; }
        public int IdCatEstadoReferencia { get; set; }

    }
}
