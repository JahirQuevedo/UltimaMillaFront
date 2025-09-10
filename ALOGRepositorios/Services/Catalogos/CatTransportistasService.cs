using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTransportistasService : ICatTransportistaService
    {
        private readonly HttpClient _httpClient;

        public CatTransportistasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatTransportistas>> GetTransportistas()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTransportistas/Listar");
            var json = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<List<CatTransportistas>>(json);
            return lstObj;
        }
    }
}
