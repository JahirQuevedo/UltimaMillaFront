using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos;

[Table("WMS_022_LINEA_TRANS_TRANSPORTE", Schema = "WMS")]
public class LineaTransporte : BaseEntity
{

    [Key]
    [Column("nIdLineaTransTransporte022")]
    public int Id { get; set; }

    [Column("sPlacas")]
    [MaxLength(25)]
    public string Placas { get; set; }

    [Column("sNumeroEconomico")]
    [MaxLength(10)]
    public string NumeroEconomico { get; set; }

    [Column("sPlacasPlana1")]
    [MaxLength(25)]
    public string PlacasPlana1 { get; set; }

    [Column("sPlacasPlana2")]
    [MaxLength(25)]
    public string PlacasPlana2 { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

    [Column("nIdCatTransportista")]
    public int IdCatTransportista { get; set; }

    [ForeignKey("IdCatTransportista")]
    public virtual CatTransportistas Transportista { get; set; }

}
