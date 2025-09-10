using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTipoOperacionService : ICatTipoOperacionService
    {
        private readonly HttpClient _httpClient;

        public CatTipoOperacionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatTipoOperacion>> CatTipoOperacionListar()
        {
            List<CatTipoOperacion> lstTipoOperacion;
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoOperacion/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<List<CatTipoOperacion>>(content);
            return lstObj;

        }
    }
}
