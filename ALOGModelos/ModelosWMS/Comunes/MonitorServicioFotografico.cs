using ALOG.Enums;

namespace ALOG.Modelos;

public class MonitorServicioFotografico : BaseEntity
{

    public int idServicioFotografico { get; set; }
    public int IdTarja { get; set; }

    public int IdFolioServicio { get; set; }

    public int IdInventario { get; set; }

    public TipoFoto TipoFoto { get; set; }

    public ArchivoBase ArchivoBase { get; set; }

}
