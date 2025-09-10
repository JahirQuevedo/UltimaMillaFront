using ALOG.Enums;

namespace ALOG.Modelos;

public class MonitorSolicitudTraslado : BaseEntity
{

    public int IdSolicitudTraslado { get; set; }

    public int FolioTraslado { get; set; }

    public EstadoTraslado EstadoTraslado { get; set; }

    public DateTime FechaSolicitudTraslado { get; set; }

    public string BoletaLiberacion { get; set; }

    public string DescripcionMercancia { get; set; }

    public string TipoMercancia { get; set; }

    public string BLBooking { get; set;}

    public int TipoOperacion { get; set; }

    public int Cantidad { get; set; }

    public decimal Peso { get; set;}

    public int Prioridad { get; set; }

    public int IdReferencia { get; set; }

    public string FolioReferencia { get; set; }

    public int IdTarja { get; set; }

    public int FolioTarja {get; set;}

    public int FolioServicio { get; set; }

    public int IdFolioServicio { get; set; }

    public EstadoFolioServicio EstadoFolioServicio { get; set; } 

    public string DescripcionFolioServicio { get; set;}

    public int IdControlTransporte { get; set; }

    public int FolioControlTransporte { get; set; }

    public string Cliente { get; set; }

    public int IdManiobristaOrigen { get; set; }

    public string ManiobristaOrigen { get; set; }

    public string ManiobristaNombreCortoOrigen { get; set; }

    public int IdManiobristaDestino { get; set; }

    public string ManiobristaDestino { get; set; }

    public string ManiobristaNombreCortoDestino { get; set; }

    public string Observaciones { get; set; }

    public List<RelacionInventarioServicio> ListaInventarioServicios{ get; set; }


}
