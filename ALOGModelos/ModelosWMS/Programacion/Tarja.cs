using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos;

[Table("WMS_015_TARJA", Schema = "WMS")]
public class Tarja : BaseEntity
{

    [Key]
    [Column("nIdTarja015")]
    public int Id { get; set; }

    [Column("nFolio")]
    public int Folio { get; set; }

    [Column("nTipo")]
    public TipoTarja TipoTarja { get; set; }

    [Column("nEstado")]
    public EstadoTarja Estado { get; set; }

    [Column("sObservaciones")]
    [MaxLength(4000)]
    public string? Observaciones { get; set; }

    [Column("nTipoServicio")]
    public TipoServicioTarja? TipoServicio { get; set; }

    [Column("nFolioIngreso")]
    public int? FolioIngreso { get; set; }

    [Column("dFechaIngreso")]
    public DateTime? FechaIngreso { get; set; }

    [Column("nMedioEntrada")]
    public MedioEntrada? MedioEntrada { get; set; }

    [Column("nIdCatEmpresa")]
    public int IdEmpresa { get; set; }

    [Column("nIdReferencia001")]
    public int? IdReferencia { get; set; }

    [ForeignKey("IdReferencia")]
    public virtual Referencia Referencia { get; set; }

    [ForeignKey("IdEmpresa")]
    public virtual CatEmpresas Empresa { get; set; }

}
