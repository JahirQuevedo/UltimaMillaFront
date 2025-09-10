using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTipoMonedaService : ICatTipoMonedaService
    {

        private readonly HttpClient _httpClient;

        public CatTipoMonedaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<CatTipoMoneda>> ObtenerMonedas()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoMoneda/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var monedas = JsonConvert.DeserializeObject<ICollection<CatTipoMoneda>>(content);
            ////Console.WriteLine("Monedas " + JsonConvert.SerializeObject(monedas));
            return monedas;
        }
    }
}
