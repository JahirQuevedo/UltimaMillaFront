using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTipoOperacionService
    {
        Task<List<CatTipoOperacion>> CatTipoOperacionListar();
    }
}
