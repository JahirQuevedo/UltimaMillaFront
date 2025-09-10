using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ALOGRepositorios.Services.Logisticos
{
    public class SLOTControlTerrestreService : ISLOTControlTerrestreService
    {
        private readonly HttpClient _httpClient;

        public SLOTControlTerrestreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RespuestaGenericaDTO> ActualizarTControlTerrestre(SLOTControlTerrestre sloTControlTerrestre)
        {
            var respuestaGenericaDto = new RespuestaGenericaDTO { IsSuccess = false };

            try
            {
                // Crear objeto nuevo para evitar referencias circulares
                var nuevoObjeto = new SLOTControlTerrestre
                {
                    IdTControlTerrestre = sloTControlTerrestre.IdTControlTerrestre,
                    FConfirmaBooking = sloTControlTerrestre.FConfirmaBooking,
                    FUnidensitiocarga = sloTControlTerrestre.FUnidensitiocarga,
                    FUnidadencarga = sloTControlTerrestre.FUnidadencarga,
                    FFinalizaCarga = sloTControlTerrestre.FFinalizaCarga,
                    FIniciaTransito = sloTControlTerrestre.FIniciaTransito,
                    FEnFrontera = sloTControlTerrestre.FEnFrontera,
                    FInicioDespachoAA = sloTControlTerrestre.FInicioDespachoAA,
                    FFinDespachoAA = sloTControlTerrestre.FFinDespachoAA,
                    FIniciaTransitoEXPO = sloTControlTerrestre.FIniciaTransitoEXPO,
                    FPuntoDescarga = sloTControlTerrestre.FPuntoDescarga,
                    FEnProcesoDescarga = sloTControlTerrestre.FEnProcesoDescarga,
                    FFinDescarga = sloTControlTerrestre.FFinDescarga,
                    FRecepcionPOD = sloTControlTerrestre.FRecepcionPOD,
                    FFinOperacion = sloTControlTerrestre.FFinOperacion,
                    IdCatUsuarios = sloTControlTerrestre.IdCatUsuarios,
                    IdSLOSolicitud = sloTControlTerrestre.IdSLOSolicitud,
                    IdSLOTransporteAsignado = sloTControlTerrestre.IdSLOTransporteAsignado,
                    IdSLOSolicitudDet = sloTControlTerrestre.IdSLOSolicitudDet,
                    // No copiar propiedades de navegación para evitar loops
                };

                var jsonContent = JsonConvert.SerializeObject(nuevoObjeto, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(
                    $"{Inicializar.UrlApiLogistico}SLOTorreControl/ActualizarTControlTerrestre",
                    content
                );

                if (response.IsSuccessStatusCode)
                {
                    respuestaGenericaDto.IsSuccess = true;
                    respuestaGenericaDto.StatusCode = response.StatusCode;
                    respuestaGenericaDto.strMensaje = "Control terrestre actualizado correctamente";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    respuestaGenericaDto.StatusCode = response.StatusCode;
                    respuestaGenericaDto.lstrErrorMessages = new List<string>
            {
                "No se ha actualizado correctamente el control terrestre",
                $"Error del servidor: {response.ReasonPhrase}",
                $"Detalle: {errorContent}"
            };
                }
            }
            catch (Exception ex)
            {
                respuestaGenericaDto.StatusCode = HttpStatusCode.InternalServerError;
                respuestaGenericaDto.lstrErrorMessages = new List<string>
        {
            "Error inesperado al intentar actualizar el control terrestre",
            ex.Message
        };
            }

            return respuestaGenericaDto;
        }



        public async Task AltaTControlTerrestre(int id)
        {
            throw new NotImplementedException();
        }

        public async Task BajaTControlTerrestre(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RespuestaGenericaDTO> CrearTControlTerrestre(SLOTControlTerrestre sloTControlTerrestre)
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new RespuestaGenericaDTO();
            respuestaGenericaDTO.IsSuccess = false;
            var jsonContent = JsonConvert.SerializeObject(sloTControlTerrestre, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/CrearTControlTerrestre", content);
            if (response.IsSuccessStatusCode)
            {
                respuestaGenericaDTO.IsSuccess = true;
                respuestaGenericaDTO.StatusCode = HttpStatusCode.OK;
                respuestaGenericaDTO.strMensaje = "Control terrestre creado correctamente";
                return respuestaGenericaDTO;
            }
            else
            {
                respuestaGenericaDTO.StatusCode = HttpStatusCode.BadRequest;
                respuestaGenericaDTO.lstrErrorMessages = new List<string>
                {
                    "No se ha creado correctamente el control terrestre.", {response.ReasonPhrase}
                };
                return respuestaGenericaDTO;
            }

        }

        public Task<RespuestaGenericaDTO> FinalizarOperacionControlTerrestre(SLOTControlTerrestre objSlOTControlTerrestre)
        {
            throw new NotImplementedException();
        }

        public async Task<RespuestaGenericaDTO> ObtenerPorIdTControlTerreste(int id)
        {
            RespuestaGenericaDTO respuestaGenericaDTO = new();
            respuestaGenericaDTO.IsSuccess = false;
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/ObtenerPorIdTControlTerrestre/{id}");

            if (response.IsSuccessStatusCode)
            {
                var dataJson = await response.Content.ReadAsStringAsync();
                var entidades = JsonConvert.DeserializeObject<List<SLOTControlTerrestre>>(dataJson);
                respuestaGenericaDTO.Entidades = entidades.Cast<object>().ToList();
                respuestaGenericaDTO.IsSuccess = true;
                respuestaGenericaDTO.strMensaje = "Entidad obtenida correctamente";
                return respuestaGenericaDTO;
            }
            else
            {
                respuestaGenericaDTO.Entidades = null;
                respuestaGenericaDTO.lstrErrorMessages = new List<string>
                {
                    "Ocurrión un error al obtener el objeto esperado", {response.ReasonPhrase}
                };
                return respuestaGenericaDTO;
            }
        }
    }
}
