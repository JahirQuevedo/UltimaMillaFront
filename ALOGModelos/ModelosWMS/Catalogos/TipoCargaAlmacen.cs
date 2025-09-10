using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ALOG.Modelos;

[Table("WMS_007_TIPO_CARGA_ALMACEN", Schema = "WMS")]
public class TipoCargaAlmacen : BaseEntity
{

    [Key]
    [Column("nIdTipoCargaAlmacen007")]
    public int Id { get; set; }

    [Column("sClave")]
    [MaxLength(6)]
    public string Clave { get; set; }

    [Column("sDescripcion")]
    [MaxLength(30)]
    public string Descripcion { get; set; }

}
