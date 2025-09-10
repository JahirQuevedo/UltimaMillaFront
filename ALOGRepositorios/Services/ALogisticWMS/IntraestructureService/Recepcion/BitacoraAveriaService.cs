using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class BitacoraAveriaService : IBitacoraAveriaService
{
    private readonly HttpClient _httpClient;

    public BitacoraAveriaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase> Eliminar(int idBitacoraAveria)
    {
        var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogisticWMS}api/BitacoraAveria/Eliminar/{idBitacoraAveria}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase>(contentResponse);
        return result;
    }

    public async Task<ResultBase<MonitorBitacoraAveria>> Guardar(MonitorBitacoraAveria monitorBitacoraAveria)
    {
        ResultBase<MonitorBitacoraAveria> resultBase = new ResultBase<MonitorBitacoraAveria>();

        var jsonObject = JsonConvert.SerializeObject(monitorBitacoraAveria);

        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/BitacoraAveria/Guardar", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorBitacoraAveria>>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<PaginadoResult<MonitorBitacoraAveria>> ObtenerListaPaginadaPorIdInventario(ConsultaCatalogoBase<ConsultaBitacoraAveria> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/BitacoraAveria/ObtenerListaPaginadaPorIdInventario", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorBitacoraAveria>>(contentResponse);
        return result;
    }

    public async Task<ResultBase<MonitorBitacoraAveria>> ObtenerPorId(int idBitacoraAveria)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/BitacoraAveria/ObtenerPorId/{idBitacoraAveria}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase<MonitorBitacoraAveria>>(contentResponse);
        return result;
    }
}
