using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos;

[Table("WMS_011_UBICACION", Schema = "WMS")]
public class Ubicacion : BaseEntity
{
    [Key]
    [Column("nIdUbicacion011")]
    public int Id { get; set; }

    [Column("sClave")]
    [MaxLength(10)]
    public string Clave { get; set; }

    [Column("sDescripcion")]
    [MaxLength(100)]
    public string Descripcion { get; set; }

    [Column("nPosicion")]
    public int Posicion { get; set; }

    [Column("nAltura")]
    public decimal Altura { get; set; }

    [Column("nCapacidad")]
    public decimal Capacidad { get; set; }

    [Column("nIdZonaAlmacenaje010")]
    public int? IdZonaAlmacenaje { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

    [ForeignKey("IdZonaAlmacenaje")]
    public virtual ZonaAlmacen ZonaAlmacenaje { get; set; }

}
