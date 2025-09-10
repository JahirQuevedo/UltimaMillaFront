using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IRecepcionService
{
    Task<PaginadoResult<MonitorRecepcionMercancia>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaRecepcion> entidad);

    Task<ResultBase<MonitorRecepcionMercancia>> ConfirmarRecepcionMercanciaPorIdPartidaInventario(int idInventario, int tieneAveria);

    Task<PaginadoResult<MonitorRecepcionMercancia>> ConfirmarRecepcionMercanciaPorIdTarja(ConsultaCatalogoBase<ConsultaRecepcion> entidad);

    Task<ResultBase<MonitorRecepcionMercancia>> FinalizarRecepcion(MonitorRecepcionMercancia monitorRecepcionMercancia);

}
