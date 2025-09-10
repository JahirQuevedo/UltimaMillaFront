using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IViajeService
{
    Task<ResultBase<MonitorViaje>> Guardar(MonitorViaje monitorViaje);

    Task<ResultBase> Eliminar(int idViaje);

    Task<PaginadoResult<MonitorViaje>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaViaje> entidad);

    Task<ResultBase<List<MonitorViaje>>> ObtenerPorReferenciaBuqueViajeContains(string referenciaBuqueViaje);
}
