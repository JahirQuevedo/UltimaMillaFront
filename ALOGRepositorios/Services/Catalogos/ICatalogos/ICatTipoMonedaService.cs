
using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTipoMonedaService
    {

        Task<ICollection<CatTipoMoneda>> ObtenerMonedas();

    }
}
