using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class InventarioAlmacenService : IInventarioAlmacenService
{

    private readonly HttpClient _httpClient;

    public InventarioAlmacenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginadoResult<MonitorInventarioAlmacen>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaMonitorInventario> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/InventarioAlmacen/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorInventarioAlmacen>>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorInventarioAlmacen>> ObtenerExistenciaMercanciaListaPaginada(ConsultaCatalogoBase<ConsultaMonitorInventario> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/InventarioAlmacen/ObtenerExistenciaMercanciaListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorInventarioAlmacen>>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorInventarioAlmacen>> ObtenerMercanciaInventarioListaPaginada(ConsultaCatalogoBase<ConsultaMonitorInventario> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/InventarioAlmacen/ObtenerMercanciaInventarioListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorInventarioAlmacen>>(contentResponse);
        return result;
    }

}
