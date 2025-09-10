using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class LineaOperadorService : ILineaOperadorService
{

    private readonly HttpClient _httpClient;

    public LineaOperadorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<List<MonitorLineaOperador>>> ObtenerPorNombreContiene(int idCatTransportista, string nombre)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/LineaOperador/ObtenerPorNombreContiene/{idCatTransportista}/{nombre}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("ObtenerPorNombreContiene - contentResponse -> " + contentResponse);
        ResultBase<List<MonitorLineaOperador>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<MonitorLineaOperador>>>(contentResponse)!;
        return resultBase;
    }

}
