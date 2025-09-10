using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface ICodigoDesperfectoService
{
    Task<ResultBase<List<CodigoDesperfecto>>> ObtenerPorDescripcionContiene(string descripcion);

}
