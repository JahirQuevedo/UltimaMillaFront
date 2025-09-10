using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
//using NuGet.Protocol.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ALOG.Modelos;

[Table("WMS_025_FOLIO_SERVICIO_MASTER_DETALLE", Schema = "WMS")]
public class FolioServicioMasterDetalle : BaseEntity
{

    [Key]
    [Column("nIdFolioServicioMasterDetalle025")]

    public int Id { get; set; }

    [Column("nIdReferencia001")]
    public int? IdReferencia { get; set; }

    [Column("nIdTarja015")]
    public int? nIdTarja { get; set; }

    [Column("nIdInventario014")]
    public int? nIdInventario { get; set; }

    [Column("nServicioPara")]
    public int ServicioPara { get; set; }

    [ForeignKey("IdReferencia")]
    public virtual Referencia Referencia { get; set; }

    [ForeignKey("IdTarja")]
    public virtual Tarja Tarja { get; set; }

    [ForeignKey("IdInventario")]
    public virtual Inventario Inventario { get; set; }

}
