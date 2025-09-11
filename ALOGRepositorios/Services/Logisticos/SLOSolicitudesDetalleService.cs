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
    public class SLOSolicitudesDetalleService : ISLOSolicitudesDetalleService
    {
        private readonly HttpClient _httpClient;

        public SLOSolicitudesDetalleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SLOSolicitudesDetalle>> SLOSolicitudesDetalleObtener(int id)
        {
            List<SLOSolicitudesDetalle> lstSLOSolicitudesDetalle = new List<SLOSolicitudesDetalle>();
            RespuestaGenericaDTO respuestaGenericaDto = new RespuestaGenericaDTO();
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/SLOSolicitudesDetalleObtener/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonReaded = await response.Content.ReadAsStringAsync();
                    respuestaGenericaDto = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(jsonReaded);
                    if (respuestaGenericaDto.IsSuccess)
                    {
                        var root = JObject.Parse(jsonReaded);
                        var lstJson = root["entidades"]?["$values"] as JArray;

                        if (lstJson != null)
                        {
                            lstSLOSolicitudesDetalle = lstJson
                    .ToObject<List<SLOSolicitudesDetalle>>(JsonSerializer.Create(new JsonSerializerSettings
                    {
                        // ignorar diferencias de mayúsculas
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    }))
                    ?? new List<SLOSolicitudesDetalle>();
                        }

                    }                    
                    return lstSLOSolicitudesDetalle;
                    
                }
                else
                {
                  return lstSLOSolicitudesDetalle;
                }

            } catch (Exception ex)
            {
                return lstSLOSolicitudesDetalle;
            } 
        }
    }
}
