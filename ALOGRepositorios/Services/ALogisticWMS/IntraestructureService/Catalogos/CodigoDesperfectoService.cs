using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class CodigoDesperfectoService : ICodigoDesperfectoService
{

    private readonly HttpClient _httpClient;

    public CodigoDesperfectoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<List<CodigoDesperfecto>>> ObtenerPorDescripcionContiene(string descripcion)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/CodigoDesperfecto/ObtenerPorDescripcionContiene/{descripcion}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<List<CodigoDesperfecto>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<CodigoDesperfecto>>>(contentResponse)!;
        return resultBase;
    }
}
