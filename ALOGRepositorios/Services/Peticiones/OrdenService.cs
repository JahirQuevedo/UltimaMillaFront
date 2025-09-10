using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOG.Modelos.Modelos.DTO.Vacios;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ALOGRepositorios.Services.Peticiones
{
    public class OrdenService : IOrdenService
    {

        private readonly HttpClient _httpClient;

        public OrdenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Ordenes> CrearOrden(Ordenes orden)
        {
            //////Console.WriteLine("entrando al metodo de CrearOrden");
            // Configurar para ignorar propiedades nulas
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var content = JsonConvert.SerializeObject(orden, settings);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/Ordenes/CrearOrden", bodyContent);

            if (response.IsSuccessStatusCode)
            {
                var contentOrden = await response.Content.ReadAsStringAsync();
                //////Console.WriteLine($"contentOrden {contentOrden}");
                var result = JsonConvert.DeserializeObject<Ordenes>(contentOrden);
                //////Console.WriteLine($"result {JsonConvert.SerializeObject(result)}");
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ErrorResponseDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }

        }

        public async Task<Ordenes> GetOrden(int idOrden)
        {

            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}operativo/Ordenes/obtenerOrden/{idOrden}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orden = JsonConvert.DeserializeObject<Ordenes>(content);

                return orden;
            }
            else
                return null;

        }

        public async Task<SolTicketDTO> ValidarSolicitud(SolTicketDTO solTicket)
        {

            if (solTicket == null)
            {
                //_logger.LogError("El objeto solTicket es nulo.");
                throw new ArgumentNullException(nameof(solTicket));
            }

            solTicket.Errores.Clear();
            List<string> listaErrores = new();

            if (solTicket.Servicios is null || !solTicket.Servicios.Any())
            {
                solTicket.Estatus = "Rechazado";
                return solTicket;
            }

            // Se ordena la lista de servicios por el IdCatServicio
            if (solTicket.Servicios != null && solTicket.Servicios.Any())
            {
                solTicket.Servicios = solTicket.Servicios
                    .OrderBy(s => s.IdCatServicio)
                    .ToList();
            }

            foreach (var servicio in solTicket.Servicios)
            {
                solTicket.IdCatServicio = servicio.IdCatServicio;
                var requestBody = JsonConvert.SerializeObject(solTicket);
                var bodyContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

                try
                {
                    using var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/ValidarSolicitud", bodyContent);
                    var contentResponse = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<SolTicketDTO>(contentResponse);
                    // En esta línea se recupera el objeto RespuestaGenericoDTO
                    JObject jsonObj = JObject.Parse(contentResponse);

                    bool isSuccess = jsonObj["isSuccess"]?.Value<bool>() ?? false;
                    JArray errorMessages = (JArray)jsonObj["lstrErrorMessages"]["$values"];

                    SolTicketDTO entidad = jsonObj["entidad"].ToObject<SolTicketDTO>();

                    solTicket.IdCatClienteFacturar = entidad.IdCatClienteFacturar;
                    solTicket.RazonSocialCLienteFacturar = entidad.RazonSocialCLienteFacturar;
                    listaErrores = errorMessages.Select(e => e.ToString()).Where(e => !string.IsNullOrEmpty(e) || !e.Equals("")).ToList();

                    foreach (var error in listaErrores)
                    {
                        if (error.Equals(""))
                        {
                            continue;
                        }

                        if (string.IsNullOrEmpty(error))
                        {
                            continue;
                        }

                        if (solTicket.Errores.Contains(error))
                        {
                            continue;
                        }
                        solTicket.Errores.Add(error);
                    }
                }
                catch (Exception ex)
                {
                    solTicket.Estatus = "Rechazado";
                    solTicket.Errores.Add("Ocurrió un error inesperado. Por favor, intentalo más tarde.");
                }
            }

            solTicket.Estatus = solTicket.Errores.Count() == 0 ? "Válido" : "Rechazado";

            return solTicket;
        }

        public async Task<SolTicketDTO> GenerarSolicitud(SolTicketDTO solTicket)
        {

            try
            {

                string referenciaCliente = "";

                foreach (var ser in solTicket.Servicios)
                {

                    solTicket.IdCatServicio = ser.IdCatServicio;

                    referenciaCliente = solTicket.ReferenciaCliente;
                    // Se ordena la lista de servicios por el IdCatServicio
                    if (solTicket.Servicios != null && solTicket.Servicios.Any())
                    {
                        solTicket.Servicios = solTicket.Servicios
                            .OrderByDescending(s => s.IdCatServicio)
                            .ToList();
                    }
                    var content = JsonConvert.SerializeObject(solTicket);
                    var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/GuardarSolicitud", bodyContent);

                    //if (response.IsSuccessStatusCode) {

                    var contentRef = await response.Content.ReadAsStringAsync();
                    JObject jsonObj = JObject.Parse(contentRef);
                    string respuesta = (string)jsonObj["strMensaje"]?.Value<string>() ?? "";

                    SolTicketDTO entidad = jsonObj["entidad"].ToObject<SolTicketDTO>();

                    solTicket.ReferenciaAlo = entidad.ReferenciaAlo;
                    solTicket.ReferenciaCliente = entidad.ReferenciaCliente == null ? referenciaCliente : entidad.ReferenciaCliente;
                    solTicket.Estatus = entidad.Estatus;

                    if (!string.IsNullOrEmpty(respuesta) || !respuesta.Equals(""))
                    {
                        solTicket.Errores.Add(respuesta);
                    }
                    //}

                }
            }
            catch (Exception ex)
            {
                solTicket.Errores.Add("Ocurrió un error inesperado al generar la solicitud. Por favor, inténtalo más tarde");
                solTicket.Estatus = "Rechazado";
            }

            return solTicket;
        }

        public async Task<RespuestaGenericaDTO> ObtenerPath(SolPathOrdenDTO pSolPathOrdenDTO)
        {
            //////Console.WriteLine("entrando al metodo de CrearOrden");
            // Configurar para ignorar propiedades nulas
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var content = JsonConvert.SerializeObject(pSolPathOrdenDTO, settings);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/Ordenes/obtenerPath", bodyContent);

            if (response.IsSuccessStatusCode)
            {
                var contentOrden = await response.Content.ReadAsStringAsync();
                //////Console.WriteLine($"contentOrden {contentOrden}");
                var result = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentOrden);
                //////Console.WriteLine($"result {JsonConvert.SerializeObject(result)}");
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ErrorResponseDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }
        public async Task<PeticionesRespuestaDTO> CrearOrdenServicio(PeticionesReferenciasClienteExternoDTO referencia)
        {

            PeticionesRespuestaDTO respuesta = new PeticionesRespuestaDTO();
            ////Console.WriteLine($"REferencia service {JsonConvert.SerializeObject(referencia, Formatting.Indented)}");
            try
            {
                var content = JsonConvert.SerializeObject(referencia);
                var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/CrearOrdenServicio", bodyContent);
                var contentRef = await response.Content.ReadAsStringAsync();
                respuesta = JsonConvert.DeserializeObject<PeticionesRespuestaDTO>(contentRef);
                ////Console.WriteLine($"Respuesta back {JsonConvert.SerializeObject(respuesta)}");
            }
            catch (Exception ex)
            {
                respuesta.ErrorMessages.Add(ex.Message);
            }
            return respuesta;
        }

        public async Task<PeticionesRespuestaDTO> IniciarProceso(List<int> ordenes)
        {

            List<string> errores = new List<string>();
            PeticionesRespuestaDTO respuesta = new PeticionesRespuestaDTO();

            try
            {
                var content = JsonConvert.SerializeObject(ordenes);
                var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/IniciarProcesoOrdenes", bodyContent);
                var contentRef = await response.Content.ReadAsStringAsync();
                respuesta = JsonConvert.DeserializeObject<PeticionesRespuestaDTO>(contentRef);
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
            }

            return respuesta;
        }

        public async Task<PeticionesRespuestaDTO> AgregarContenedorAReferencia(int IdReferencia, PeticionesContenedoresClienteExternoDTO contenedor)
        {
            List<string> errores = new List<string>();
            PeticionesRespuestaDTO respuesta = new PeticionesRespuestaDTO();

            try
            {
                var content = JsonConvert.SerializeObject(contenedor);
                var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/orden/{IdReferencia}/contenedores/nuevo", bodyContent);
                var contentRef = await response.Content.ReadAsStringAsync();
                respuesta = JsonConvert.DeserializeObject<PeticionesRespuestaDTO>(contentRef);
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
                //Console.WriteLine("Exception ==< " + ex.Message);
            }
            //Console.WriteLine($"Respuesta API {JsonConvert.SerializeObject(respuesta, Formatting.Indented)}");
            //Console.WriteLine($"errores API {JsonConvert.SerializeObject(errores, Formatting.Indented)}");
            return respuesta;
        }

        public async Task<PeticionesRespuestaDTO> AgregarServicioAContenedor(int IdReferencia, int IdContenedor, PeticionesServiciosClienteExternoDTO servicio)
        {
            List<string> errores = new List<string>();
            PeticionesRespuestaDTO respuesta = new PeticionesRespuestaDTO();

            try
            {
                var content = JsonConvert.SerializeObject(servicio);
                var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/orden/{IdReferencia}/{IdContenedor}/servicios/nuevo", bodyContent);
                //Console.WriteLine($"response {JsonConvert.SerializeObject(response, Formatting.Indented)}");
                var contentRef = await response.Content.ReadAsStringAsync();
                //Console.WriteLine($"contentRef {JsonConvert.SerializeObject(contentRef, Formatting.Indented)}");
                respuesta = JsonConvert.DeserializeObject<PeticionesRespuestaDTO>(contentRef);
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
                //Console.WriteLine("Exception ==< " + ex.Message);
            }
            //Console.WriteLine($"Respuesta API {JsonConvert.SerializeObject(respuesta, Formatting.Indented)}");
            //Console.WriteLine($"errores API {JsonConvert.SerializeObject(errores, Formatting.Indented)}");
            return respuesta;
        }

        public async Task<PeticionesRespuestaDTO> CancelarOrdenes(List<int> ordenes)
        {
            List<string> errores = new List<string>();
            PeticionesRespuestaDTO respuesta = new PeticionesRespuestaDTO();

            try
            {
                var content = JsonConvert.SerializeObject(ordenes);
                var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/CancelarOrdenes", bodyContent);
                //Console.WriteLine($"response {JsonConvert.SerializeObject(response, Formatting.Indented)}");
                var contentRef = await response.Content.ReadAsStringAsync();
                //Console.WriteLine($"contentRef {JsonConvert.SerializeObject(contentRef, Formatting.Indented)}");
                respuesta = JsonConvert.DeserializeObject<PeticionesRespuestaDTO>(contentRef);
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
                //Console.WriteLine("Exception ==< " + ex.Message);
            }

            return respuesta;
        }

        public async Task<PeticionesRespuestaDTO> CancelarElemento(string parametrosEncriptados)
        {
            List<string> errores = new List<string>();
            PeticionesRespuestaDTO respuesta = new PeticionesRespuestaDTO();

            try
            {
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/CancelarElemento/{parametrosEncriptados}", null);

                var contentRef = await response.Content.ReadAsStringAsync();
                //Console.WriteLine($"contentRef: {contentRef}");

                if (response.IsSuccessStatusCode)
                {
                    respuesta = JsonConvert.DeserializeObject<PeticionesRespuestaDTO>(contentRef);
                }
                else
                {
                    respuesta = new PeticionesRespuestaDTO
                    {
                        IsSuccess = false,
                        StatusCode = response.StatusCode,
                        ErrorMessages = new List<string> { contentRef }
                    };
                }
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
                //Console.WriteLine("Exception: " + ex.Message);
                respuesta = new PeticionesRespuestaDTO
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ErrorMessages = errores
                };
            }

            return respuesta;
        }

        public async Task<PeticionesRespuestaDTO> Integracion1G(int idOrden)
        {
            List<string> errores = new List<string>();
            PeticionesRespuestaDTO respuesta = new PeticionesRespuestaDTO();

            try
            {
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/Integracion1G/{idOrden}", null);
                var contentRef = await response.Content.ReadAsStringAsync();
                respuesta = JsonConvert.DeserializeObject<PeticionesRespuestaDTO>(contentRef);
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
            }
            return respuesta;
        }

        public async Task<PeticionesRespuestaDTO> GenerarAnticipo(int idOrden)
        {
            List<string> errores = new List<string>();
            PeticionesRespuestaDTO respuesta = new PeticionesRespuestaDTO();

            try
            {
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/GenerarAnticipo/{idOrden}", null);
                var contentRef = await response.Content.ReadAsStringAsync();
                respuesta = JsonConvert.DeserializeObject<PeticionesRespuestaDTO>(contentRef);
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
            }
            return respuesta;
        }

        public async Task<PeticionesContenedores> ObtenerContenedor(int idContenedor)
        {
            PeticionesContenedores contenedor = new PeticionesContenedores();
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/ObtenerContenedor/{idContenedor}");
                var contentRef = await response.Content.ReadAsStringAsync();
                contenedor = JsonConvert.DeserializeObject<PeticionesContenedores>(contentRef);
                //Console.WriteLine($"Respuesta back {JsonConvert.SerializeObject(contenedor)}");
            }
            catch (Exception ex)
            {
            }
            return contenedor;
        }
    }
}
