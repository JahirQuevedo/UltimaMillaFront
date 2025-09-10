using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos;


[Table("WMS_017_INVENTARIO_ALO", Schema = "WMS")]
public class InventarioALO : BaseEntity
{

    [Key]
    [Column("nIdInventarioALO017")]
    public int Id { get; set; }

    [Column("nTipoMercancia")]
    public TipoMercanciaInventario? TipoMercancia { get; set; }

    [Column("nIdInventario014")]
    public int? IdInventario { get; set; }

    [Column("nIdReferencia001")]
    public int? IdReferencia { get; set; }

    [Column("nIdReferenciaOrigen001")]
    public int? IdReferenciaOrigen { get; set; }

    [ForeignKey("IdInventario")]
    public virtual Inventario Inventario { get; set; }

    [ForeignKey("IdReferencia")]
    public virtual Referencia Referencia { get; set; }

    [ForeignKey("IdReferenciaOrigen")]
    public virtual Referencia ReferenciaOrigen { get; set; }
}
