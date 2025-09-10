using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTipoOperacionesComercioService : ICatTipoOperacionesComercioService
    {
        private readonly HttpClient _httpClient;

        public CatTipoOperacionesComercioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatTipoOperacionComercio>> GetCatTipoOperacionesComercio()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoOperacionComercio/Listar");
            var json = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<List<CatTipoOperacionComercio>>(json);
            return lstObj;
        }
    }
}
