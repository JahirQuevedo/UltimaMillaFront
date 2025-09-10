using ALOG.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ALOG.Modelos;

public class ConsultaViaje : BaseEntity
{
    public ConsultaViaje()
    {

    }

    public int IdViaje {get; set;}

    public string Folio { get; set; }

    public TipoFechaConsultaViaje TipoFecha { get; set; }

    /// <summary>
    /// Parametro Fecha de inicio del filtro para viajes
    /// </summary>
    public DateTime? FechaInicio { get; set; }

    /// <summary>
    /// Parametro Fecha de final del filtro para viajes
    /// </summary>
    public DateTime? FechaFin { get; set; }

}
