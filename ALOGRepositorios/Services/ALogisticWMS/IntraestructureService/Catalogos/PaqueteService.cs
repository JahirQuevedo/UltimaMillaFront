using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class PaqueteService : IPaqueteService
{

    private readonly HttpClient _httpClient;

    public PaqueteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<List<Paquete>>> ObtenerPorDescripcionContiene(string descripcion)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/Paquete/ObtenerPorDescripcionContiene/{descripcion}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<List<Paquete>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<Paquete>>>(contentResponse)!;
        return resultBase;
    }

}
