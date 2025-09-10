using ALOG.Modelos;

namespace ALOGRepositorios;

public interface ISolicitudIngresoService
{

    Task<ResultBase<MonitorSolicitudIngreso>> Guardar(MonitorSolicitudIngreso monitorSolicitudIngreso);

    Task<ResultBase<MonitorSolicitudIngreso>> ObtenerPorId(int idSolicitudIngreso);

    Task<PaginadoResult<MonitorSolicitudIngreso>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaSolicitudIngreso> entidad);

}
