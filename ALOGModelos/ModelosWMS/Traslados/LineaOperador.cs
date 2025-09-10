using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ALOG.Modelos;


[Table("WMS_021_LINEA_TRANS_OPERADOR", Schema = "WMS")]
public class LineaOperador : BaseEntity
{

    [Key]
    [Column("nIdLineaTransOperador021")]
    public int Id { get; set; }

    [Column("sNombre")]
    [MaxLength(100)]
    public string Nombre { get; set; }

    [Column("sApellidoPaterno")]
    [MaxLength(100)]
    public string ApellidoPaterno { get; set; }

    [Column("sApellidoMaterno")]
    [MaxLength(100)]
    public string ApellidoMaterno { get; set; }

    [NotMapped]
    public string NombreCompleto
    {
        get
        {
            return Nombre + " " + ApellidoPaterno + " " + ApellidoMaterno;
        }
    }

    [Column("sTelefono")]
    [MaxLength(50)]
    public string Telefono { get; set; }

    [Column("sLicencia")]
    [MaxLength(50)]
    public string Licencia { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

    [Column("nIdCatTransportista")]
    public int IdCatTransportista { get; set; }

    [ForeignKey("IdCatTransportista")]
    public virtual CatTransportistas Transportista { get; set; }

}
