using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IPaqueteService
{
    Task<ResultBase<List<Paquete>>> ObtenerPorDescripcionContiene(string descripcion);

}
