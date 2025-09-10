using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos;

[Table("WMS_029_CODIGO_DESPERFECTO", Schema = "WMS")]
public class CodigoDesperfecto : BaseEntity
{

    [Key]
    [Column("nIdCodigoDesperfecto029")]
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
