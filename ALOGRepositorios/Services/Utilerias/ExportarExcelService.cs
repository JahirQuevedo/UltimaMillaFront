using ALOG.Modelos.Modelos.DTO.Utilerias;
using ALOGRepositorios.Services.Utilerias.IUtilerias;
using ClienteBlazorWASM.Helpers;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net.Http.Json;


namespace ALOGRepositorios.Services.Utilerias
{
    public class ExportarExcelService : IExportarExcelService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public ExportarExcelService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task ExportarExcelAsync<T>(List<T> datos, string nombreArchivo = "Reporte.xlsx")
        {
            if (datos == null || !datos.Any()) return;

            // Serializar a JSON y luego a diccionarios
            var json = System.Text.Json.JsonSerializer.Serialize(datos);
            var diccionarios = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
            var response = await _httpClient.PostAsJsonAsync($"{Inicializar.UrlApiLogistico}Utileria/GenerarExcelDinamico", diccionarios);
            //var response = await _httpClient.PostAsJsonAsync("https://localhost:7185/Utileria/GenerarExcelDinamico", diccionarios);

            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsByteArrayAsync();
                await _jsRuntime.InvokeVoidAsync("saveAsFile", nombreArchivo, Convert.ToBase64String(contenido));
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                //Console.WriteLine($"Error en respuesta del backend: {response.StatusCode} - {error}");
                throw new Exception("Error al generar el Excel.");
            }
        }

        public async Task<List<Dictionary<string, string>>> CargarExcelAsync(MultipartFormDataContent contenido)
        {
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}Utileria/CargarExcelDinamico", contenido);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<FilaExcelDTO>>(json);

            return result.Select(f => f.Columnas).ToList(); // FilaExcelDTO tiene un Dictionary<string, string>
        }
    }
}
