using ALOG.Enums;

namespace ALOG.Modelos;

public class ConsultaCtrlTransporte : BaseEntity
{

    public int IdControlTransporte { get; set; }

    public string FolioTurno { get; set; }

    public string NombreEstadoTurno { get; set; }

    public EstadoTurno EstadoTurno { get; set; }

    public DateTime FechaInicio { get; set; } = DateTime.Now.AddDays(-7);

    public DateTime FechaFin { get; set; } = DateTime.Now;

}
