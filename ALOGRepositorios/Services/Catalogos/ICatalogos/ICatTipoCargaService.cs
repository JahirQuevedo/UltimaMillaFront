using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTipoCargaService
    {
        Task<List<CatTipoCarga>> CatTipoCargaListar();
    }
}
