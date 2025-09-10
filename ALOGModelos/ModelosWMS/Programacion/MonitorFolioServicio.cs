using ALOG.Enums;

namespace ALOG.Modelos;

public class MonitorFolioServicio : BaseEntity
{

    public int IdFolioServicio { get; set; }

    public int Folio { get; set; }

    public EstadoFolioServicio EstadoFolioServicio { get; set;}

    public int IdPaquete { get; set; }  

    public string DescripcionPaquete { get; set; }

    public TipoEntradaPaquete TipoIngreso { get; set; }

    public int ServicioPara { get; set; }

    public int IdServicio { get; set; }

    public bool AplicaServicioAgrupado { get; set; }

    public DateTime FechaProgramada { get; set; }

    public string Solicito { get; set; }

    public EstadoFacturacion EstadoFacturacion { get; set; }

    public EstadoFactura EstadoFactura { get; set; }

    public string SerieFactura { get; set; }

    public string Factura { get; set; }

    public int IdFacturarA { get; set; }

    public string NombreFacturarA { get; set; }

    public int IdMoneda { get; set; }

    public string DescripcionMoneda { get; set; }

    public int IdZonaAlmacenaje { get; set; }

    public string DescripcionZonaAlmacenaje { get; set; }

    public int IdCliente { get; set; }  

    public string RazonSocialCliente { get; set; }

    public string InstruccionesDelServicio { get; set; }

    public DateTime FechaRevisado { get; set; }

    public int IdReferencia { get; set; }

    public string FolioReferencia { get; set; }

    public int IdTarja  { get; set; }

    public int FolioTarja { get; set; }

    public int IdInventario { get; set; }

    public int NumeroPartida { get; set; }

    public string FolioTarjaPartida { get; set; }

    public List<int> ListIdMonitorTarja { get; set; }

    public List<int> ListaIdPartidaInventario { get; set; }

    public List<RelacionInventarioServicio> ListaRelacionInventarioServicio { get; set; }

}
