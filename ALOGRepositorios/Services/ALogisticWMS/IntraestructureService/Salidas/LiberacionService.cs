using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class LiberacionService : ILiberacionService
{

    private readonly HttpClient _httpClient;

    public LiberacionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase> CrearOrdenSalida(MonitorLiberacionInventario monitorLiberacionInventario)
    {
        ResultBase resultBase = new ResultBase();

        var jsonObject = JsonConvert.SerializeObject(monitorLiberacionInventario);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Liberacion/CrearOrdenSalida", content);
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

    public async Task<ResultBase> EliminarLiberacion(ConsultaLiberacionInventario consultaLiberacionInventario)
    {
        ResultBase resultBase = new ResultBase();

        var jsonObject = JsonConvert.SerializeObject(consultaLiberacionInventario);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Liberacion/EliminarLiberacion", content);
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

    public async Task<ResultBase> EliminarMercanciaLiberacion(ConsultaLiberacionInventario consultaLiberacionInventario)
    {
        ResultBase resultBase = new ResultBase();

        var jsonObject = JsonConvert.SerializeObject(consultaLiberacionInventario);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Liberacion/EliminarMercanciaLiberacion", content);
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

    /// <summary>
    /// Obtener listado paginado de la Orden de Salida / Liberación   
    /// </summary>
    /// <param name="entidad"></param>
    /// <returns></returns>
    public async Task<PaginadoResult<MonitorLiberacionInventario>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaLiberacionInventario> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Liberacion/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorLiberacionInventario>>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorInventarioAlmacen>> ObtenerDetalleInventarioListaPaginada(ConsultaCatalogoBase<ConsultaLiberacionInventario> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Liberacion/ObtenerDetalleInventarioListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorInventarioAlmacen>>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorLiberacionInventario>> ObtenerLiberacionControlEmbarqueListaPaginada(ConsultaCatalogoBase<ConsultaControlEmbarque> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Liberacion/ObtenerLiberacionControlEmbarqueListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        ////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorLiberacionInventario>>(contentResponse);
        return result;
    }

    public async Task<ResultBase> AutorizarLiberacion(ConsultaLiberacionInventario consultaLiberacionInventario)
    {
        ResultBase resultBase = new ResultBase();

        var jsonObject = JsonConvert.SerializeObject(consultaLiberacionInventario);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Liberacion/AutorizarLiberacion", content);
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

    public async Task<ResultBase> SalidaAlmacen(ConsultaLiberacionInventario consultaLiberacionInventario)
    {
        ResultBase resultBase = new ResultBase();

        var jsonObject = JsonConvert.SerializeObject(consultaLiberacionInventario);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Liberacion/SalidaAlmacen", content);
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

    public async Task<ResultBase> ControlEmbarqueAlmacen(ConsultaLiberacionInventario consultaLiberacionInventario)
    {
        ResultBase resultBase = new ResultBase();

        var jsonObject = JsonConvert.SerializeObject(consultaLiberacionInventario);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/Liberacion/ControlEmbarqueAlmacen", content);
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


}
