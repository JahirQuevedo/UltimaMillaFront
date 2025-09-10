using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class TipoSeveridadService : ITipoSeveridadService
{

    private readonly HttpClient _httpClient;

    public TipoSeveridadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<List<TipoSeveridad>>> ObtenerPorDescripcionContiene(string descripcion)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/TipoSeveridad/ObtenerPorDescripcionContiene/{descripcion}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<List<TipoSeveridad>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<TipoSeveridad>>>(contentResponse)!;
        return resultBase;
    }
}
