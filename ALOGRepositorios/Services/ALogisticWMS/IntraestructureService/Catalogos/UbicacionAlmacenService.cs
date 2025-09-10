using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services;

public class UbicacionAlmacenService : IUbicacionAlmacenService
{
    private readonly HttpClient _httpClient;

    public UbicacionAlmacenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<MonitorUbicacionAlmacen>> ObtenerPorClave(string clave)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/UbicacionAlmacen/ObtenerPorClave/{clave}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase<MonitorUbicacionAlmacen>>(contentResponse);
        return result;
    }

    public async Task<ResultBase<List<MonitorUbicacionAlmacen>>> ObtenerPorClaveContiene(int idZonaAlmacenaje, string clave)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/UbicacionAlmacen/ObtenerPorClaveContiene/{idZonaAlmacenaje}/{clave}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<List<MonitorUbicacionAlmacen>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<MonitorUbicacionAlmacen>>>(contentResponse)!;
        return resultBase;
    }

    public async Task<ResultBase<List<MonitorUbicacionAlmacen>>> ObtenerListaUbicacionPorIdAlmacenaje(int idZonaAlmacenaje)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/UbicacionAlmacen/ObtenerListaUbicacionPorIdAlmacenaje/{idZonaAlmacenaje}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<List<MonitorUbicacionAlmacen>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<MonitorUbicacionAlmacen>>>(contentResponse)!;
        return resultBase;
    }

    public async Task<ResultBase<MonitorUbicacionAlmacen>> ObtenerPorId(int idUbicacion)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/UbicacionAlmacen/ObtenerPorId/{idUbicacion}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<ResultBase<MonitorUbicacionAlmacen>>(contentResponse);
        return result;
    }
}
