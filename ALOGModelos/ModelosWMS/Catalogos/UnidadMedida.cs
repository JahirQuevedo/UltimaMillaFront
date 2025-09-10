using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos;

[Table("WMS_012_UNIDAD_MEDIDA", Schema = "WMS")]
public class UnidadMedida : BaseEntity
{

    [Key]
    [Column("nIdUnidadMedidad012")]
    public int Id { get; set; }

    [Column("sClave")]
    [MaxLength(7)]
    public string Clave { get; set; }

    [Column("sDescripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

}
