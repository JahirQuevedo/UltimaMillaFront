using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTipoIMOService : ICatTipoIMOService
    {
        private readonly HttpClient _httpClient;

        public CatTipoIMOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatTipoIMO>> GetCatTipoIMO()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoIMO/Listar");
            var json = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<List<CatTipoIMO>>(json);
            return lstObj;
        }
    }
}
