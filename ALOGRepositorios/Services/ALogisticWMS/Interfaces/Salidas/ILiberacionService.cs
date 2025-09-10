using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface ILiberacionService
{

    Task<ResultBase> CrearOrdenSalida(MonitorLiberacionInventario monitorLiberacionInventario);

    Task<ResultBase> EliminarLiberacion(ConsultaLiberacionInventario consultaLiberacionInventario);

    Task<ResultBase> EliminarMercanciaLiberacion(ConsultaLiberacionInventario consultaLiberacionInventario);

    Task<PaginadoResult<MonitorLiberacionInventario>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaLiberacionInventario> entidad);

    Task<PaginadoResult<MonitorInventarioAlmacen>> ObtenerDetalleInventarioListaPaginada(ConsultaCatalogoBase<ConsultaLiberacionInventario> entidad);

    Task<PaginadoResult<MonitorLiberacionInventario>> ObtenerLiberacionControlEmbarqueListaPaginada(ConsultaCatalogoBase<ConsultaControlEmbarque> entidad);

    Task<ResultBase> AutorizarLiberacion(ConsultaLiberacionInventario consultaLiberacionInventario);

    Task<ResultBase> SalidaAlmacen(ConsultaLiberacionInventario consultaLiberacionInventario);

    Task<ResultBase> ControlEmbarqueAlmacen(ConsultaLiberacionInventario consultaLiberacionInventario);

}
