using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;


public class TarjaService : ITarjaService
{
    private readonly HttpClient _httpClient;

    public TarjaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Método para guardar la Tarja 
    /// </summary>
    /// <param name="monitorTarja"></param>
    /// <returns></returns>
    public async Task<ResultBase<MonitorTarja>> Guardar(MonitorTarja monitorTarja)
    {
        ResultBase<MonitorTarja> resultBase = new ResultBase<MonitorTarja>();

        var jsonObject = JsonConvert.SerializeObject(monitorTarja);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Tarja/Guardar", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorTarja>>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    /// <summary>
    /// Método para eliminar tarja
    /// </summary>
    /// <param name="idTarja"></param>
    /// <returns></returns>/
    public async Task<ResultBase> Eliminar(int idTarja)
    {
        var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogisticWMS}api/Tarja/Eliminar/{idTarja}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase>(contentResponse);
        return result;
    }

    public async Task<ResultBase<MonitorTarja>> Confirmar(MonitorTarja monitorTarja)
    {
        ResultBase<MonitorTarja> resultBase = new ResultBase<MonitorTarja>();

        var jsonObject = JsonConvert.SerializeObject(monitorTarja);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Tarja/Confirmar", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorTarja>>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<ResultBase<MonitorTarja>> ObtenerPorId(int idTarja)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/Tarja/ObtenerPorId/{idTarja}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<ResultBase<MonitorTarja>>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorTarja>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaTarja> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Tarja/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorTarja>>(contentResponse);
        return result;
    }

    public async Task<ResultBase> GuardarCargaMasiva(CargaMasiva cargaMasiva)
    {

        ResultBase resultBase = new ResultBase();

        var jsonObject = JsonConvert.SerializeObject(cargaMasiva);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Tarja/GuardarCargaMasiva", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<PaginadoResult<MonitorTarja>> ObtenerListaPaginadaPorIdReferencia(ConsultaCatalogoBase<ConsultaTarja> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Tarja/ObtenerListaPaginadaPorIdReferencia", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorTarja>>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorTarjaInventario>> ObtenerListaPaginadaTarjaInventarioPorIdReferencia(ConsultaCatalogoBase<ConsultaTarja> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Tarja/ObtenerListaPaginadaTarjaInventarioPorIdReferencia", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorTarjaInventario>>(contentResponse);
        return result;
    }
}

