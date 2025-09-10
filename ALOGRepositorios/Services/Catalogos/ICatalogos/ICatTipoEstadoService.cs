using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTipoEstadoService
    {

        public Task<CatTipoEstados> GetTipoEstado(int idTipoEstado);
        public Task<ICollection<CatTipoEstados>> GetTiposEstado();
    }
}
