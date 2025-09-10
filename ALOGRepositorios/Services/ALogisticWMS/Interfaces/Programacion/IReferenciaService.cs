using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IReferenciaWMSService
{
    Task<ResultBase<MonitorReferencia>> Guardar(MonitorReferencia monitorReferencia);

    Task<ResultBase<MonitorReferencia>> ObtenerReferenciaPorId(int idReferencia);

    Task<PaginadoResult<MonitorReferencia>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaReferencia> entidad);

    Task<ResultBase> Eliminar(int idReferencia);

}

