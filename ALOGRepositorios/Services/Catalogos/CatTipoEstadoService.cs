using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatTipoEstadoService : ICatTipoEstadoService
    {

        protected readonly HttpClient _httpClient;

        public CatTipoEstadoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CatTipoEstados> GetTipoEstado(int idTipoEstado)
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoEstados/Obtener/{idTipoEstado}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CatTipoEstados>(content)!;
        }

        public async Task<ICollection<CatTipoEstados>> GetTiposEstado()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoEstados/Listar");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<CatTipoEstados>>(content)!;
        }
    }
}
