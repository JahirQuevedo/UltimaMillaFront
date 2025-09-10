using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class SolicitudTrasladoService : ISolicitudTrasladoService
{

    private readonly HttpClient _httpClient;

    public SolicitudTrasladoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<MonitorSolicitudTraslado>> Guardar(MonitorSolicitudTraslado monitorSolicitudTraslado)
    {
        ResultBase<MonitorSolicitudTraslado> resultBase = new ResultBase<MonitorSolicitudTraslado>();

        var jsonObject = JsonConvert.SerializeObject(monitorSolicitudTraslado);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/SolicitudTraslado/Guardar", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorSolicitudTraslado>>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<ResultBase> CancelarSolicitudTraslado(ConsultaSolicitudTraslado entidad)
    {
        ResultBase resultBase = new ResultBase();

        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/SolicitudTraslado/CancelarSolicitudTraslado", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            resultBase = JsonConvert.DeserializeObject<ResultBase>(contentResponse);

        }
        catch (Exception ex)
        {
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<ResultBase<MonitorSolicitudTraslado>> GuardarServicioSolicitudTraslado(MonitorSolicitudTraslado monitorSolicitudTraslado)
    {
        ResultBase<MonitorSolicitudTraslado> resultBase = new ResultBase<MonitorSolicitudTraslado>();

        var jsonObject = JsonConvert.SerializeObject(monitorSolicitudTraslado);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/SolicitudTraslado/GuardarServicioSolicitudTraslado", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorSolicitudTraslado>>(contentResponse);

        }
        catch (Exception ex)
        {
            //Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<PaginadoResult<MonitorSolicitudTraslado>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaSolicitudTraslado> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/SolicitudTraslado/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorSolicitudTraslado>>(contentResponse);
        return result;
    }

    public async Task<ResultBase<MonitorSolicitudTraslado>> ObtenerPorId(int idSolicitudTraslado)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/SolicitudTraslado/ObtenerPorId/{idSolicitudTraslado}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<ResultBase<MonitorSolicitudTraslado>>(contentResponse);
        return result;
    }

}
