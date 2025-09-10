using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class ZonaAlmacenService : IZonaAlmacenService
{

    private readonly HttpClient _httpClient;

    public ZonaAlmacenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<List<MonitorZonaAlmacen>>> ObtenerLista()
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/ZonaAmacenaje/ObtenerLista");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase<List<MonitorZonaAlmacen>>>(contentResponse);
        return result;
    }
}
