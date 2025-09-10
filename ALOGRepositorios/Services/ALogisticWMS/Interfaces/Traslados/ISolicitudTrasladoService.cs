using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface ISolicitudTrasladoService
{

    Task<ResultBase<MonitorSolicitudTraslado>> Guardar(MonitorSolicitudTraslado monitorSolicitudTraslado);

    Task<ResultBase<MonitorSolicitudTraslado>> GuardarServicioSolicitudTraslado(MonitorSolicitudTraslado monitorSolicitudTraslado);

    Task<ResultBase<MonitorSolicitudTraslado>> ObtenerPorId(int idSolicitudTraslado);

    Task<PaginadoResult<MonitorSolicitudTraslado>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaSolicitudTraslado> entidad);

    Task<ResultBase> CancelarSolicitudTraslado(ConsultaSolicitudTraslado entidad);

}
