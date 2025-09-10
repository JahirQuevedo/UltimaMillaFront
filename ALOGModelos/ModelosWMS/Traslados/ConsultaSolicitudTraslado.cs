using ALOG.Enums;

namespace ALOG.Modelos;

public class ConsultaSolicitudTraslado : BaseEntity
{

    public int FolioSolicitudTraslado { get; set; }

    public EstadoTraslado EstadoSolicitudTraslado { get; set; }

    public DateTime FechaInicio { get; set; } = DateTime.Now.AddDays(-7);

    public DateTime FechaFin { get; set; } = DateTime.Now;

    public int IdControlTransporte { get; set; }

    public int FolioControlTransporte {get; set; }

}
