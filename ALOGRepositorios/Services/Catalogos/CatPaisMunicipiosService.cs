using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatPaisMunicipiosService : ICatPaisMunicipiosService
    {
        private readonly HttpClient _httpClient;

        public CatPaisMunicipiosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatPaisMunicipios>> CatPaisMunicipiosListar()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatPaisMunicipios/Listar");
                if (response.IsSuccessStatusCode)
                {
                    var jsonRead = await response.Content.ReadAsStringAsync();
                    var lstMunicipios = JsonConvert.DeserializeObject<List<CatPaisMunicipios>>(jsonRead);
                    return lstMunicipios;
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
