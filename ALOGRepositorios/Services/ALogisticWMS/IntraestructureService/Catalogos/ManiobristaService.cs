using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class ManiobristaService : IManiobristaService
{

    private readonly HttpClient _httpClient;

    public ManiobristaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<List<MonitorManiobrista>>> ObtenerPorRazonSocialContiene(string razonSocial)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/Maniobrista/ObtenerPorRazonSocialContiene/{razonSocial}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<List<MonitorManiobrista>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<MonitorManiobrista>>>(contentResponse)!;
        return resultBase;
    }
}
