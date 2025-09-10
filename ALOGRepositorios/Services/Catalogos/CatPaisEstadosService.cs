using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatPaisEstadosService : ICatPaisEstadosService
    {
        private readonly HttpClient _httpClient;

        public CatPaisEstadosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatPaisEstados>> CatPaisEstadosListar()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatPaisEstados/Listar");
                if (response.IsSuccessStatusCode)
                {
                    var JsonRead = await response.Content.ReadAsStringAsync();
                    var lstPaisEstados = JsonConvert.DeserializeObject<List<CatPaisEstados>>(JsonRead);
                    return lstPaisEstados;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }


        }
    }
}
