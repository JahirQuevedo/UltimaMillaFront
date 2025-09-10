using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos;


[Table("WMS_020_TIPO_TRANSPORTE", Schema = "WMS")]
public class TipoTransporte : BaseEntity
{

    [Key]
    [Column("nIdTipoTransporte020")]
    public int Id { get; set; }

    [Column("sDescripcion")]
    [MaxLength(50)]
    public string Descripcion { get; set; }

    [Column("nTipo")]
    public TipoMedioTransporteCatalogo Tipo { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

}
