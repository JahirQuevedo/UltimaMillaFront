using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTransportistaService
    {

        public Task<List<CatTransportistas>> GetTransportistas();

    }
}
