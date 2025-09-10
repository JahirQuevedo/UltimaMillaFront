using ALOG.Enums;

namespace ALOG.Modelos;

public class MonitorCtrlTransporte : BaseEntity
{

    public int IdControlTransporte { get; set; }

    public int FolioTurno { get; set; }

    public EstadoTurno Estado { get; set; }

    public int IdManiobristaOrigen { get; set; }

    public string ManiobristaOrigen { get; set; }

    public string ManiobristaNombreCortoOrigen { get; set; }

    public int IdManiobristaDestino { get; set; }

    public string ManiobristaDestino { get; set; }

    public string ManiobristaNombreCortoDestino { get; set; }

    public TipoViaje TipoViaje { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin  { get; set; }

    public DateTime FechaCancelacion { get; set; }

    public int IdLineaTransTransporte { get; set; }

    public int IdCatTransportista { get; set; }

    public string RFCLineaTransporte { get; set; }

    public string RazonSocialLineaTransporte { get; set; }

    public string PlacasLineaTransporte { get; set;  }

    public string NumEcoLineaTransporte { get; set; }

    public int IdLineaTransOperador { get; set; }

    public string NombreOperadorLineaTransporte { get; set; }

    public int IdTipoTransporte { get; set; }

    public string TipoTransporte { get; set; }

    public string Observaciones { get; set; }

    public bool TieneTrasladosAsignado { get; set; }

    public List<MonitorSolicitudTraslado> listaSolicitudTraslado { get; set; } 

}
