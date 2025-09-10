namespace ALOG.Modelos.Modelos.FiltrosBusqueda
{
    public class UltimaMillaEncabezadoFiltro
    {
        public int IdDtUltimaMillaDet { get; set; }
        public string NumeroParte { get; set; }
        public int Piezas { get; set; }
        public int Pallet { get; set; }
        public int IdDtUltMillaEnc { get; set; }
        public DateTime FechaSolicitudIni { get; set; }
        public DateTime FechaSolicitudFin { get; set; }
        public int Viaje { get; set; }
        public int IdCliente { get; set; }
        public string Cliente { get; set; }
        public string Factura { get; set; }
        public string FacturaCliente { get; set; }
        public string Bodega { get; set; }
        public int IdCatEmpresa { get; set; }
        public DateTime FechaSalidaIni { get; set; }
        public DateTime FechaSalidaFin { get; set; }
        public int IdTipoEstado { get; set; }
        public int IdOrden { get; set; }
        public bool Activo { get; set; }
        public int NumeroPagina { get; set; } = 1;
        public int NumeroRegistros { get; set; } = 10;

        public int? IdCatProveedor { get; set; }
    }
}

