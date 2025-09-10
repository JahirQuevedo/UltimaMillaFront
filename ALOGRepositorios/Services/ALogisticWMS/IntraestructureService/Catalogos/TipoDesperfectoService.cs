using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class TipoDesperfectoService : ITipoDesperfectoService
{

    private readonly HttpClient _httpClient;

    public TipoDesperfectoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<List<TipoDesperfecto>>> ObtenerPorDescripcionContiene(string descripcion)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/TipoDesperfecto/ObtenerPorDescripcionContiene/{descripcion}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<List<TipoDesperfecto>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<TipoDesperfecto>>>(contentResponse)!;
        return resultBase;
    }
}
