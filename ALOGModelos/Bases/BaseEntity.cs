using ALOG.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos;

public class BaseEntity
{

    [NotMapped]
    public int? IdPermisoAuditoria { get; set; } = 0;

    [NotMapped]
    public int? IdEmpresaLogin { get; set; } = 0;

    [NotMapped]
    public ECRUDAction CRUDAction { get; set; } = ECRUDAction.Read;

    [Column("dFechaAlta")]
    public DateTime? FechaAlta { get; set; } = DateTime.Now;

}
