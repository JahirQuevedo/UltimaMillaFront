namespace ALOG.Modelos;

public class MonitorLineaTransporte : BaseEntity
{

    public int IdCatTransportista { get; set; }
    
    public int IdLineaTransporte { get; set; }

    public string RazonSocialLineaTransporte { get; set; }

    public string RFC { get; set; }

    public string Placas { get; set; }

    public string NumeroEconomico { get; set; }

    public string PlacasPlana1 { get; set; }

    public string PlacasPlana2 { get; set; }

}
