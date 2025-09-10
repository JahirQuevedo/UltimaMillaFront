using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTransportistaService : ICatTransportistaService
    {

        private readonly HttpClient _httpClient;

        public CatTransportistaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatTransportistas>> GetTransportistas()
        {

            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTransportistas/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var transportistas = JsonConvert.DeserializeObject<List<CatTransportistas>>(content);
            return transportistas;

        }
    }
}
