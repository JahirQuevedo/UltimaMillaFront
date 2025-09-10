using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos;

[Table("WMS_009_TIPO_ZONA_ALMACENAJE", Schema = "WMS")]
public class TipoZonaAlmacenaje : BaseEntity
{
    [Key]
    [Column("nIdTipoZonaAlmacenaje009")]
    public int Id { get; set; }

    [Column("sDescripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }
}
