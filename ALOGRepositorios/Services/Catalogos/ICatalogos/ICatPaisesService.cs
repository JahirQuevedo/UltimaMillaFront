using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatPaisesService
    {
        Task<List<CatPaises>> CatPaisesListar();
    }
}
