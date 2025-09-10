using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatDocumentoService : ICatDocumentoService
    {

        private readonly HttpClient _httpClient;

        public CatDocumentoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<CatDocumentos>> GetTiposDocumento()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatDocumentos/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var documentos = JsonConvert.DeserializeObject<ICollection<CatDocumentos>>(content);
            return documentos;
        }

    }
}

