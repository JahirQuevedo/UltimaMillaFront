using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTipoIMOService
    {
        Task<List<CatTipoIMO>> GetCatTipoIMO();
    }
}
