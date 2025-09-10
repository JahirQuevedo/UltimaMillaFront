using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class BarcoService : IBarcoService
{
    private readonly HttpClient _httpClient;

    public BarcoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<MonitorBarco>> Guardar(MonitorBarco monitorViaje)
    {
        ResultBase<MonitorBarco> resultBase = new ResultBase<MonitorBarco>();

        var jsonObject = JsonConvert.SerializeObject(monitorViaje);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Barco/Guardar", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorBarco>>(contentResponse);

        }
        catch (Exception ex)
        {
            //Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<ResultBase> Eliminar(int idBarco)
    {
        var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogisticWMS}api/Barco/Eliminar/{idBarco}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase>(contentResponse);
        return result;
    }

    public async Task<ResultBase<List<MonitorBarco>>> ObtenerPorNombreContiene(string nombre)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/Barco/ObtenerPorNombreContiene/{nombre}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<List<MonitorBarco>> resultBase = JsonConvert.DeserializeObject<ResultBase<List<MonitorBarco>>>(contentResponse)!;
        return resultBase;
    }

    public async Task<PaginadoResult<MonitorBarco>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaBarco> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Barco/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorBarco>>(contentResponse);
        return result;
    }
}

