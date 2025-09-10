using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class ViajeService : IViajeService
{

    private readonly HttpClient _httpClient;

    public ViajeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<MonitorViaje>> Guardar(MonitorViaje monitorViaje)
    {
        ResultBase<MonitorViaje> resultBase = new ResultBase<MonitorViaje>();

        var jsonObject = JsonConvert.SerializeObject(monitorViaje);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Viaje/Guardar", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            //////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorViaje>>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<ResultBase> Eliminar(int idViaje)
    {
        var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogisticWMS}api/Viaje/Eliminar/{idViaje}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorViaje>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaViaje> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Viaje/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorViaje>>(contentResponse);
        return result;
    }

    public async Task<ResultBase<List<MonitorViaje>>> ObtenerPorReferenciaBuqueViajeContains(string referenciaBuqueViaje)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/Viaje/ObtenerPorReferenciaBuqueViajeContiene/{referenciaBuqueViaje}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<List<MonitorViaje>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<MonitorViaje>>>(contentResponse)!;
        return resultBase;
    }
}
