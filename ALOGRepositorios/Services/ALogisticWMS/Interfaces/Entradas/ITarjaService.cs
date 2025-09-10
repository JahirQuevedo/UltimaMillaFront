using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface ITarjaService
{
    /// <summary>
    /// Método para guardar tarja
    /// </summary>
    /// <param name="monitorTarja"></param>
    /// <returns></returns>
    Task<ResultBase<MonitorTarja>> Guardar(MonitorTarja monitorTarja);

    /// <summary>
    //  Método para eliminar tarja
    /// </summary>
    /// <param name="idTarja"></param>
    /// <returns></returns>
    Task<ResultBase> Eliminar(int idTarja);

    /// <summary>
    /// Método para Confirmar Tarja
    /// </summary>
    /// <param name="monitorTarja"></param>
    /// <returns></returns>
    Task<ResultBase<MonitorTarja>> Confirmar(MonitorTarja monitorTarja);

    Task<ResultBase<MonitorTarja>> ObtenerPorId(int idTarja);

    Task<PaginadoResult<MonitorTarja>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaTarja> entidad);

    Task<PaginadoResult<MonitorTarja>> ObtenerListaPaginadaPorIdReferencia(ConsultaCatalogoBase<ConsultaTarja> entidad);

    Task<ResultBase> GuardarCargaMasiva(CargaMasiva cargaMasiva);

    Task<PaginadoResult<MonitorTarjaInventario>> ObtenerListaPaginadaTarjaInventarioPorIdReferencia(ConsultaCatalogoBase<ConsultaTarja> entidad);

}
