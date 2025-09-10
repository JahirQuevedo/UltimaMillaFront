using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
//using NuGet.Protocol.Resources;

namespace ALOG.Modelos;

[Table("WMS_018_FOLIO_SERVICIO", Schema = "WMS")]
public class FolioServicio : BaseEntity
{
    [Key]
    [Column("nIdFolioServicio018")]
    public int Id { get; set; }

    [Column("nFolio")]
    public int Folio { get; set; }

    [Column("nEstado")]
    public EstadoFolioServicio? EstadoFolioServicio { get; set; }

    [Column("nServicioPara")]
    public int? ServicioPara { get; set; }

    [Column("dFechaProgramacion")]
    public DateTime? FechaProgramada { get; set; }

    [Column("sInstrucciones")]
    [MaxLength(4000)]
    public string? Instrucciones { get; set; }

    [Column("bFacturable")]
    public bool Facturable { get; set; } = false;

    [Column("nIdPaquete005")]
    public int? IdPaquete { get; set; }

    [Column("nIdReferencia001")]
    public int? IdReferencia { get; set; }

    [Column("nIdInventario014")]
    public int? IdInventario { get; set; }

    [Column("nIdTarja015")]
    public int? IdTarja { get; set; }

    [Column("nIdCatEmpresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdPaquete")]
    public virtual Paquete Paquete { get; set; }

    [ForeignKey("IdReferencia")]
    public virtual Referencia Referencia { get; set; }

    [ForeignKey("IdInventario")]
    public virtual Inventario Inventario { get; set; }

    [ForeignKey("IdTarja")]
    public virtual Tarja Tarja { get; set; }

    [ForeignKey("IdEmpresa")]
    public virtual CatEmpresas Empresa { get; set; }

}
