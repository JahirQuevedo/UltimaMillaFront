using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IFolioServicioService
{

    Task<ResultBase<MonitorFolioServicio>> Guardar(MonitorFolioServicio monitorFolioServicio);

    Task<ResultBase> Confirmar(int idFolioServicio);

    Task<ResultBase> Eliminar(int idFolioServicio);

    Task<ResultBase<MonitorFolioServicio>> ObtenerPorId(int IdFolioServicio);

    Task<PaginadoResult<MonitorFolioServicio>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaFolioServicio> entidad);

}
