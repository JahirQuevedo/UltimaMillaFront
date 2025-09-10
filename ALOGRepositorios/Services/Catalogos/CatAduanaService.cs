using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatAduanaService : ICatAduanaService
    {
        private readonly HttpClient _httpClient;

        public CatAduanaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ICollection<CatAduana>> GetAduanas()
        {

            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatAduana/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<ICollection<CatAduana>>(content);
            return lstObj;
        }


        public async Task<ICollection<RespListarCoincidenciasDTO>> GetAduanasConincidencia(string pCoincidencia)
        {

            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatAduana/ListarCoincidencia/{pCoincidencia}");

            var content = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<ICollection<RespListarCoincidenciasDTO>>(content);
            return lstObj;

        }


    }
}
