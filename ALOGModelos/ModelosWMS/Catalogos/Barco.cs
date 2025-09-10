using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos;

[Table("WMS_003_BARCO", Schema = "WMS")]
public class Barco : BaseEntity
{

    [Key]
    [Column("nIdBarco003")]
    public int Id { get; set; }

    [Column("sNombre")]
    public required string Nombre { get; set; }

    [Column("nIdCatPais")]
    public int? IdCatPais { get; set; }

    [Column("nIdCatNaviera")]
    public int? IdCatNaviera { get; set; }

    [Column("bActivo")]
    public bool Activo { get; set; }

    [ForeignKey("IdCatPais")]
    public virtual CatPaises Pais { get; set; }

    [ForeignKey("IdCatNaviera")]
    public virtual CatNavieras Naviera { get; set; }

}
