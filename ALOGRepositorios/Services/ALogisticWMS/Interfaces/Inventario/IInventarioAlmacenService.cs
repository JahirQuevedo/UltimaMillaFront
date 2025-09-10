using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IInventarioAlmacenService
{

    Task<PaginadoResult<MonitorInventarioAlmacen>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaMonitorInventario> entidad);

    Task<PaginadoResult<MonitorInventarioAlmacen>> ObtenerExistenciaMercanciaListaPaginada(ConsultaCatalogoBase<ConsultaMonitorInventario> entidad);


    Task<PaginadoResult<MonitorInventarioAlmacen>> ObtenerMercanciaInventarioListaPaginada(ConsultaCatalogoBase<ConsultaMonitorInventario> entidad);

}
