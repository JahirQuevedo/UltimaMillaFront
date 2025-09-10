using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatPaisMunicipiosService
    {
        Task<List<CatPaisMunicipios>> CatPaisMunicipiosListar();
    }
}
