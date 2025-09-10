using ALOG.Enums;

namespace ALOG.Modelos;

public class MonitorTarja : BaseEntity
{

    public int IdTarja { get; set; }

    public int Folio { get; set; }  

    public int IdReferencia { get; set; }

    public string FolioReferencia { get; set; }

    public TipoTarja TipoTarja{ get; set; }

    public EstadoTarja Estado { get; set; } 

    public TipoServicioTarja TipoServicio { get; set; }

    public int CantidadServicios { get; set; }

    public int NumeroPartidas { get; set; }

    public decimal TotalPeso { get; set; }

    public decimal TotalPesoSaldo { get; set; }

    public string ZonaAlmacen { get; set; }

    public string Observaciones { get; set; }

    public DateTime FechaIngreso { get; set; }

    public bool EsTrasladoAsignado { get; set; }

    public List<MonitorPartida> listaMonitorPartidas{ get; set; }

}
