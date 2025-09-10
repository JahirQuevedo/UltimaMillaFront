using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos;

[Table("WMS_002_VIAJE", Schema = "WMS")]
public class Viaje : BaseEntity
{
    [Key]
    [Column("nIdViaje002")]
    public int Id { get; set; }

    [Column("sFolio")]
    public required string Folio { get; set; }

    [Column("nIdCatEmpresa")]
    public int IdEmpresa { get; set; }

    [Column("nTipoOperacion")]
    public TipoOperacionAduanera TipoOperacion { get; set; }

    [Column("dFechaArriboSalida")]
    public DateTime? FechaArriboSalida { get; set; }

    [Column("dFechaFondeo")]
    public DateTime? FechaFondeo { get; set; }

    [Column("dFechaAtraque")]
    public DateTime? FechaAtraque { get; set; }

    [Column("dFechaInicioCargaDescarga")]
    public DateTime? FechaInicioCargaDescarga { get; set; }

    [Column("dFechaDeatraque")]
    public DateTime? FechaDesatraque { get; set; }

    [Column("dFechaFinCargaDescarga")]
    public DateTime? FechaFinCargaDescarga { get; set; }

    [Column("nPedoBls")]
    public decimal? PesoBLs { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

    [Column("nIdBarco003")]
    public int? IdBarco { get; set; }

    [Column("bExterior")]
    public bool Exterior { get; set; }

    [Column("nReferenciaBuque")]
    public int? ReferenciaBuque { get; set; }

    [ForeignKey("IdEmpresa")]
    public virtual CatEmpresas Empresa { get; set; }

    [ForeignKey("IdBarco")]
    public virtual Barco Barco { get; set; }

}
