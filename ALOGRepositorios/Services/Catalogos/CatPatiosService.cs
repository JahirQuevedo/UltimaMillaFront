using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatPatiosService : ICatPatiosServices
    {
        private readonly HttpClient _httpClient;

        public CatPatiosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ICollection<CatPatios>> GetPatios()
        {

            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatPatios/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var clientes = JsonConvert.DeserializeObject<ICollection<CatPatios>>(content);
            return clientes;

        }

        public async Task<ICollection<RespuestaGenericaCatalogosDTO>> GetPatiosConincidencia(string pCoincidencia, int? pIdAduana)
        {
            pIdAduana = pIdAduana == null ? 0 : pIdAduana;
            ////Console.WriteLine($"{Inicializar.UrlApi}APICatalogos/CatPatios/ListarCoincidencia/{pCoincidencia}");
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatPatios/Filtrar/{pCoincidencia}/{pIdAduana}");

            var content = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine($"GetPatiosConincidencia: {content}");
            var clientes = JsonConvert.DeserializeObject<ICollection<RespuestaGenericaCatalogosDTO>>(content);
            return clientes;

        }

        public async Task<ICollection<CatPatios>> ObtenerListaPatiosFiltrados(string parametroEncriptado)
        {

            ICollection<CatPatios> _patios = new List<CatPatios>();

            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatPatios/ObtenerListaPatiosFiltrados/{parametroEncriptado}");
                var content = await response.Content.ReadAsStringAsync();
                _patios = JsonConvert.DeserializeObject<ICollection<CatPatios>>(content);
            }
            catch (Exception ex)
            {

            }
            return _patios;
        }
    }
}
