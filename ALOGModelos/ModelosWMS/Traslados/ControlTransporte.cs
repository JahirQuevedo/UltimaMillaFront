using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ALOG.Modelos;

[Table("WMS_024_CONTROL_TRANSPORTE", Schema = "WMS")]
public class ControlTransporte : BaseEntity
{
    [Key]
    [Column("nIdControlTransporte024")]
    public int Id { get; set; }

    [Column("nFolio")]
    public int Folio { get; set; }

    [Column("nCantidad")]
    public int? Cantidad { get; set; }

    [Column("nPeso")]
    public int? Peso { get; set; }

    [Column("nTipoEntrada")]
    public TipoEntrada? TipoEntrada { get; set; }

    [Column("nTipoViaje")]
    public TipoViaje? TipoViaje { get; set; }

    [Column("sViajes")]
    [MaxLength(50)]
    public string? Viajes { get; set; }

    [Column("nTipoOperacion")]
    public TipoOperacionTurno TipoOperacion { get; set; }

    [Column("sObservacion")]
    [MaxLength(4000)]
    public string? Observacion { get; set; }

    [Column("dFechaLlegada")]
    public DateTime? FechaLlegada { get; set; }

    [Column("dFechaIngreso")]
    public DateTime? FechaIngreso { get; set; }

    [Column("dFechaSalida")]
    public DateTime? FechaSalida { get; set; }

    [Column("bCargado")]
    public bool? Cargado { get; set; }

    [Column("bSalidaAutorizada")]
    public bool? SalidaAutorizada { get; set; }

    [Column("dFechaAutorizacionEntrada")]
    public DateTime? FechaAtuorizacionEntrada { get; set; }

    [Column("dFechaInicioCargaDescarga")]
    public DateTime? FechaInicioCargaDescarga { get; set; }

    [Column("FechaFinCargaDescarga")]
    public DateTime? FechaFinCargaDescarga { get; set; }

    [Column("dFechaAutorizacionSalida")]
    public DateTime? FechaAtuorizacionSalida { get; set; }

    [Column("dFechaCancelacion")]
    public DateTime? FechaCancelacion { get; set; }

    [Column("sMotivoCancelacion")]
    public DateTime? MotivoCancelacion { get; set; }

    [Column("nIdTipoTransporte020")]
    public int? IdTipoTransporte { get; set; }

    [Column("nIdLineaTansOperador021")]
    public int? IdLineaTransOperador { get; set; }

    [Column("nIdLineaTransTransporte022")]
    public int? IdLineaTransTransporte { get; set; }

    [Column("nTurno")]
    public int? Turno { get; set; }

    [Column("nEstado")]
    public EstadoTurno? Estado { get; set; }

    [Column("nIdManiobristaOrigen023")]
    public int? IdManiobristaOrigen { get; set; }

    [Column("nIdManiobristaDestino023")]
    public int? IdManiobristaDestino { get; set; }

    [Column("nIdCatEmpresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdTipoTransporte")]
    public virtual TipoTransporte TipoTransporte { get; set; }

    [ForeignKey("IdLineaTransOperador")]
    public virtual LineaOperador LineaOperador { get; set; }

    [ForeignKey("IdLineaTransTransporte")]
    public virtual LineaTransporte LineaTransporte { get; set; }

    [ForeignKey("IdEmpresa")]
    public virtual CatEmpresas Empresa { get; set; }

    [ForeignKey("IdManiobristaOrigen")]
    public virtual Maniobrista ManiobristaOrigen { get; set; }

    [ForeignKey("IdManiobristaDestino")]
    public virtual Maniobrista ManiobristaDestino { get; set; }

}
