using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public class SolicitudIngresoService : ISolicitudIngresoService
{

    private readonly HttpClient _httpClient;

    public SolicitudIngresoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ResultBase<MonitorSolicitudIngreso>> Guardar(MonitorSolicitudIngreso monitorSolicitudIngreso)
    {
        throw new NotImplementedException();
    }

    public Task<PaginadoResult<MonitorSolicitudIngreso>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaSolicitudIngreso> entidad)
    {
        throw new NotImplementedException();
    }

    public Task<ResultBase<MonitorSolicitudIngreso>> ObtenerPorId(int idSolicitudIngreso)
    {
        throw new NotImplementedException();
    }
}
