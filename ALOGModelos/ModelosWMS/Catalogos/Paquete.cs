using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos;

[Table("WMS_005_PAQUETE", Schema = "WMS")]
public class Paquete : BaseEntity
{
    [Key]
    [Column("nIdPaquete005")]
    public int Id { get; set; }

    [Column("sDescripcion")]
    public string Descripcion { get; set; }

    [Column("nTipoOperacion")]
    public int? TipoOperacionAduanera { get; set; }

    [Column("nTipoIngreso")]
    public TipoEntradaPaquete? TipoIngreso { get; set; }

    [Column("nIdServicio004")]
    public int? IdServicio { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

    public virtual ICollection<Servicio> Servicios { get; set; }

}
