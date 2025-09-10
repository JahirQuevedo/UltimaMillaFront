using ALOG.Modelos;
using ALOG.Modelos.ModelosWMS.Reportes.Salidas;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;


namespace ALOGRepositorios.Services;

public class ReporteLiberacionService : IReporteLiberacionService
{

    private readonly HttpClient _httpClient;

    public ReporteLiberacionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<ResultBase<DocumentoBase>> ObtieneReporteTarjaSalidaLiberacion(ConsultaLiberacionReporteador datosReporteador)
    {
        ResultBase<DocumentoBase> resultBase = new ResultBase<DocumentoBase>();

        var jsonObject = JsonConvert.SerializeObject(datosReporteador);
        var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogisticWMS}api/ReporteadorLiberacion/ObtieneReporteTarjaSalidaLiberacion", content);
        var jsonResponse = JsonConvert.SerializeObject(response);
        try
        {
            var contentResponse = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("contentResponse -> " + contentResponse);
            resultBase = JsonConvert.DeserializeObject<ResultBase<DocumentoBase>>(contentResponse);

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
