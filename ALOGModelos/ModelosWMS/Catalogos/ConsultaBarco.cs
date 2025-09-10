using ALOG.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ALOG.Modelos;

public class ConsultaBarco : BaseEntity
{
    public ConsultaBarco() {

    }

    public string Nombre { get; set; }
}
