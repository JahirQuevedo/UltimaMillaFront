using ALOGRepositorios.Services.Control.IControl;
using ClienteBlazorWASM.Helpers;
using System.Text;
using System.Text.Json;

namespace ALOGRepositorios.Services.Control
{
    public class ControlService : IControlService
    {

        private readonly HttpClient _httpClient;

        public ControlService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetFiltroCifrado(object filtro)
        {
            var json = JsonSerializer.Serialize(filtro);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiControl}Control/EncriptarFiltro", content);

            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {response.StatusCode} - {error}");
        }
    }
}
