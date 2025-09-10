using ALOG.Modelos;


namespace ALOGRepositorios.Services;

public interface IControlTransporteService
{

    Task<ResultBase<MonitorCtrlTransporte>> Guardar(MonitorCtrlTransporte monitorCtrlTransporte);

    Task<ResultBase<MonitorCtrlTransporte>> GuardarRelacionSolicitudTraslado(MonitorCtrlTransporte monitorCtrlTransporte);

    Task<ResultBase<MonitorCtrlTransporte>> ObtenerPorId(int ididTarja);

    Task<PaginadoResult<MonitorCtrlTransporte>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaCtrlTransporte> entidad);


}
