using ALOG.Enums;

namespace ALOG.Modelos;

public class ConsultaServicioFotografico : BaseEntity
{
    public int IdTarja { get; set; }

    public int IdFolioServicio { get; set; }

    public int IdInventario { get; set; }

    public TipoFoto TipoFoto { get; set; }
    
}
