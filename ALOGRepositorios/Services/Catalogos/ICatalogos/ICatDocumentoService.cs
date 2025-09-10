using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatDocumentoService
    {

        public Task<ICollection<CatDocumentos>> GetTiposDocumento();

    }
}
