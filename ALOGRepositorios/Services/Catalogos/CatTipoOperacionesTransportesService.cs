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
    public class CatTipoOperacionesTransportesService : ICatTipoOperacionesTransportesService
    {
        private readonly HttpClient _httpClient;

        public CatTipoOperacionesTransportesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatTipoOperacionesTransportes>> GetTiposOperacionesTransportes()
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new RespuestaGenericaDTO();
            List<CatTipoOperacionesTransportes> lstTipoOperacionesTransportes = new List<CatTipoOperacionesTransportes>();
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoOperacionesTransportes/ListarCatTipoOperacionesTransportes");
                var JsonReaded = await response.Content.ReadAsStringAsync();
                respuestaGenericaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>( JsonReaded );

                if (response.IsSuccessStatusCode)
                {
                    var root = JObject.Parse( JsonReaded );

                    var lstObject = root["entidades"]?["$values"] as JArray;

                    if (lstObject != null)
                    {
                        lstTipoOperacionesTransportes = lstObject.ToObject<List<CatTipoOperacionesTransportes>>(JsonSerializer.Create(new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                        })) ?? new List<CatTipoOperacionesTransportes>();

                        return lstTipoOperacionesTransportes;
                    }
                    else
                    {
                        return lstTipoOperacionesTransportes;
                    }
                }
                else
                {
                    return lstTipoOperacionesTransportes;
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine( ex.ToString() );
                return lstTipoOperacionesTransportes;
            }
        }
    }
}
