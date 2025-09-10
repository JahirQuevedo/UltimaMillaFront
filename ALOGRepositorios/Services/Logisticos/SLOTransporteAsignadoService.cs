using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services.Logisticos
{
    public class SLOTransporteAsignadoService : ISLOTransporteAsignadoService
    {
        private readonly HttpClient _httpClient;

        public SLOTransporteAsignadoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RespuestaGenericaDTO> SLOTransporteAsignadoCrear(SLOTransporteAsignado objSLOTransporteAsignado)
        {
            RespuestaGenericaDTO respuesta = new RespuestaGenericaDTO();
            respuesta.IsSuccess = false;
            try
            {
                var json = JsonConvert.SerializeObject(objSLOTransporteAsignado, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resultado = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOTorreControl/CrearTransporteAsignado", content);
                var responseContent = await resultado.Content.ReadAsStringAsync();
                if (resultado.IsSuccessStatusCode)
                {
                    SLOTransporteAsignado asignado = new();
                    var datos = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
                    respuesta.IsSuccess = true;
                    respuesta.StatusCode = resultado.StatusCode;
                    respuesta.strMensaje = "Transporte asignado correctamente.";
                    asignado.IdSLOTransporteAsignado = Convert.ToInt32(datos["id"]);
                    asignado.Placas = Convert.ToString(datos["placas"]);
                    respuesta.Entidad = asignado;
                    return respuesta;
                }
                else
                {
                    respuesta.IsSuccess = false;
                    respuesta.StatusCode = resultado.StatusCode;
                    respuesta.lstrErrorMessages = new List<string>
                    {
                        $"Error al asignar transporte: {resultado.ReasonPhrase} - Detalles: {responseContent}"
                    };
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.IsSuccess = false;
                respuesta.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                respuesta.lstrErrorMessages = new List<string> { $"Error al asignar transporte: {ex.Message}" };
                return respuesta;
            }
        }
    }
}
