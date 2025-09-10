using ALOG.Enums;

namespace ALOG.Modelos;

public class CargaArchivoFotografico : BaseEntity
{

    public int IdTarja { get; set; }

    public int FolioTarja { get; set; }

    public int IdFolioServicio { get; set; }

    public int FolioServicio { get; set; }

    public int Consecutivo { get; set; }

    public string IdMercancia { get; set; }    

    public int IdInventario { get; set; }

    public TipoFoto TipoFoto {  get; set; }

    public ArchivoBase ArchivoBase { get; set; }

}
