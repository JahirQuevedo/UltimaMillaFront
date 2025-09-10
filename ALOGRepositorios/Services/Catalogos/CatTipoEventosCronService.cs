using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTipoEventosCronService : ICatTipoEventosCronService
    {
        private readonly HttpClient _httpClient;

        public CatTipoEventosCronService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<CatTipoEventosCron>> CatTipoEventosCronListar()
        {

            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoEventosCron/Listar");
            if (response.IsSuccessStatusCode)
            {
                var objson = await response.Content.ReadAsStringAsync();

                var lstObj = JsonConvert.DeserializeObject<List<CatTipoEventosCron>>(objson);
                return lstObj;
            }
            else
            {
                return null;
            }
        }
    }
}
