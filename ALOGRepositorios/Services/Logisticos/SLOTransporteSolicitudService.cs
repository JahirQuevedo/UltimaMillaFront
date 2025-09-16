using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ALOGRepositorios.Services.Logisticos
{
    public class SLOTransporteSolicitudService : ISLOTransporteSolicitudService
    {
        private readonly HttpClient _httpClient;
        public SLOTransporteSolicitudService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<RespuestaGenericaDTO> SLOTransporteSolicitudCrear(SLOTransporteSolicitud transporteSolicitud)
        {
            RespuestaGenericaDTO respuestaGenerica = new RespuestaGenericaDTO();
            var jsonObject = JsonConvert.SerializeObject(transporteSolicitud, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var resultado = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/CrearTransporteSolicitud", content);

            var respuesta = await resultado.Content.ReadAsStringAsync();
            var deserializado = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(respuesta);
            if (resultado.IsSuccessStatusCode)
            {
                //deserializado.IsSuccess = true;
                return deserializado;
            }
            else
            {
                respuestaGenerica.IsSuccess = false;
                respuestaGenerica.StatusCode = resultado.StatusCode;
                respuestaGenerica.lstrErrorMessages = new List<string>
                {
                    $"Error al crear la solicitud de transporte: {resultado.ReasonPhrase}"
                };
                return respuestaGenerica;
            }
        }

        public async Task<RespuestaGenericaDTO> SLOTransporteSolicitudFinalizar(int id)
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new RespuestaGenericaDTO();

            try
            {
                var emptyContent = new StringContent("", Encoding.UTF8, "application/json");
                var response = await _httpClient.PatchAsync($"{ Inicializar.UrlApiLogistico}SLOTorreControl/FinalizarTransporteSolicitud/{id}",emptyContent);

                if (response.IsSuccessStatusCode)
                {
                    respuestaGenericaDTO.IsSuccess = true;
                    respuestaGenericaDTO.strMensaje = $"Solicitud de Transporte {id} finalizada correctamente";
                    return respuestaGenericaDTO;
                }
                else
                {
                    respuestaGenericaDTO.IsSuccess = false;
                    respuestaGenericaDTO.lstrErrorMessages = new List<string>
                    {
                        $"Error al finalizar solicitud de transporte. {response.ReasonPhrase}"
                    };
                    return respuestaGenericaDTO;
                }
            } catch(Exception ex)
            {
                respuestaGenericaDTO.IsSuccess = false;
                respuestaGenericaDTO.lstrErrorMessages = new List<string>
                {
                    $"Error al Finalizar Solicitud de Transporte {ex.Message}"
                };

                return respuestaGenericaDTO;
            }
        }

        public Task<RespuestaGenericaDTO> SLOTransporteSolicitudListar()
        {
            throw new NotImplementedException();
        }
    }
}
