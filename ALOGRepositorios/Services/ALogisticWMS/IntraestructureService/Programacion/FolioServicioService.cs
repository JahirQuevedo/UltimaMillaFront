using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class FolioServicioService : IFolioServicioService
{

    private readonly HttpClient _httpClient;

    public FolioServicioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<MonitorFolioServicio>> Guardar(MonitorFolioServicio monitorFolioServicio)
    {
        ResultBase<MonitorFolioServicio> resultBase = new ResultBase<MonitorFolioServicio>();

        var jsonObject = JsonConvert.SerializeObject(monitorFolioServicio);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/FolioServicio/Guardar", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorFolioServicio>>(contentResponse);

        }
        catch (Exception ex)
        {

            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<ResultBase> Confirmar(int idFolioServicio)
    {
        var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogisticWMS}api/FolioServicio/Confirmar/{idFolioServicio}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase>(contentResponse);
        return result;
    }

    public async Task<ResultBase> Eliminar(int idFolioServicio)
    {
        var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogisticWMS}api/FolioServicio/Eliminar/{idFolioServicio}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorFolioServicio>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaFolioServicio> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/FolioServicio/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorFolioServicio>>(contentResponse);
        return result;
    }

    public async Task<ResultBase<MonitorFolioServicio>> ObtenerPorId(int IdFolioServicio)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/FolioServicio/ObtenerPorId/{IdFolioServicio}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<ResultBase<MonitorFolioServicio>>(contentResponse);
        return result;
    }
}
