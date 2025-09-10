using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTransportistasService
    {
        Task<List<CatTransportistas>> GetTransportistas();
    }
}
