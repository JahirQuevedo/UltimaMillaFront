//using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ALOG.Modelos;

[Table("WMS_016_PARTIDA", Schema = "WMS")]
public class Partida : BaseEntity
{
    [Key]
    [Column("nIdPartida016")]
    public int Id { get; set; }

    [Column("nNumeroPartida")]
    public int NumeroPartida { get; set; }

    [Column("sMarcas")]
    [MaxLength(255)]
    public string? Marcas { get; set; }

    [Column("sNumeros")]
    [MaxLength(200)]
    public string? Numeros { get; set; }

    [Column("sModelo")]
    [MaxLength(255)]
    public string? Modelo { get; set; }

    [Column("bTieneAveria")]
    public bool TieneAveria { get; set; }

    [Column("sDescripcionAveria")]
    [MaxLength(4000)]
    public string? DescripcionAveria { get; set; }

    [Column("sBLHouse")]
    [MaxLength(50)]
    public string? BLHouse { get; set; }

    [Column("bCargaMasiva")]
    public bool CargaMasiva { get; set; } = false;

    [Column("nIdInventario014")]
    public int? IdInventario { get; set; }

    [Column("nIdInventarioOrigen014")]
    public int? IdInventarioOrigen { get; set; }

    [Column("nIdTarja015")]
    public int? IdTarja { get; set; }

    [Column("nIdTarjaOrigen015")]
    public int? IdTarjaOrigen { get; set; }

    [ForeignKey("IdInventario")]
    public virtual Inventario Inventario { get; set; }

    [ForeignKey("IdInventarioOrigen")]
    public virtual Inventario InventarioOrigen { get; set; }

    [ForeignKey("IdTarja")]
    public virtual Tarja Tarja { get; set; }

    [ForeignKey("IdTarjaOrigen")]
    public virtual Tarja TarjaOrigen { get; set; }


}
