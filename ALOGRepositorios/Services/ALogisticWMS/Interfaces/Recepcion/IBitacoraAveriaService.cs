using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IBitacoraAveriaService
{

    Task<ResultBase<MonitorBitacoraAveria>> Guardar(MonitorBitacoraAveria monitorBitacoraAveria);

    Task<ResultBase> Eliminar(int idBitacoraAveria);

    Task<ResultBase<MonitorBitacoraAveria>> ObtenerPorId(int idBitacoraAveria);

    Task<PaginadoResult<MonitorBitacoraAveria>> ObtenerListaPaginadaPorIdInventario(ConsultaCatalogoBase<ConsultaBitacoraAveria> entidad);

}
