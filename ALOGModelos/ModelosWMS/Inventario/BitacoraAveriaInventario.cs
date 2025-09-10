using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.AspNetCore.Mvc;

namespace ALOG.Modelos;

[Table("WMS_032_BITACORA_AVERIA", Schema = "WMS")]
public class BitacoraAveriaInventario
{
    [Key]
    [Column("nIdBitacoraAveria032")]
    public int Id { get; set; }

    [Column("nIdInventario014")]
    public int IdInventario { get; set; }

    [Column("nIdCodigoDesperfecto029")]
    public int IdCodigoDesperfecto { get; set; }

    [Column("nIdTipoDesperfecto030")]
    public int IdTipoDesperfecto { get; set; }

    [Column("nIdTipoSeveridad031")]
    public int IdTipoSeveridad { get; set; }

    [Column("sDescripcionAveria")]
    [MaxLength(4000)]
    public string? DescripcionAveria { get; set; }

    [ForeignKey("IdInventario")]
    public virtual Inventario Inventario { get; set; }

    [ForeignKey("IdCodigoDesperfecto")]
    public virtual CodigoDesperfecto CodigoDesperfecto { get; set; }

    [ForeignKey("IdTipoDesperfecto")]
    public virtual TipoDesperfecto TipoDesperfecto { get; set; }

    [ForeignKey("IdTipoSeveridad")]
    public virtual TipoSeveridad TipoSeveridad { get; set; }
}
