using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IPartidaService
{
    Task<ResultBase<MonitorPartida>> Guardar(MonitorPartida monitorPartida);
    Task<PaginadoResult<MonitorPartida>> ObtenerListaPaginadaPorIdTarja(ConsultaCatalogoBase<ConsultaPartida> entidad);

}
