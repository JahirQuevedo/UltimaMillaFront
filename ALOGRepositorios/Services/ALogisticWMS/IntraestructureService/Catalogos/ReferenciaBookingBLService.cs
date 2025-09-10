using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class ReferenciaBookingBLService : IReferenciaBookingBLService
{

    private readonly HttpClient _httpClient;

    public ReferenciaBookingBLService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase<MonitorReferenciaBookingBL>> Guardar(MonitorReferenciaBookingBL referenciaBookingBL)
    {
        ResultBase<MonitorReferenciaBookingBL> resultBase = new ResultBase<MonitorReferenciaBookingBL>();

        var jsonObject = JsonConvert.SerializeObject(referenciaBookingBL);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/ReferenciaBookingBL/Guardar", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<MonitorReferenciaBookingBL>>(contentResponse);

        }
        catch (Exception ex)
        {
            ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;
    }

    public async Task<ResultBase> Eliminar(int idReferenciaBookingBl)
    {
        var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogisticWMS}api/ReferenciaBookingBL/Eliminar/{idReferenciaBookingBl}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase>(contentResponse);
        return result;
    }

    public async Task<ResultBase<MonitorReferenciaBookingBL>> ObtenerPorId(int idReferenciaBookingBl)
    {
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogisticWMS}api/ReferenciaBookingBL/ObtenerPorId/{idReferenciaBookingBl}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        //////Console.WriteLine("contentResponse -> " + contentResponse);
        var result = JsonConvert.DeserializeObject<ResultBase<MonitorReferenciaBookingBL>>(contentResponse);
        return result;
    }


}
