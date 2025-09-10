using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatClientesUbicacionesService : ICatClientesUbicacionesService
    {
        public readonly HttpClient _httpClient;

        public CatClientesUbicacionesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatClientesUbicaciones>> CatClientesUbicacionesListar()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatClientesUbicaciones/ListarClientesUbicaciones");
            var json = await response.Content.ReadAsStringAsync();
            var lstObj = JsonConvert.DeserializeObject<List<CatClientesUbicaciones>>(json);
            return lstObj;
        }

        public async Task<RespuestaGenericaDTO> CatClientesUbicacionesCrear(CatClientesUbicaciones ubicacionCliente)
        {
            RespuestaGenericaDTO respuestaGenericaDto = new RespuestaGenericaDTO();
            respuestaGenericaDto.IsSuccess = false;
            try
            {
                var serializado = JsonConvert.SerializeObject(ubicacionCliente, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
                var content = new StringContent(serializado, Encoding.UTF8, "application/json");
                var resultado = await _httpClient.PostAsync($"{Inicializar.UrlApiCatalogos}CatClientesUbicaciones/CrearClientesUbicaciones", content);
                if (resultado.IsSuccessStatusCode)
                {
                    respuestaGenericaDto.IsSuccess = true;
                    respuestaGenericaDto.StatusCode = resultado.StatusCode;
                    respuestaGenericaDto.strMensaje = "Ubicación registrada correctamente";
                    return respuestaGenericaDto;
                }
                else
                {
                    respuestaGenericaDto.StatusCode = resultado.StatusCode;
                    respuestaGenericaDto.lstrErrorMessages = new List<string>
                    {
                        $"Error al agregar la ubicación del cliente: {resultado.ReasonPhrase}"
                    };
                    return respuestaGenericaDto;
                }
            }
            catch (Exception ex)
            {
                respuestaGenericaDto.lstrErrorMessages = new List<string>
                {
                    $"No se pudo realizar la petición POST por un error inesperado: {ex.InnerException}"
                };
                return respuestaGenericaDto;
            }
        }
    }
}
