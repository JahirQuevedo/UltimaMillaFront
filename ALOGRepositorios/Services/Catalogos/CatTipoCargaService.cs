using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTipoCargaService : ICatTipoCargaService
    {
        private readonly HttpClient _httpClient;

        public CatTipoCargaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CatTipoCarga>> CatTipoCargaListar()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoCarga/Listar");
            var json = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<List<CatTipoCarga>>(json);
            return lstObj;
        }
    }
}
