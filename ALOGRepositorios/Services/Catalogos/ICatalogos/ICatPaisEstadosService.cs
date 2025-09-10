using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatPaisEstadosService
    {
        Task<List<CatPaisEstados>> CatPaisEstadosListar();
    }
}
