using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IUbicacionAlmacenService
{

    Task<ResultBase<MonitorUbicacionAlmacen>> ObtenerPorClave(string clave);

    Task<ResultBase<MonitorUbicacionAlmacen>> ObtenerPorId(int idUbicacion);

    Task<ResultBase<List<MonitorUbicacionAlmacen>>> ObtenerPorClaveContiene(int idZonaAlmacenaje, string clave);

    Task<ResultBase<List<MonitorUbicacionAlmacen>>> ObtenerListaUbicacionPorIdAlmacenaje(int idZonaAlmacenaje);

}
