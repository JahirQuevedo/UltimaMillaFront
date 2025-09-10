using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ALOG.Enums;
using ALOG.Modelos.Modelos.Catalogos;
//using NuGet.Protocol.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos;

[Table("WMS_027_SOLICITUD_INGRESO", Schema = "WMS")]
public class SolicitudIngreso : BaseEntity
{

    [Key]
    [Column("nIdSolicitudIngreso027")]
    public int Id { get; set; }

    [Column("nFolio")]
    public int Folio { get; set; }

    [Column("nEstado")]
    public EstadoSolicitudIngreso? EstadoIngreso { get; set; }


}