using ALOG.Modelos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services;

public class ServicioFotograficoService : IServicioFotograficoService
{
    private readonly HttpClient _httpClient;

    public ServicioFotograficoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultBase> Guardar(CargaArchivoFotografico cargaArchivoFotografico)
    {
        ResultBase resultBase = new ResultBase();

        var jsonObject = JsonConvert.SerializeObject(cargaArchivoFotografico);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/ServicioFotografico/Guardar", content);
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

    public async Task<ResultBase> Eliminar(int idServicioFotografico)
    {
        var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogisticWMS}api/ServicioFotografico/Eliminar/{idServicioFotografico}");
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResultBase>(contentResponse);
        return result;
    }

    public async Task<PaginadoResult<MonitorServicioFotografico>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaServicioFotografico> entidad)
    {
        var jsonObject = JsonConvert.SerializeObject(entidad);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/ServicioFotografico/ObtenerListaPaginada", content);
        var contentResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PaginadoResult<MonitorServicioFotografico>>(contentResponse);
        return result;
    }

    public async Task<ResultBase<ArchivoBase>> ObtenerZipArchivos(MultipleSeleccionArchivoBase multipleSeleccionArchivoBase)
    {

        ResultBase<ArchivoBase> resultBase = new ResultBase<ArchivoBase>();

        var jsonObject = JsonConvert.SerializeObject(multipleSeleccionArchivoBase);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/ServicioFotografico/ObtenerZipArchivos", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            resultBase = JsonConvert.DeserializeObject<ResultBase<ArchivoBase>>(contentResponse);
        }
        catch (Exception ex)
        {
            resultBase.MensajeRespuesta = $"Error al Deserializar la respuesta: {ex.Message}";
            throw;
        }

        return resultBase;

    }
}
