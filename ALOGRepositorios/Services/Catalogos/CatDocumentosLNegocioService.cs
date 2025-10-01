using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatDocumentosLNegocioService : ICatDocumentosLNegocioService
    {
        private readonly HttpClient _httpClient;

        public CatDocumentosLNegocioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatDocumentos>> ListarDocumentosLNegocio(int IdLineaNegocio)
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new RespuestaGenericaDTO();
            List<CatDocumentos> lstTipoDocumentos = new List<CatDocumentos>();
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}/CatDocumentosLNegocio/ListarDocumentosLNegocio/{IdLineaNegocio}");
                var jsonReaded = await response.Content.ReadAsStringAsync();
                respuestaGenericaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(jsonReaded);

                if (respuestaGenericaDTO.IsSuccess)
                {
                    var root = JObject.Parse(jsonReaded);
                    var lstJson = root["entidades"]?["$values"] as JArray;

                    if (lstJson != null)
                    {
                        lstTipoDocumentos = lstJson.ToObject<List<CatDocumentos>>(JsonSerializer.Create(new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                        })) ?? new List<CatDocumentos>();

                        return lstTipoDocumentos;
                    }
                    else
                    {
                        return lstTipoDocumentos;
                    }
                }
                else
                {
                    return lstTipoDocumentos;
                }
            }
            catch (Exception ex) 
            { 
                respuestaGenericaDTO.lstrErrorMessages.Add($"Error en ListarDocumentosLNegocio: {ex.Message}");
                respuestaGenericaDTO.IsSuccess = false;
                Console.WriteLine($"{respuestaGenericaDTO.lstrErrorMessages}");
                return lstTipoDocumentos;
            }
        }
    }
}
