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

        public Task<RespuestaGenericaDTO> SLOTransporteSolicitudListar()
        {
            throw new NotImplementedException();
        }
    }
}
