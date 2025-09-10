using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface ITipoDesperfectoService
{

    Task<ResultBase<List<TipoDesperfecto>>> ObtenerPorDescripcionContiene(string descripcion);

}
