using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
//using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ALOG.Modelos;

[Table("WMS_014_INVENTARIO", Schema = "WMS")]
public class Inventario : BaseEntity
{
    [Key]
    [Column("nIdInventario014")]
    public int Id { get; set; }

    [Column("nEstado")]
    public EstadoInventario Estado { get; set; }

    [Column("sMercancia")]
    [MaxLength(4000)]
    public string Mercancia { get; set; }

    [Column("nIdTipoEmbalaje013")]
    public int? IdTipoEmbalaje { get; set; }

    [Column("nIdUnidadMediad012")]
    public int? IdUnidadMedida { get; set; }

    [Column("nCantidadInicial")]
    public int? CantidadInicial { get; set; }

    [Column("nPesoInicial")]
    public decimal? PesoInicial { get; set; }

    [Column("nCantidadFinal")]
    public int? CantidadFinal { get; set; }

    [Column("nPesoFinal")]
    public decimal? PesoFinal { get; set; }

    [Column("bExistencia")]
    public bool Existencia { get; set; } = false;

    [Column("sObservaciones")]
    [MaxLength(4000)]
    public string? Observaciones { get; set; }

    [Column("bProcesoRecepcionCompletado")]
    public bool ProcesoRecepcionCompletado { get; set; }

    [Column("dFechaRecoleccion")]
    public DateTime? FechaRecoleccion { get; set; }

    [Column("dFechaIngreso")]
    public DateTime? FechaIngreso { get; set; }

    [Column("dFechaEmbarque")]
    public DateTime? FechaEmbarque { get; set; }

    [Column("dFechaVerificacion")]
    public DateTime? FechaVerificacion { get; set; }

    [Column("dFechaSalida")]
    public DateTime? FechaSalida { get; set; }

    [Column("nIdUbicacion011")]
    public int? IdUbicacion { get; set; }

    [Column("sGrupo")]
    public string? Grupo { get; set; }

    [ForeignKey("IdTipoEmbalaje")]
    public virtual TipoEmbalaje TipoEmbalaje { get; set; }

    [ForeignKey("IdUbicacion")]
    [MaxLength(70)]
    public virtual Ubicacion Ubicacion { get; set; }

    [ForeignKey("IdUnidadMedida")]
    public virtual UnidadMedida UnidadMedida { get; set; }
}
