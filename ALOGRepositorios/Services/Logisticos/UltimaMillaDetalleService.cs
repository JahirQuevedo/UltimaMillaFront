using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services.Logisticos
{
    public class UltimaMillaDetalleService : IUltimaMillaDetalleService
    {

        protected readonly HttpClient _httpClient;
        private ICollection<DtUltimaMillaDet> detalles;

        public UltimaMillaDetalleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            detalles = new List<DtUltimaMillaDet>();
        }

        public async Task<bool> Actualizar(DtUltimaMillaDet detalle)
        {

            bool respuesta = false;

            try
            {

                var jsonObject = JsonConvert.SerializeObject(detalle);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(Inicializar.UrlApiLogistico + Inicializar.DtUltimaMillaDetalleActualizarUrlApi, content);
                var jsonResponse = JsonConvert.SerializeObject(response);

                if (response.IsSuccessStatusCode)
                {
                    var contentDetalle = await response.Content.ReadAsStringAsync();
                    ////Console.WriteLine($"contentEncabezado {contentDetalle}");
                    var result = JsonConvert.DeserializeObject<DtUltimaMillaDet>(contentDetalle);
                    respuesta = true;
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
            return respuesta;
        }

        public async Task<ICollection<DtUltimaMillaDet>> GetDetalles()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtUltimaMilla/UMillaDetListar");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<DtUltimaMillaDet>>(content)!;
        }

        public async Task<ICollection<DtUltimaMillaDet>> GetDetalles(int idEncabezado)
        {
            string url = $"{Inicializar.UrlApiLogistico}DTLogistica/DtUltimaMilla/UltMillaEncObtenerDetalles/{idEncabezado}";
            ////Console.WriteLine(url);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<DtUltimaMillaDet>>(content)!;
        }
    }
}
