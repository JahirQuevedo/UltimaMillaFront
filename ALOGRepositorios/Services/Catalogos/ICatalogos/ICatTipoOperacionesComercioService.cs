using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTipoOperacionesComercioService
    {
        Task<List<CatTipoOperacionComercio>> GetCatTipoOperacionesComercio();
    }
}
