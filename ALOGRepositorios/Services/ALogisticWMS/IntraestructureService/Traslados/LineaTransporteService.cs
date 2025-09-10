using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class LineaTransporteService : ILineaTransporteService
{
    private readonly HttpClient _httpClient;

    public LineaTransporteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ResultBase<MonitorLineaTransporte>> ObtenerPorIdLineaTransportista(int idLineaTransportista)
    {
        throw new NotImplementedException();
    }

    public Task<ResultBase<MonitorLineaTransporte>> ObtenerPorIdransportista(int idTransportista)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultBase<List<MonitorLineaTransporte>>> ObtenerPorPlacasContiene(int idCatTransportista, string placas)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/LineaTransporte/ObtenerPorPlacasContiene/{idCatTransportista}/{placas}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("ObtenerPorNombreContiene - contentResponse -> " + contentResponse);
        ResultBase<List<MonitorLineaTransporte>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<MonitorLineaTransporte>>>(contentResponse)!;
        return resultBase;
    }

    public async Task<ResultBase<List<MonitorLineaTransporte>>> ObtenerPorRazonSocialContiene(string razonSocial)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/LineaTransporte/ObtenerPorRazonSocialContiene/{razonSocial}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("ObtenerPorNombreContiene - contentResponse -> " + contentResponse);
        ResultBase<List<MonitorLineaTransporte>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<MonitorLineaTransporte>>>(contentResponse)!;
        return resultBase;
    }
}
