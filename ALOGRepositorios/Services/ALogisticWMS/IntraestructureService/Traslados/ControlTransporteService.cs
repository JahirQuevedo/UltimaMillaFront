using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class ControlTransporteService : IControlTransporteService
{

    private readonly HttpClient _httpClient;

    public ControlTransporteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<MonitorCtrlTransporte>> Guardar(MonitorCtrlTransporte monitorCtrlTransporte)
    {
        ResultBase<MonitorCtrlTransporte> resultBase = new ResultBase<MonitorCtrlTransporte>();

        var jsonObject = JsonConvert.SerializeObject(monitorCtrlTransporte);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/ControlTransporte/Guardar", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorCtrlTransporte>>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<ResultBase<MonitorCtrlTransporte>> GuardarRelacionSolicitudTraslado(MonitorCtrlTransporte monitorCtrlTransporte)
    {
        ResultBase<MonitorCtrlTransporte> resultBase = new ResultBase<MonitorCtrlTransporte>();

        var jsonObject = JsonConvert.SerializeObject(monitorCtrlTransporte);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/ControlTransporte/GuardarRelacionSolicitudTraslado", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorCtrlTransporte>>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<PaginadoResult<MonitorCtrlTransporte>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaCtrlTransporte> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/ControlTransporte/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorCtrlTransporte>>(contentResponse);
        return result;
    }

    public async Task<ResultBase<MonitorCtrlTransporte>> ObtenerPorId(int id)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/ControlTransporte/ObtenerPorId/{id}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<ResultBase<MonitorCtrlTransporte>>(contentResponse);
        return result;
    }
}
