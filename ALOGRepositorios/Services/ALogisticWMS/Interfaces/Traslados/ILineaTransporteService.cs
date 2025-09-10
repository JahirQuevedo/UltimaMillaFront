using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface ILineaTransporteService
{
    Task<ResultBase<MonitorLineaTransporte>> ObtenerPorIdLineaTransportista(int idLineaTransportista);

    Task<ResultBase<MonitorLineaTransporte>> ObtenerPorIdransportista(int idTransportista);

    Task<ResultBase<List<MonitorLineaTransporte>>> ObtenerPorRazonSocialContiene(string razonSocial);

    Task<ResultBase<List<MonitorLineaTransporte>>> ObtenerPorPlacasContiene(int idCatTransportista, string placas);

}
