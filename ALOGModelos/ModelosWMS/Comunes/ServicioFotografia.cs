using ALOG.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos;


[Table("WMS_028_SERVICIO_FOTOGRAFIA", Schema = "WMS")]
public class ServicioFotografia : BaseEntity
{

    [Key]
    [Column("nIdServicioFotografia028")]
    public int Id { get; set; }

    [Column("nIdFolioServicio018")]
    public int? IdFolioServicio { get; set; }

    [Column("nIdInventario014")]
    public int? IdInventario { get; set; }

    [Column("sRutaBase")]
    public string RutaBase { get; set; }

    [Column("sRutaNombreArchivo")]
    public string RutaNombreArchivo { get; set; }

    [Column("nTipoFoto")]
    public TipoFoto TipoFoto { get; set; }

    [ForeignKey("IdFolioServicio")]
    public virtual FolioServicio FolioServicio { get; set; }

    [ForeignKey("IdInventario")]
    public virtual Inventario Inventario { get; set; }

}
