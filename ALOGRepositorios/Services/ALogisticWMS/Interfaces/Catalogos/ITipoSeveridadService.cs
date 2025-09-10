using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface ITipoSeveridadService
{

    Task<ResultBase<List<TipoSeveridad>>> ObtenerPorDescripcionContiene(string descripcion);

}
