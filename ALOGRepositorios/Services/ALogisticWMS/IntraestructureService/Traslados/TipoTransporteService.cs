using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class TipoTransporteServie : ITipoTransporteService
{

    private readonly HttpClient _httpClient;

    public TipoTransporteServie(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<List<TipoTransporte>>> ObtenerPorNombreContiene(string nombre)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/TipoTransporte/ObtenerPorNombreContiene/{nombre}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("ObtenerPorNombreContiene - contentResponse -> " + contentResponse);
        ResultBase<List<TipoTransporte>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<TipoTransporte>>>(contentResponse)!;
        return resultBase;
    }
}
