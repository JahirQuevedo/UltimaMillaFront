using ALOG.Enums;

namespace ALOG.Modelos;

public class RecepcionMercanciaDTO
{
    public int FolioTarja { get; set; }
    
    public int IdTarja { get; set; }

    public string IdMercancia { get; set; }

    public int IdInventario { get; set; }

    public TipoBusquedaRecepcion TipoBusquedaRecepcion { get; set; }

}
