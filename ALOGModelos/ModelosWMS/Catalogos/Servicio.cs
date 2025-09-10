using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOG.Modelos;

[Table("WMS_004_SERVICIO", Schema = "WMS")]
public class Servicio : BaseEntity
{
    [Key]
    [Column("nIdServicio004")]
    public int Id { get; set; }

    [Column("sClaveServicio")]
    public required string Clave { get; set; }

    [Column("sDescripcion")]
    public string? Descripcion { get; set; }

    [Column("nCosto")]
    public decimal? nCosto { get; set; }

    [Column("nTipoOperacion")]
    public TipoOperacionAduanera TipoOperacion { get; set; }

    [Column("bEsAlmacenaje")]
    public bool? EsAlmacenaje { get; set; }

    [Column("bEsFlete")]
    public bool? EsFlete { get; set; }

    [Column("bExtraccionPatioExterno")]
    public bool? ExtraccionPatioExterno { get; set; }

    [Column("bPaquete")]
    public bool? EsPaquete { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

    [Column("nIdCatEmpresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    public virtual CatEmpresas Empresa { get; set; }

    public virtual ICollection<Paquete>? Paquetes { get; set; }

}
