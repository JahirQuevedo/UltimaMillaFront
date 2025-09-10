using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatNavieraService : ICatNavieraService
    {

        private readonly HttpClient _httpClient;

        public CatNavieraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ICollection<CatNavieras>> GetNavieras()
        {

            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatNavieras/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var navieras = JsonConvert.DeserializeObject<ICollection<CatNavieras>>(content);
            return navieras;

        }

        public async Task<ICollection<RespuestaGenericaCatalogosDTO>> GetNavierasConincidencia(string pCoincidencia)
        {

            //////Console.WriteLine($"{Inicializar.UrlApi}APICatalogos/CatPatios/ListarCoincidencia/{pCoincidencia}");
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatNavieras/ListarCoincidencia/{pCoincidencia}");
            var content = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine($"GetNavierasConincidencia: {content}");
            var clientes = JsonConvert.DeserializeObject<ICollection<RespuestaGenericaCatalogosDTO>>(content);
            return clientes;

        }

        public async Task<List<CatNavieras>> FiltrarNavierasAsync(string razonSocial, string rfc)
        {
            // Validar que al menos uno de los dos parámetros tenga valor
            bool tieneRazon = !string.IsNullOrWhiteSpace(razonSocial);
            bool tieneRfc = !string.IsNullOrWhiteSpace(rfc);

            if (!tieneRazon && !tieneRfc)
            {
                throw new ArgumentException("Debes proporcionar al menos 'razonSocial' o 'rfc' para filtrar.");
            }

            var queryParams = new List<string>();

            if (tieneRazon)
                queryParams.Add($"prazonSocial={Uri.EscapeDataString(razonSocial)}");

            if (tieneRfc)
                queryParams.Add($"prfc={Uri.EscapeDataString(rfc)}");

            string queryString = string.Join("&", queryParams);
            string url = $"{Inicializar.UrlApiCatalogos}Filtrar?{queryString}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CatNavieras>>();
            }

            // Opcional: lanza una excepción o retorna lista vacía
            throw new HttpRequestException($"Error al consumir la API: {response.ReasonPhrase}");
        }
    }
}
