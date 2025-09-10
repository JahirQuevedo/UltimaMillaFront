using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatMercanciasService : ICatMercanciasService
    {
        private readonly HttpClient _httpClient;

        public CatMercanciasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<CatMercancias>> GetCatMercancias()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatMercancias/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<ICollection<CatMercancias>>(content);
            return lstObj;
        }
    }
}
