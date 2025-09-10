namespace ALOG.Modelos;

public class ReporteDetalleTarjaRecepcion : BaseEntity
{

    public int NumeroRegistro { get; set; }

    public string VIN { get; set; }

    public string Modelo { get; set; }

    public string TipoDannioOrigen { get; set; }

    public string TipoDannioTransporte { get; set; }

    public string Comentarios { get; set; }

    public string Economico { get; set; }

    public string Placa { get; set; }

    public string Operador { get; set; }

}
