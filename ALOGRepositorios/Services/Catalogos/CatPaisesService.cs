using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatPaisesService : ICatPaisesService
    {
        private readonly HttpClient _httpClient;

        public CatPaisesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatPaises>> CatPaisesListar()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatPaises/Listar");
                if (response.IsSuccessStatusCode)
                {
                    var jsonReaded = await response.Content.ReadAsStringAsync();
                    var lstPaises = JsonConvert.DeserializeObject<List<CatPaises>>(jsonReaded);
                    return lstPaises;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
