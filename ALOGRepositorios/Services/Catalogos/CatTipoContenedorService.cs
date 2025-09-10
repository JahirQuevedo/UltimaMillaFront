using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTipoContenedorService : ICatTipoContenedorService
    {

        private readonly HttpClient _httpClient;

        public CatTipoContenedorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ICollection<CatTipoContenedor>> GetTiposContenedorAsync()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTiposContenedor/Listar");

            var content = await response.Content.ReadAsStringAsync();
            var tiposContenedores = JsonConvert.DeserializeObject<ICollection<CatTipoContenedor>>(content);
            return tiposContenedores;
        }
    }
}
