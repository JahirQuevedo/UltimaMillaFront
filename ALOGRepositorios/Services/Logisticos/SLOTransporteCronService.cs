using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using Radzen;
using System.Text;

namespace ALOGRepositorios.Services.Logisticos
{
    public class SLOTransporteCronService : ISLOTransporteCronService
    {

        private readonly HttpClient _httpClient;
        public SLOTransporteCronService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RespuestaGenericaDTO> ActualizarSLOTransporteCron(SLOTransportesCron transportesCron)
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new RespuestaGenericaDTO();
            respuestaGenericaDTO.IsSuccess = false;
            try
            {
                var jsonSerial = JsonConvert.SerializeObject(transportesCron);
                var json = new StringContent(jsonSerial, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/ActualizarTransporteCron", json);

                if (response.IsSuccessStatusCode)
                {
                    respuestaGenericaDTO.IsSuccess = true;
                    respuestaGenericaDTO.strMensaje = "Eventualidad actualizada correctamente";
                    return respuestaGenericaDTO;
                }
                else
                {
                    respuestaGenericaDTO.IsSuccess = false;
                    respuestaGenericaDTO.lstrErrorMessages.Add("Error al Actualizar Eventualidad");
                    return respuestaGenericaDTO;
                }

            }
            catch (Exception ex)
            {
                respuestaGenericaDTO.IsSuccess = false;
                respuestaGenericaDTO.lstrErrorMessages.Add($"Error en la petición al servidor: {ex.Message}");
                return respuestaGenericaDTO;
            }
        }

        public async Task<RespuestaGenericaDTO> ObtenerPorIDTransporteCron(int idTransporteCron)
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new RespuestaGenericaDTO();

            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/ObtenerPorIDTransporteCron/{idTransporteCron}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonReaded = await response.Content.ReadAsStringAsync();
                    SLOTransportesCron transporteCron = new SLOTransportesCron();

                    transporteCron = JsonConvert.DeserializeObject<SLOTransportesCron>(jsonReaded);

                    respuestaGenericaDTO.IsSuccess = true;
                    respuestaGenericaDTO.Entidad = transporteCron;

                    return respuestaGenericaDTO;
                }
                else
                {
                    respuestaGenericaDTO.IsSuccess = false;
                    respuestaGenericaDTO.lstrErrorMessages = new List<string>
                    {
                        "Error al obtener la entidad"
                    };
                    return respuestaGenericaDTO;
                }
            }
            catch (Exception ex)
            {
                respuestaGenericaDTO.lstrErrorMessages.Add(ex.Message);
                respuestaGenericaDTO.IsSuccess = false ;
                return respuestaGenericaDTO;
            }

        }

        public async Task<RespuestaGenericaDTO> SLOTransporteCronCrear(SLOTransportesCron trasnporteCron)
        {
            RespuestaGenericaDTO respuestaGenericaDto = new();
            respuestaGenericaDto.IsSuccess = false;
            var jsonSerial = JsonConvert.SerializeObject(trasnporteCron, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            var json = new StringContent(jsonSerial, Encoding.UTF8, "application/json");
            try
            {
                var response =
                    await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/CrearTransporteCron",
                        json);
                if (response.IsSuccessStatusCode)
                {
                    var jsonReaded = await response.Content.ReadAsStringAsync();
                    respuestaGenericaDto = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(jsonReaded);
                    return respuestaGenericaDto;
                }
                else
                {
                    respuestaGenericaDto.lstrErrorMessages = new List<string>
                    {
                        "Ha ocurrido un error akl guardar la eventualidad, por favor revisar el mensaje del servidor",
                        { response.RequestMessage.ToString() }
                    };
                    return respuestaGenericaDto;
                }
            }
            catch (Exception ex)
            {
                respuestaGenericaDto.lstrErrorMessages = new List<string>
                {
                    "Ha ocurrido un error al realizar el post, validar los datos antes del envio",
                    ex.Message
                };
                return respuestaGenericaDto;
            }

        }

        public async Task<List<SLOTransportesCron>> SLOTransporteCronListar(int id)
        {
            try
            {
                var response = await
                    _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/ListarTransporteCron/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonReaded = await response.Content.ReadAsStringAsync();
                    var lstobj = JsonConvert.DeserializeObject<List<SLOTransportesCron>>(jsonReaded);
                    return lstobj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
