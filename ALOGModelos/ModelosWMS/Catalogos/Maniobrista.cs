using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ALOG.Modelos;


[Table("WMS_023_MANIOBRISTA", Schema = "WMS")]
public class Maniobrista : BaseEntity
{

    [Key]
    [Column("nIdManiobrista023")]
    public int Id { get; set; }


    [Column("sRazonSocial")]
    [MaxLength(250)]
    public string RazonSocial { get; set; }

    [Column("sRFC")]
    [MaxLength(30)]
    public string RFC { get; set; }

    [Column("sNombreCorto")]
    [MaxLength(20)]
    public string? NombreCorto { get; set; }

    [Column("sResponsable")]
    [MaxLength(150)]
    public string? Responsable { get; set; }

    [Column("sIdCamir")]
    [MaxLength(4)]
    public string? IdCamir { get; set; }

    [Column("bRecintoFiscalizado")]
    public bool? EsRecintoFiscalizado { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

}
