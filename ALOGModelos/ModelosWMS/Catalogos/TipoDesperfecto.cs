using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ALOG.Modelos;

[Table("WMS_030_TIPO_DESPERFECTO", Schema = "WMS")]
public class TipoDesperfecto : BaseEntity
{

    [Key]
    [Column("nIdTipoDesperfecto030")]
    public int Id { get; set; }

    [Column("sClave")]
    [MaxLength(10)]
    public string Clave { get; set; }

    [Column("sDescripcion")]
    [MaxLength(250)]
    public string Descripcion { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

}
