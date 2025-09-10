using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTipoContenedorService
    {

        public Task<ICollection<CatTipoContenedor>> GetTiposContenedorAsync();

    }
}
