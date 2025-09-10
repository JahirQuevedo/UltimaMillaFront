using ALOG.Enums;


namespace ALOG.Modelos;

public class RelacionInventarioServicio : BaseEntity
{

    public int IdFolioServicio { get; set; }

    public int IdReferencia { get; set; }

    public int IdTarja { get; set; }

    public int IdPartidaInventario { get; set; }

    public int Folio { get; set; }

    public ServicioAplicado TipoServicioAplicado { get; set; }

}
