using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.DTO.Vacios;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services.Peticiones
{
    public class ReferenciaService : IReferenciaService
    {
        private readonly HttpClient _httpClient;

        public ReferenciaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> BajaReferencia(RespObtenerReferenciasDTO referencia)
        {
            try
            {
                ////Console.WriteLine($"{Inicializar.UrlApi}api/Vacios/BajaReferencia/{referencia.IdReferencia}");
                var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogistico}api/Vacios/BajaReferencia/{referencia.IdReferencia}");
                ////Console.WriteLine($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
                throw;
            }
        }

        public async Task<PeticionesReferencias> CrearReferencia(PeticionesReferencias referencia)
        {

            // Configurar para ignorar propiedades nulas
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            // Serializar el objeto ignorando las propiedades nulas
            var content = JsonConvert.SerializeObject(referencia, settings);
            ////Console.WriteLine($"json Creacion referencia {content}");
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/CrearReferencia/{referencia.IdOrden}", bodyContent);

            if (response.IsSuccessStatusCode)
            {
                var contentRef = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PeticionesRespuestaDTO>(contentRef)!;
                ////Console.WriteLine($"Referencia creada {JsonConvert.SerializeObject(result)}");

                if (result.IdReferenciaALO > 0)
                {
                    referencia.IdReferencia = result.IdReferenciaALO;
                }

                return referencia;
            }

            var contentTemp = await response.Content.ReadAsStringAsync();
            var errorModel = JsonConvert.DeserializeObject<ErrorResponseDTO>(contentTemp);
            throw new Exception(errorModel.ErrorMessage);
        }

        public async Task<ICollection<RespObtenerReferenciasDTO>> GetReferencias(FiltroOrdenesReferenciasDTO filtro)
        {

            ICollection<RespObtenerReferenciasDTO> referencias;

            try
            {
                var content = JsonConvert.SerializeObject(filtro);
                var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/FiltrarReferencias", bodyContent);
                var ReferenciasContent = await response.Content.ReadAsStringAsync();
                referencias = JsonConvert.DeserializeObject<ICollection<RespObtenerReferenciasDTO>>(ReferenciasContent);
                return referencias.OrderByDescending(r => r.IdReferencia).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.Message}");
            }
            return referencias = new List<RespObtenerReferenciasDTO>();
        }

        public async Task<PeticionesReferencias> ObtenerReferencia(int idReferencia)
        {
            ////Console.WriteLine($"{Inicializar.UrlApi}APILogistico/operativo/VReferencias/ObtieneReferencia/{idReferencia}");
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/ObtieneReferencia/{idReferencia}");
            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<PeticionesReferencias>(content);
            ////Console.WriteLine(content);
            //////Console.WriteLine(JsonConvert.DeserializeObject<List<Referencia>>(content)!);
            //////Console.WriteLine(obj);
            return obj;
        }




    }
}
