using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTipoTransporteService : ICatTipoTransporteService
    {
        private readonly HttpClient _httpClient;

        public CatTipoTransporteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<CatTipoTransporte>> GetTipoTransporte()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoTransporte/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<ICollection<CatTipoTransporte>>(content);
            return lstObj;
        }
    }
}
