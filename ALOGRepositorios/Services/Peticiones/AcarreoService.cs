using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services.Peticiones
{
    public class AcarreoService : IAcarreoService
    {

        private HttpClient _httpClient;

        public AcarreoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<DtAcarreos> CrearAcarreo(DtAcarreos acarreo)
        {

            var jsonObject = JsonConvert.SerializeObject(acarreo);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtAcarreos/Crear", content);
            var jsonResponse = JsonConvert.SerializeObject(response);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var contentAcarreo = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<DtAcarreos>(contentAcarreo);
                    return result;
                }
                else
                {
                    var contentTemp = await response.Content.ReadAsStringAsync();
                    var errorModel = System.Text.Json.JsonSerializer.Deserialize<ErrorResponseDTO>(contentTemp);
                    throw new Exception(errorModel.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
                throw;
            }
        }

        public async Task<DtAcarreos> ObtenerAcarreo(int idAcarreo)
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtAcarreos/ObtieneAcarreos/{idAcarreo}");
            var content = await response.Content.ReadAsStringAsync();
            DtAcarreos acarreo = JsonConvert.DeserializeObject<DtAcarreos>(content)!;
            return acarreo;
        }

        public async Task<ICollection<DtAcarreos>> ObtenerAcarreos(FiltroDtAcarreosDTO pFiltro)
        {
            var jsonObject = JsonConvert.SerializeObject(pFiltro);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtAcarreos/ObtieneAcarreos", content);
            var jsonResponse = JsonConvert.SerializeObject(response);
            ICollection<DtAcarreos> acarreos = JsonConvert.DeserializeObject<ICollection<DtAcarreos>>(jsonResponse)!;
            return acarreos;
        }

        public Task<ICollection<DtAcarreos>> ObtenerAcarreos()
        {
            throw new NotImplementedException();
        }
    }
}
