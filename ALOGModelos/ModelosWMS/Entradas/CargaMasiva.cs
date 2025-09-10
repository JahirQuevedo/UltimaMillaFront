

namespace ALOG.Modelos;

public class CargaMasiva : BaseEntity
{

    public int IdTarja { get; set; }

    public ArchivoBase ArchivoBase { get; set; }

    public List<PartidaAlmacen> ListaPartidas { get; set; }

}
