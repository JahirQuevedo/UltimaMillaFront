using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services.Logisticos
{
    public class UltimaMillaEncabezadoService : IUltimaMillaEncabezadoService
    {

        protected readonly HttpClient _httpClient;
        public string ErrorMessage { get; set; }

        public UltimaMillaEncabezadoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            ErrorMessage = string.Empty;
        }

        public async Task<ICollection<DtUltimaMillaEnc>> GetEncabezados(FiltroDtUltimaMillaDTO filtro)
        {

            ICollection<DtUltimaMillaEnc> encabezados = new List<DtUltimaMillaEnc>();

            try
            {

                var json = JsonConvert.SerializeObject(filtro, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore, // Ignorar valores nulos.
                    DefaultValueHandling = DefaultValueHandling.Ignore // Ignorar valores predeterminados.
                });

                ////Console.WriteLine("JSON generado:");
                ////Console.WriteLine(json);

                //var jsonObject = JsonConvert.SerializeObject(filtro, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                ////Console.WriteLine($"{Inicializar.UrlApi}APILogistico/DTLogistica/DtUltimaMilla/UltMillaEncObtenerSolicitudes");
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtUltimaMilla/UltMillaEncObtenerSolicitudes", content);
                var contentResponse = await response.Content.ReadAsStringAsync();
                encabezados = JsonConvert.DeserializeObject<ICollection<DtUltimaMillaEnc>>(contentResponse)!;
                //var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtUltimaMilla/UltMillaEncListar");

                //var content = await response.Content.ReadAsStringAsync();
                //var lstObj = JsonConvert.DeserializeObject<ICollection<DtUltimaMillaEnc>>(content);
                //encabezados = lstObj;

            }
            catch (HttpRequestException ex)
            {
                encabezados = new List<DtUltimaMillaEnc>();
                ErrorMessage = $"Error de red: {ex.Message}";
                ////Console.WriteLine("Error de red " + ex.Message);
            }
            catch (Exception ex)
            {
                encabezados = new List<DtUltimaMillaEnc>();
                // Maneja otras excepciones si es necesario
                ErrorMessage = $"Error inesperado: {ex.Message}";
                ////Console.WriteLine($"Error inesperado: {ex.Message}");
            }

            encabezados = encabezados.OrderByDescending(e => e.IdDtUltMillaEnc).ToList();

            return encabezados;
        }

        public async Task<DtUltimaMillaEnc> GetEncabezado(int idEncabezado)
        {

            DtUltimaMillaEnc encabezado = null;

            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtUltimaMilla/UltMillaEncObtener/{idEncabezado}");
                var content = await response.Content.ReadAsStringAsync();
                encabezado = JsonConvert.DeserializeObject<DtUltimaMillaEnc>(content)!;
                ////Console.WriteLine(content);
            }
            catch (HttpRequestException ex)
            {
                encabezado = new DtUltimaMillaEnc();
                ErrorMessage = $"Error de red: {ex.Message}";
                ////Console.WriteLine("Error de red " + ex.Message);
            }
            catch (Exception ex)
            {
                encabezado = new DtUltimaMillaEnc();
                // Maneja otras excepciones si es necesario
                ErrorMessage = $"Error inesperado: {ex.Message}";
                ////Console.WriteLine($"Error inesperado: {ex.Message}");
            }
            return encabezado;
        }

        public async Task<DtUltimaMillaEnc> CrearEncabezado(DtUltimaMillaEnc encabezado)
        {

            DtUltimaMillaEnc nuevo = new DtUltimaMillaEnc();
            //nuevo.FechaSolicitud = encabezado.FechaSolicitud;
            //nuevo.Viaje = encabezado.Viaje;
            //nuevo.IdCliente = encabezado.IdCliente;
            ////nuevo.NombreCliente = encabezado.catClientes.RazonSocial;
            //nuevo.FacturaCliente = encabezado.FacturaCliente;
            //nuevo.Bodega = encabezado.Bodega;
            //nuevo.IdCatEmpresa = encabezado.IdCatEmpresa;
            //nuevo.FechaSalida = encabezado.FechaSalida;
            //nuevo.IdTipoEstado = encabezado.IdTipoEstado;
            //nuevo.IdOrden = encabezado.IdOrden;
            //nuevo.IdCatServicio = encabezado.catServicios.IdCatServicio;
            //nuevo.catTipoTransporte = encabezado.catTipoTransporte;
            //nuevo.TipoOperacion = encabezado.TipoOperacion;
            //nuevo.Origen = encabezado.Origen;
            //nuevo.Destino = nuevo.Destino;
            //nuevo.LugarPosicionamiento = encabezado.LugarPosicionamiento;
            //nuevo.LugarEntregaDestino = encabezado.LugarEntregaDestino;
            //nuevo.FechaPosicionamiento = encabezado.FechaPosicionamiento;
            //nuevo.CargaPeligrosa = encabezado.CargaPeligrosa;            
            //nuevo.DtUltimaMillaDets = encabezado.DtUltimaMillaDets;

            var jsonObject = JsonConvert.SerializeObject(encabezado);
            ////Console.WriteLine($"CrearEncabezado {jsonObject}");
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}DTLogistica/DtUltimaMilla/UltMillaEncCrear", content);
            var jsonResponse = JsonConvert.SerializeObject(response);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var contentEncabezado = await response.Content.ReadAsStringAsync();
                    ////Console.WriteLine($"contentEncabezado {contentEncabezado}");
                    var result = JsonConvert.DeserializeObject<DtUltimaMillaEnc>(contentEncabezado);
                    return result;
                }
                else
                {
                    var contentTemp = await response.Content.ReadAsStringAsync();
                    var errorModel = System.Text.Json.JsonSerializer.Deserialize<ErrorResponseDTO>(contentTemp);
                    throw new Exception(errorModel.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CambiarTipoEstado(int idEncabezado, int idTipoEstado)
        {
            string url = $"{Inicializar.UrlApiLogistico}DTLogistica/DtUltimaMilla/UltMillaEncCambiarEstado/{idEncabezado}/{idTipoEstado}";
            var respuesta = await _httpClient.PutAsync(url, null);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Actualizar(DtUltimaMillaEnc encabezado)
        {

            bool respuesta = false;

            var jsonObject = JsonConvert.SerializeObject(encabezado);
            ////Console.WriteLine($"Editando encabezado {jsonObject}");


            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(Inicializar.UrlApiLogistico + Inicializar.DtUltimaMillaActualizarUrlApi, content);
            var jsonResponse = JsonConvert.SerializeObject(response);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var contentEncabezado = await response.Content.ReadAsStringAsync();
                    ////Console.WriteLine($"contentEncabezado {contentEncabezado}");
                    var result = JsonConvert.DeserializeObject<DtUltimaMillaEnc>(contentEncabezado);
                    respuesta = true;
                }
                else
                {
                    var contentTemp = await response.Content.ReadAsStringAsync();
                    var errorModel = System.Text.Json.JsonSerializer.Deserialize<ErrorResponseDTO>(contentTemp);
                    throw new Exception(errorModel.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ////Console.WriteLine($"Error deserializando la respuesta: {ex.Message}");
                throw;
            }
            return respuesta;
        }
    }
}
