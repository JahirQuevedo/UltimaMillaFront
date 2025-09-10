namespace ALOG.Modelos;

public class MonitorBitacoraAveria : BaseEntity
{

    public int IdBitacoraAveria { get; set; }

    public int IdInventario { get; set; }

    public int IdCodigoDesperfecto { get; set; }

    public string ClaveCodigoDesperfecto { get; set; }

    public string DescripcionCodgioDesperfecto { get; set; }

    public int IdTipoDesperfecto { get; set; }

    public string ClaveTipoDesperfecto {get; set; }

    public string DescripcionTipoDesperfecto { get; set; }

    public int IdTipoSeveridad { get; set; }

    public string ClaveTipoSeveridad { get; set; }

    public string DescripcionTipoSeveridad { get; set; }

    public string DescripcionAveria { get; set; }

}
