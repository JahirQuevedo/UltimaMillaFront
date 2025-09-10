using ALOG.Enums;


namespace ALOG.Modelos;

public class MonitorRecepcionMercancia : BaseEntity
{

    public int IdSolicitudTraslado { get; set; }

    public int FolioSolicitudTraslado { get; set; }

    public int IdControlTransporte { get; set; }

    public int FolioControlTransporte { get; set; }

    public int IdInventario { get; set; }

    public string IdMercancia { get; set; }

    public string Marcas { get; set; }

    public string Modelo { get; set; }

    public bool TieneAveria { get; set; }

    public string Buque { get; set; }

    public string FolioViaje { get; set; }

    public DateTime FechaProgramacion { get; set; }

    public DateTime FechaRecoleccion { get; set; }

    public DateTime FechaIngreso { get; set; }

    public bool ProcesoRecepcionCompletado { get; set; }

    public bool RecoleccionTarjaConfirmada { get; set; }

    public bool IngresoTarjaConfirmada { get; set; }

    public bool AplicaConfirmarMercancia { get; set; }

    public bool ConfirmaRecepcionMercancia { get; set; }

    public bool AplicaExtraccionPatioExterno { get; set; }

    public bool AplicaUbicacionMercancia { get; set; }

    public int IdTarja { get; set; }

    public int FolioTarja { get; set; }

    public TipoServicioTarja TipoServicioTarja { get; set; }

    public int FolioServicio { get; set; }

    public int IdFolioServicio { get; set; }

    public int IdPaquete { get; set; }

    public TipoEntradaPaquete TipoEntrada { get; set; }

    public string Mercancia { get; set; }

    public string LineaTransportista { get; set; }

    public string OperadorTransporte { get; set; }

    public string PlacasTransporte { get; set; }

    public List<MonitorBitacoraAveria> ListaMonitorBitacoraAveria { get; set; }

    public string DescripcionAveria { get; set; }


}
