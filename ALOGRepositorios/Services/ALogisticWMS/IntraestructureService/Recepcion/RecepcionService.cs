using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class RecepcionService : IRecepcionService
{

    private readonly HttpClient _httpClient;

    public RecepcionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<MonitorRecepcionMercancia>> ConfirmarRecepcionMercanciaPorIdPartidaInventario(int idInventario, int tieneAveria)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/Recepcion/ConfirmarRecepcionPorIdPartidaInventario/{idInventario}/{tieneAveria}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        ResultBase<MonitorRecepcionMercancia> resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorRecepcionMercancia>>(contentResponse)!;
        return resultBase;
    }

    public async Task<PaginadoResult<MonitorRecepcionMercancia>> ConfirmarRecepcionMercanciaPorIdTarja(ConsultaCatalogoBase<ConsultaRecepcion> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Recepcion/ConfirmarRecepcionPorIdTarja", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorRecepcionMercancia>>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorRecepcionMercancia>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaRecepcion> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Recepcion/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorRecepcionMercancia>>(contentResponse);
        return result;
    }

    public async Task<ResultBase<MonitorRecepcionMercancia>> FinalizarRecepcion(MonitorRecepcionMercancia monitorRecepcionMercancia)
    {
        ResultBase<MonitorRecepcionMercancia> resultBase = new ResultBase<MonitorRecepcionMercancia>();

        var jsonObject = JsonConvert.SerializeObject(monitorRecepcionMercancia);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Recepcion/FinalizarRecepcion", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorRecepcionMercancia>>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }
}
