using ALOG.Enums;

namespace ALOG.Modelos;

public class MonitorReferencia : BaseEntity
{

    public int IdReferencia { get; set; }

    public string Folio { get; set; }

    public TipoOperacionAduanera TipoOperacion { get;  set; }

    public EstadoReferencia EstadoReferencia { get; set; }

    public TipoMercanciaInventario TipoMercancia {get; set; }

    public DateTime? FechaEntrada { get; set; }

    public int IdViaje { get; set; }

    public int ReferenciaBuque { get; set; }

    public string FolioViaje { get; set; }

    public string ManifiestoBuque { get; set; }

    public string Cliente { get; set; }

    public string FacturarA { get; set; }

    public string ProveedorDest { get; set; }

    public int IdCliente { get; set; }

    public string Mercancias { get; set; }

    public int BultosInical { get; set; }

    public decimal PesoInicial { get; set; }

    public int BultosFinal { get; set; }

    public decimal PesoFinal { get; set; }

    public string Observaciones { get; set; } 

}
