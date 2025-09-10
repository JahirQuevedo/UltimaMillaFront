using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos;

public interface ICatPaisesServices
{
    Task<ICollection<CatPaises>> ObtenerPaisesConincidencia(string pConcidencia);

}
