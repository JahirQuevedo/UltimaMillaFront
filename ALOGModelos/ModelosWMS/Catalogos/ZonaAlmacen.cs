using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos;

[Table("WMS_010_ZONA_ALMACENAJE", Schema = "WMS")]
public class ZonaAlmacen : BaseEntity
{
    [Key]
    [Column("nIdZonaAlmacenaje010")]
    public int Id { get; set; }

    [Column("sClave")]
    [MaxLength(7)]
    public string Clave { get; set; }

    [Column("sDescripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; }

    [Column("nTotalFilas")]
    public int TotalFilas { get; set; }

    [Column("nTotalColumnas")]
    public int TotalColumnas { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

    [Column("nIdTipoZonaAlmacenaje009")]
    public int IdTipoZonaAlmacenaje { get; set; }

    [Column("nIdAlmacen008")]
    public int nIdAlmacen { get; set; }

    [ForeignKey("IdTipoZonaAlmacenaje")]
    public virtual TipoZonaAlmacenaje TipoZonaAlmacenaje { get; set; }

    [ForeignKey("nIdAlmacen")]
    public virtual Almacen Almacen { get; set; }

}
