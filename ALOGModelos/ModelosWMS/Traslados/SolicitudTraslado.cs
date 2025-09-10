using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
////using NuGet.Protocol.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos;

[Table("WMS_026_SOLICITUD_TRASLADO", Schema = "WMS")]
public class SolicitudTraslado : BaseEntity
{

    [Key]
    [Column("nIdSolicitudTraslado026")]
    public int Id { get; set; }

    [Column("nFolio")]
    public int Folio { get; set; }

    [Column("sBoletaLiberacionOrigen")]
    public string? BoletaLiberacionOrigen { get; set; }

    [Column("nEstado")]
    public EstadoTraslado? EstadoTraslado { get; set; }

    [Column("nPrioridad")]
    public int? Prioridad { get; set; }

    [Column("sObservaciones")]
    [MaxLength(4000)]
    public string? Observaciones { get; set; }

    [Column("dFechaSolicitudTraslado")]
    public DateTime? FechaSolicitudTraslado { get; set; }

    [Column("dFechaRecepcionBoleta")]
    public DateTime? FechaRecepcionBoleta { get; set; }

    [Column("dFechaVigenciaBoleta")]
    public DateTime? FechaVigenciaBoleta { get; set; }

    [Column("nIdFolioServicio018")]
    public int? IdFolioServicio { get; set; }

    [Column("nIdFolioServicioMasterDetalle025")]
    public int? IdFolioServicioMasterDetalle { get; set; }

    [Column("nIdManiobristaOrigen023")]
    public int? IdManiobristaOrigen { get; set; }

    [Column("nIdManiobristaDestino023")]
    public int? IdManiobristaDestino { get; set; }

    [Column("nIdControlTransporte024")]
    public int? IdControlTransporte { get; set; }

    [Column("nIdCatEmpresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    public virtual CatEmpresas Empresa { get; set; }

    [ForeignKey("IdFolioServicio")]
    public virtual FolioServicio FolioServicio { get; set; }

    [ForeignKey("IdControlTransporte")]
    public virtual ControlTransporte ControlTransporte { get; set; }

    [ForeignKey("IdFolioServicioMasterDetalle")]
    public virtual FolioServicioMasterDetalle FolioServicioMsterDetalle { get; set; }

    [ForeignKey("IdManiobristaOrigen")]
    public virtual Maniobrista ManiobristaOrigen { get; set; }

    [ForeignKey("IdManiobristaDestino")]
    public virtual Maniobrista ManiobristaDestino { get; set; }

}
