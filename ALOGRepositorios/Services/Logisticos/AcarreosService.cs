using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;


namespace ALOGRepositorios.Services.Logisticos
{
    public class AcarreosService : IAcarreosService
    {
        private HttpClient _httpClient;
        public string ErrorMessage { get; set; }
        public AcarreosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Actualizar(DtAcarreos pAcarreo)
        {
            var jsonObject = JsonConvert.SerializeObject(pAcarreo);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            string url = $"{Inicializar.UrlApiLogistico}DTLogistica/DtAcarreos/Actualizar";
            var respuesta = await _httpClient.PutAsync(url, content);
            ////Console.WriteLine(respuesta);
            return respuesta.IsSuccessStatusCode;
        }



        public async Task<RespEntidadErrorDTO> CrearAcarreo(DtAcarreos acarreo)
        {
            RespEntidadErrorDTO respEntidadErrorDTO = new RespEntidadErrorDTO();
            respEntidadErrorDTO.entidad = null;
            var jsonObject = JsonConvert.SerializeObject(acarreo);
            ////Console.WriteLine(jsonObject);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtAcarreos/Crear", content);
            ////Console.WriteLine(response);
            var jsonResponse = JsonConvert.SerializeObject(response);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var contentAcarreo = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<DtAcarreos>(contentAcarreo);
                    respEntidadErrorDTO.entidad = (DtAcarreos)result;

                    return respEntidadErrorDTO;
                }
                else
                {
                    var contentTemp = await response.Content.ReadAsStringAsync();
                    var errorModel = System.Text.Json.JsonSerializer.Deserialize<ErrorResponseDTO>(contentTemp);
                    respEntidadErrorDTO.MensajeError = $"Error deserializando la respuesta: {errorModel}";
                    return respEntidadErrorDTO;

                }
            }
            catch (Exception ex)
            {
                ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
                respEntidadErrorDTO.MensajeError = $"Error deserializando la respuesta: {ex.Message}";
                return respEntidadErrorDTO;

            }
        }
        public async Task<bool> CambiarEstado(int idEncabezado, int idTipoEstado)
        {
            string url = $"{Inicializar.UrlApiLogistico}DTLogistica/DtAcarreos/CambiarEstado/{idEncabezado}/{idTipoEstado}";
            var respuesta = await _httpClient.PutAsync(url, null);
            return respuesta.IsSuccessStatusCode;
        }
        public async Task<DtAcarreos> ObtenerAcarreo(int idAcarreo)
        {
            ////Console.WriteLine($"{Inicializar.UrlApi}APILogistico/DTLogistica/DtAcarreos/Obtener/{idAcarreo}");
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtAcarreos/Obtener/{idAcarreo}");
            var content = await response.Content.ReadAsStringAsync();
            DtAcarreos acarreo = JsonConvert.DeserializeObject<DtAcarreos>(content)!;
            ////Console.WriteLine("ObtenerAcarreo: " + content);
            return acarreo;
        }

        public async Task<ICollection<RespObtenerAcarreosDTO>> ObtenerAcarreos(FiltroDtAcarreosDTO pFiltro)
        {

            ICollection<RespObtenerAcarreosDTO> encabezados = new List<RespObtenerAcarreosDTO>();

            try
            {

                var json = JsonConvert.SerializeObject(pFiltro, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore, // Ignorar valores nulos.
                    DefaultValueHandling = DefaultValueHandling.Ignore // Ignorar valores predeterminados.
                });

                ////Console.WriteLine("JSON generado:");
                ////Console.WriteLine(json);

                //var jsonObject = JsonConvert.SerializeObject(filtro, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                ////Console.WriteLine(content);
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtAcarreos/ObtieneAcarreos", content);
                ////Console.WriteLine("response:" + response);
                var contentResponse = await response.Content.ReadAsStringAsync();
                encabezados = JsonConvert.DeserializeObject<ICollection<RespObtenerAcarreosDTO>>(contentResponse)!;
                ////Console.WriteLine("encabezados:" + encabezados.Count());
                ////Console.WriteLine("encabezados2:" + encabezados);
                //foreach (var acarreo in encabezados)
                //{
                //    //var txt = "";
                //    //if (acarreo.Cliente.RazonSocial is null)
                //    //    txt = "NULO";
                //    //else txt = acarreo.Cliente.RazonSocial;
                //    ////Console.WriteLine($"Propiedad1: {acarreo.NumeroContenedor}, {acarreo.NombreCliente}");
                //}

            }
            catch (HttpRequestException ex)
            {
                encabezados = new List<RespObtenerAcarreosDTO>();
                ErrorMessage = $"Error de red: {ex.Message}";
                ////Console.WriteLine("Error de red " + ex.Message);
            }
            catch (Exception ex)
            {
                encabezados = new List<RespObtenerAcarreosDTO>();
                // Maneja otras excepciones si es necesario
                ErrorMessage = $"Error inesperado: {ex.Message}";
                ////Console.WriteLine($"Error inesperado: {ex.Message}");
            }

            encabezados = encabezados.OrderByDescending(e => e.idDtAcarreos).ToList();

            return encabezados;
        }
    }
}
