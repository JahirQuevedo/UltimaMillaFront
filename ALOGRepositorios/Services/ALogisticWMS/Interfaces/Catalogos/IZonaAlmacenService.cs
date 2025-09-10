using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IZonaAlmacenService
{

    Task<ResultBase<List<MonitorZonaAlmacen>>> ObtenerLista();

}
