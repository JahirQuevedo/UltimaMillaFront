using ALOG.Enums;
//using NuGet.Common;

namespace ALOG.Modelos;

public class ConsultaReferencia : BaseEntity
{
    public int IdReferencia { get; set; }

    public string Folio { get; set; }

    public EstadoReferencia Estado { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

}
