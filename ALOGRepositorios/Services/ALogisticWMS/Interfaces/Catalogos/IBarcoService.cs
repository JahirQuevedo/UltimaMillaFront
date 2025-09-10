using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IBarcoService
{

    Task<ResultBase<MonitorBarco>> Guardar(MonitorBarco monitorBarco);

    Task<ResultBase> Eliminar(int idBarco);

    Task<ResultBase<List<MonitorBarco>>> ObtenerPorNombreContiene(string nombre);

    Task<PaginadoResult<MonitorBarco>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaBarco> entidad);
}
