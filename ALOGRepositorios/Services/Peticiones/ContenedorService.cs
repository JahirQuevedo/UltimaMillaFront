using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace ALOGRepositorios.Services.Operativo
{
    public class ContenedorService : IContenedorService
    {

        private readonly HttpClient _httpClient;

        public ContenedorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PeticionesContenedores> CrearContenedor(PeticionesContenedores contenedor, int idReferencia)
        {

            var jsonObject = JsonConvert.SerializeObject(contenedor);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/CrearContenedor/{idReferencia}", content);
            var jsonResponse = JsonConvert.SerializeObject(response);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var contentContenedor = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PeticionesContenedores>(contentContenedor);
                    //var result = System.Text.Json.JsonSerializer.Deserialize<Contenedor>(contentContenedor);
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

        public async Task<List<PeticionesReferencias>> GetContenedores(int idReferencia)
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/ObtieneReferenciaALO/{idReferencia}");
            var content = await response.Content.ReadAsStringAsync();
            List<PeticionesReferencias> referencias = JsonConvert.DeserializeObject<List<PeticionesReferencias>>(content)!;

            return referencias;
        }

        public async Task<bool> AsignarServicios(int idReferencia, int idContenedor, PeticionesServicios servicio)
        {


            servicio.IdContenedor = idContenedor;

            var jsonServicio = JsonConvert.SerializeObject(servicio);

            var jsonObject = JsonConvert.SerializeObject(servicio);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/CrearServicio/{idReferencia}/{idContenedor}", content);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    //////Console.WriteLine("1");
                    //var contentContenedor = await response.Content.ReadAsStringAsync();
                    //////Console.WriteLine("2");
                    //var result = System.Text.Json.JsonSerializer.Deserialize<Contenedor>(contentContenedor);
                    //////Console.WriteLine("3");
                    //var json = System.Text.Json.JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
                    //////Console.WriteLine("4");
                    //////Console.WriteLine("Objeto 2: " + json);
                    return true;
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

        public async Task<bool> RemoverServicio(int idReferencia, PeticionesServicios servicio)
        {
            try
            {
                // Construimos la URL con los parámetros
                var url = $"{Inicializar.UrlApiLogistico}operativo/VReferencias/BajaServicio/{idReferencia}/{servicio.IdContenedor}/{servicio.IdServicio}";
                // Realizamos la solicitud DELETE
                var response = await _httpClient.DeleteAsync(url);
                // Si la respuesta fue exitosa, retornamos true
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Opcional: manejar errores de la respuesta
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ////Console.WriteLine($"Error al eliminar el servicio: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ////Console.WriteLine($"Ocurrió un error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ActualizaFolioContenedor(SolActualizarFolioManiobraDTO pSolActualizarFolioManiobraDTO)
        {
            try
            {

                var jsonObject = JsonConvert.SerializeObject(pSolActualizarFolioManiobraDTO);
                ////Console.WriteLine(jsonObject);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                ////Console.WriteLine(content);
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/ActualizarFolioContenedor", content);
                ////Console.WriteLine($"{Inicializar.UrlApi}APILogistico/operativo/VReferencias/ActualizarFolioContenedor");
                var contentTemp = await response.Content.ReadAsStringAsync();
                RespuestaGenericaDTO respuestaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentTemp)!;
                ////Console.WriteLine("ActualizaFolioContenedor response:" + contentTemp);
                if (response.IsSuccessStatusCode && respuestaDTO.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;

            }
        }

        public async Task<RespuestaGenericaDTO> ActualizaEstadoContenedor(SolCambioEstadoDTO pSolCambioEstadoDTO)
        {
            try
            {

                var jsonObject = JsonConvert.SerializeObject(pSolCambioEstadoDTO);
                //Console.WriteLine("ActualizaEstadoContenedor:" + jsonObject);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                ////Console.WriteLine(content);
                var response = await _httpClient.PatchAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/CambiarEstadoContenedor", content);
                ////Console.WriteLine($"{Inicializar.UrlApi}APILogistico/operativo/VReferencias/CambiarEstadoContenedor");
                var contentTemp = await response.Content.ReadAsStringAsync();
                RespuestaGenericaDTO respuestaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentTemp)!;
                ////Console.WriteLine("ActualizaEStadoContenedor response:" + contentTemp);

                return respuestaDTO;

            }
            catch (Exception ex)
            {

                return null;

            }
        }

        public async Task<PeticionesContenedores> ObtenerContenedor(PeticionesContenedores pContenedor)
        {
            List<string> lstResult = new List<string>();
            RespuestaGenericaDTO respuestaDTO = new RespuestaGenericaDTO();
            try
            {

                var jsonObject = JsonConvert.SerializeObject(pContenedor);

                ////Console.WriteLine(content);
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/ObtenerContenedor/{pContenedor.IdContenedor}");
                ////Console.WriteLine($"{Inicializar.UrlApiLogistico}operativo/VReferencias/ObtenerContenedor/{pContenedor.IdContenedor}");
                var contentTemp = await response.Content.ReadAsStringAsync();
                var objresp = JsonConvert.DeserializeObject<PeticionesContenedores>(contentTemp)!;
                ////Console.WriteLine("ObtenerContenedor response:" + contentTemp);
                return objresp;

            }
            catch (Exception ex)
            {


                return null;

            }
        }

        public async Task<List<string>> ActualizaEstadoServicio(SolCambioEstadoDTO pSolCambioEstadoDTO)
        {
            List<string> lstResult = new List<string>();

            try
            {

                var jsonObject = JsonConvert.SerializeObject(pSolCambioEstadoDTO);
                ////Console.WriteLine(jsonObject);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                ////Console.WriteLine(content);
                var response = await _httpClient.PatchAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/CambiarEstadoServicio", content);
                ////Console.WriteLine($"{Inicializar.UrlApi}APILogistico/operativo/VReferencias/CambiarEstadoServicio");
                var contentTemp = await response.Content.ReadAsStringAsync();
                RespuestaGenericaDTO respuestaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentTemp)!;
                ////Console.WriteLine("ActualizaEStadoServicio response:" + contentTemp);
                if (respuestaDTO.IsSuccess)
                {
                    return lstResult;
                }
                else
                {
                    lstResult.AddRange(respuestaDTO.lstrErrorMessages);

                    return lstResult;
                }
            }
            catch (Exception ex)
            {

                lstResult.Add(ex.Message);

                return lstResult;

            }
        }

        public async Task<List<string>> ActualizaEstadoTodosContenedores(SolCambioEstadoDTO pSolCambioEstadoDTO)
        {
            #region Variables
            List<string> lstResult = new List<string>();
            #endregion Variables
            try
            {

                var jsonObject = JsonConvert.SerializeObject(pSolCambioEstadoDTO);
                ////Console.WriteLine(jsonObject);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                ////Console.WriteLine(content);
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/CambiarEstadoTodosContenedor", content);
                ////Console.WriteLine($"{Inicializar.UrlApi}APILogistico/operativo/VReferencias/CambiarEstadoTodosContenedor");
                var contentTemp = await response.Content.ReadAsStringAsync();
                RespuestaGenericaDTO respuestaDTO = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentTemp)!;
                ////Console.WriteLine("ActualizaTodosContenedores response:" + contentTemp);
                ////Console.WriteLine("ActualizaTodosContenedores response:" + respuestaDTO.lstrErrorMessages);

                if (response.IsSuccessStatusCode && respuestaDTO.IsSuccess)
                {

                    return lstResult;
                }
                else
                {



                    lstResult.AddRange(respuestaDTO.lstrErrorMessages);
                    ////Console.WriteLine("resultadows:" + lstResult);
                    return lstResult;
                }
            }
            catch (Exception ex)
            {

                return lstResult;

            }
        }

        public async Task<RespuestaGenericaDTO> AsignarPatio(PeticionesContenedores contenedor)
        {

            RespuestaGenericaDTO respuesta = new RespuestaGenericaDTO();

            contenedor.catClientes = null;
            contenedor.catPatios = null;
            contenedor.catAduana = null;
            contenedor.catNavieras = null;
            contenedor.catClientesFacturarA = null;
            contenedor.catReferenciaEstado = null;
            contenedor.PeticionesReferencias = null;

            foreach (var servicio in contenedor.Servicios)
            {
                servicio.catServicios = null;
                servicio.catReferenciaEstado = null;
                servicio.PeticionesContenedores = null;
                servicio.catClientesFacturarA = null;
            }

            try
            {
                var jsonObject = JsonConvert.SerializeObject(contenedor);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}VaciosControlTower/ActualizarPatio", content);
                var contentRef = await response.Content.ReadAsStringAsync();
                respuesta = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentRef)!;
            }
            catch (Exception ex)
            {
                respuesta.StatusCode = System.Net.HttpStatusCode.BadRequest;
                respuesta.IsSuccess = false;
                respuesta.lstrErrorMessages.Add(ex.Message);
            }
            return respuesta;
        }
    }
}
