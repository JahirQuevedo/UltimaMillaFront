using ALOG.Modelos.Modelos.DTO;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ALOGRepositorios.Services.Logisticos
{
    public class SLOTransporteCronDocumentosService : ISLOTransporteCronDocumentosService
    {
        private readonly HttpClient _httpClient;

        public SLOTransporteCronDocumentosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RespuestaGenericaDTO> CrearSLOTransporteCronDocumeto(List<SLOTransporteCronDocumentos> lstSloTransporteCronDocumentos)
        {
            var respuestaGenericaDTO = new RespuestaGenericaDTO();

            try
            {
                // Serializar toda la lista
                var jsonSerializado = JsonConvert.SerializeObject(lstSloTransporteCronDocumentos, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                var content = new StringContent(jsonSerializado, Encoding.UTF8, "application/json");

                // POST hacia tu endpoint (que espera una lista)
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/CrearTransporteCronDocumentos", content);

                var contentResponse = await response.Content.ReadAsStringAsync();

                respuestaGenericaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentResponse);

                return respuestaGenericaDTO;
            }
            catch (Exception ex)
            {
                respuestaGenericaDTO.lstrErrorMessages.Add($"Error en CrearSLOTransporteCronDocumentos: {ex.Message}");
                respuestaGenericaDTO.IsSuccess = false;
                return respuestaGenericaDTO;
            }
        }

        public async Task<List<SLOTransporteCronDocumentos>> ObtenerSLOTranporteCronDocumentos(int IdTransporteCron)
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new RespuestaGenericaDTO();
            List<SLOTransporteCronDocumentos> lstTransporteCronDocumentos = new List<SLOTransporteCronDocumentos>();

            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/ObtenerTransporteCronDocumentos/{IdTransporteCron}");
                var json = await response.Content.ReadAsStringAsync();

                respuestaGenericaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(json);

                if (respuestaGenericaDTO.IsSuccess)
                {
                    var root = JObject.Parse(json);

                    var lstJson = root["entidades"]?["$values"] as JArray;

                    if (lstJson != null)
                    {
                        lstTransporteCronDocumentos = lstJson.ToObject<List<SLOTransporteCronDocumentos>>(JsonSerializer.Create(new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        })) ?? new List<SLOTransporteCronDocumentos>();

                        return lstTransporteCronDocumentos;
                    }
                    else
                    {
                        return lstTransporteCronDocumentos;
                    }
                }
                else
                {
                    return lstTransporteCronDocumentos;
                }

            }catch (Exception ex)
            {
                Console.WriteLine($"{respuestaGenericaDTO.lstrErrorMessages}");
                return lstTransporteCronDocumentos;
            }
        }
    }
}
