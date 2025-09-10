using ALOG.Modelos;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services.Logisticos
{
    public class SLOSolicitudesService : ISLOSolicitudesService
    {
        protected readonly HttpClient _httpClient;

        public string ErrorMessage { get; set; }

        public SLOSolicitudesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<SLOSolicitudes>> GetSolicitudes(FiltroSLOSolicitudes filtro)
        {
            ICollection<SLOSolicitudes> sloSolicitudes = new List<SLOSolicitudes>();

            try
            {
                var json = JsonConvert.SerializeObject(filtro, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/SolicitudesFiltrar", content);
                var contentResponse = await response.Content.ReadAsStringAsync();
                sloSolicitudes = JsonConvert.DeserializeObject<ICollection<SLOSolicitudes>>(contentResponse);

            }
            catch (HttpRequestException ex)
            {
                sloSolicitudes = new List<SLOSolicitudes>();
                ErrorMessage = $"Error de red: {ex.Message}";
            }
            catch (Exception ex)
            {
                sloSolicitudes = new List<SLOSolicitudes>();
                ErrorMessage = $"Error inesperado: {ex.Message}";
            }
            if (sloSolicitudes.Count < 0)
            {
                sloSolicitudes = sloSolicitudes.OrderByDescending(s => s.IdSLOSolicitud).ToList();
            }


            return sloSolicitudes;
        }

        public async Task<ICollection<SLOSolicitudes>> GetSolicitudesListar()
        {
            ICollection<SLOSolicitudes> sloListadas;
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/SolicitudesListar");
                var contentResponse = await response.Content.ReadAsStringAsync();
                sloListadas = JsonConvert.DeserializeObject<ICollection<SLOSolicitudes>>(contentResponse);
            }
            catch (HttpRequestException ex)
            {
                sloListadas = new List<SLOSolicitudes>();
                ErrorMessage = $"Error de red: {ex.Message}";
            }
            catch (Exception ex)
            {
                sloListadas = new List<SLOSolicitudes>();
                ErrorMessage = $"Error inesperado: {ex.Message}";
            }
            if (sloListadas != null)
                sloListadas = sloListadas.OrderByDescending(s => s.IdSLOSolicitud).ToList();

            return sloListadas;
        }

        public async Task<RespuestaGenericaDTO> CrearSolicitudSLO(SLOSolicitudes solicitud)
        {
            RespuestaGenericaDTO respuestaGenericaDTO;
            var jsonObject = JsonConvert.SerializeObject(solicitud, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/SolicitudesCrear", content);
            var salida = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
              
                respuestaGenericaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(salida);
                respuestaGenericaDTO.IsSuccess = true;
                return respuestaGenericaDTO;
            }
            else
            {
                respuestaGenericaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(salida);
                respuestaGenericaDTO.IsSuccess = false;
                respuestaGenericaDTO.lstrErrorMessages = new List<string>
                {
                    $"Error: {response.RequestMessage}"
                };
                return respuestaGenericaDTO;
            }

                
        }

        //public async Task<RespuestaGenericaDTO> ActualizarSolicitudSLO(SLOSolicitudes solicitud)
        //{
        //    RespuestaGenericaDTO respuestaGenericaDTO = new();

        //    var jsonObject = JsonConvert.SerializeObject(solicitud, new JsonSerializerSettings
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //        NullValueHandling = NullValueHandling.Ignore,

        //    });

        //    var content =  new StringContent(jsonObject, Encoding.UTF8, "application/json");

        //    try
        //    {
        //        var response = await _httpClient.PutAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/SolicitudesActualizar/{solicitud.IdSLOSolicitud}", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseContent = await response.Content.ReadAsStringAsync();

        //            try
        //            {
        //                respuestaGenericaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(responseContent);
        //            }
        //            catch (Exception ex)
        //            {
        //                respuestaGenericaDTO = new RespuestaGenericaDTO
        //                {
        //                    IsSuccess = true, // la llamada fue exitosa, aunque no matcheó
        //                    strMensaje = "La solicitud fue procesada, pero no se pudo mapear la respuesta.",
        //                    lstrErrorMessages = new List<string> { ex.Message, responseContent }
        //                };
        //            }
        //        }
        //        else
        //        {
        //            respuestaGenericaDTO.IsSuccess = false;
        //            respuestaGenericaDTO.lstrErrorMessages = new List<string>
        //        {
        //            $"Error al actualizar: {response.RequestMessage}"
        //        };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }                        


        //    return respuestaGenericaDTO;
        //}
        public async Task<RespuestaGenericaDTO> ActualizarSolicitudSLO(SLOSolicitudes solicitud)
        {
            var respuestaGenericaDTO = new RespuestaGenericaDTO();

            try
            {
                // Serializar el objeto evitando referencias circulares
                var jsonObject = JsonConvert.SerializeObject(solicitud,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });

                using var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var url = $"{Inicializar.UrlApiLogistico}SLOTorreControl/SolicitudesActualizar/{solicitud.IdSLOSolicitud}";

                var response = await _httpClient.PutAsync(url, content);

                // Leer respuesta cruda siempre
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"📩 Respuesta API ({response.StatusCode}): {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        respuestaGenericaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(responseContent)
                                                ?? new RespuestaGenericaDTO();

                        respuestaGenericaDTO.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        respuestaGenericaDTO = new RespuestaGenericaDTO
                        {
                            IsSuccess = true, // La llamada fue exitosa aunque no se pudo mapear
                            strMensaje = "La solicitud fue procesada, pero no se pudo mapear la respuesta.",
                            lstrErrorMessages = new List<string> { ex.Message, responseContent }
                        };
                    }
                }
                else
                {
                    respuestaGenericaDTO = new RespuestaGenericaDTO
                    {
                        IsSuccess = false,
                        strMensaje = $"Error al actualizar ({response.StatusCode}).",
                        lstrErrorMessages = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
                    };
                }
            }
            catch (Exception ex)
            {
                respuestaGenericaDTO = new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = "Excepción al ejecutar la petición.",
                    lstrErrorMessages = new List<string> { ex.Message, ex.StackTrace ?? string.Empty }
                };

                Console.WriteLine($"🔥 Excepción en ActualizarSolicitudSLO: {ex}");
            }

            return respuestaGenericaDTO;
        }


        public async Task<RespuestaGenericaDTO> BajaSolicitudSLO(int id)
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new RespuestaGenericaDTO();

            var response = await _httpClient.PutAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/SLOSolicitudesBaja/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                respuestaGenericaDTO.IsSuccess = true;
                respuestaGenericaDTO.strMensaje = $"La solicitud con id: {id} ha sido dada de baja correctamente";
                return respuestaGenericaDTO;
            }
            else
            {
                respuestaGenericaDTO.IsSuccess = false;
                respuestaGenericaDTO.lstrErrorMessages = new List<string>
                {
                    "Ha ocurrido un error al dar de baja la solicitud requeridad"
                };
                return respuestaGenericaDTO;
            }

        }

        public async Task<SLOSolicitudes> ObtenerSolicitudId(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/SolicitudesEditar{id}");
                var jsonReaded = await response.Content.ReadAsStringAsync();
                var objSloSolicitud = JsonConvert.DeserializeObject<SLOSolicitudes>(jsonReaded);
                return objSloSolicitud;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
