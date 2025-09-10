using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Logistica;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Integracion1G;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Orden;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ALOGRepositorios.Services.Logisticos
{
    public class SLOReferenciasService : ISLOReferenciasService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public SLOReferenciasService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IEnumerable<Ordenes>> ObtenerReferencias(FiltroOrdenesReferenciasDTO filtro)
        {
            var queryParams = new Dictionary<string, string>();

            if (filtro.IdCliente.HasValue) queryParams.Add("idCliente", filtro.IdCliente.Value.ToString());
            if (filtro.IdClienteFact.HasValue) queryParams.Add("idClienteFacturarA", filtro.IdClienteFact.Value.ToString());
            if (!string.IsNullOrWhiteSpace(filtro.ReferenciaALO)) queryParams.Add("referenciaALO", filtro.ReferenciaALO);
            if (!string.IsNullOrWhiteSpace(filtro.ReferenciaCliente)) queryParams.Add("referenciaCliente", filtro.ReferenciaCliente);
            if (filtro.IdAduana.HasValue) queryParams.Add("idAduana", filtro.IdAduana.Value.ToString());
            if (filtro.FechaRegistro.HasValue) queryParams.Add("fechaRegistro", filtro.FechaRegistro.Value.ToString("o"));
            //if (filtro.FechaEnvio1G.HasValue) queryParams.Add("fechaFin", filtro.FechaEnvio1G.Value.ToString("o"));
            if (!string.IsNullOrWhiteSpace(filtro.Estado1G)) queryParams.Add("estado1G", filtro.Estado1G);
            if (filtro.FechaEnvio1G.HasValue) queryParams.Add("fechaEnvio", filtro.FechaEnvio1G.Value.ToString("o"));


            var queryString = string.Join("&", queryParams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            var url = $"{Inicializar.UrlApiLogistico}SLOReferencias/ObtenerListaReferencias?{queryString}";

            try
            {
                //var response = await _httpClient.GetAsync<IEnumerable>(url);
                //return response ?? new List<SLOPeticionesReferencias>();
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var orden = JsonConvert.DeserializeObject<IEnumerable<Ordenes>>(content);

                    return orden;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] al consumir SLOReferencias: {ex.Message}");
                return new List<Ordenes>();
            }
            return new List<Ordenes>();
        }

        public async Task<List<SLOIntegraFacturaEnc>> ObtenerFacturas(int idOrden)
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/ObtenerFacturas/{idOrden}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<SLOIntegraFacturaEnc>>(content);
            }
            return new List<SLOIntegraFacturaEnc>();
        }

        public async Task<Ordenes> ObtenerServicios(int idOrden)
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/ObtenerServicios/{idOrden}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Ordenes>(content);
            }
            return new Ordenes();
        }



        public async Task<RespuestaGenericaDTO> CrearReferenciaALO(SLOReferenciasDTO SLOrefe)
        {
            try
            {
                // Enviar la solicitud a la API
                var json = JsonConvert.SerializeObject(SLOrefe);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/CrearReferenciaALO", content);

                var ContentResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(ContentResponse);

                if (response.IsSuccessStatusCode)
                {
                    return result;
                }
                else
                {
                    return new RespuestaGenericaDTO
                    {
                        IsSuccess = false,
                        strMensaje = $" Error del servidor: {response.StatusCode}\n{ContentResponse}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = $" Excepción: {ex.Message}"
                };

            }
        }

        public async Task<RespuestaGenericaDTO> EditarReferenciaCliente(Ordenes orden)
        {
            try
            {
                var json = JsonConvert.SerializeObject(orden);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/ActualizarReferenciaCliente", content);

                var contentResponse = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentResponse);

                if (response.IsSuccessStatusCode)
                {
                    return resultado;
                }
                else
                {
                    return new RespuestaGenericaDTO
                    {
                        IsSuccess = false,
                        strMensaje = $"Error del servidor: {response.StatusCode}\n{contentResponse}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = $"Excepción: {ex.Message}"
                };
            }
        }

        //public async Task<RespuestaGenericaDTO> GuardarServicios(List<SLOPeticionesContenedores> datos)
        //{
        //    try
        //    {
        //        var json = System.Text.Json.JsonSerializer.Serialize(datos);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/GuardarServicio", content);

        //        var contentResponse = await response.Content.ReadAsStringAsync();
        //        var resultado = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentResponse);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return resultado; 
        //        }
        //        else
        //        {
        //            var error = await response.Content.ReadAsStringAsync();
        //            return new RespuestaGenericaDTO
        //            {
        //                IsSuccess = false,
        //                strMensaje = $"Error al guardar servicios: {response.StatusCode} - {error}"
        //            };
        //        }
        //    }
        //    catch (Exception ex)

        //    {
        //        return new RespuestaGenericaDTO
        //        {
        //            IsSuccess = false,
        //            strMensaje = $"Excepción: {ex.Message}"
        //        };
        //    }
        //}

        public async Task<RespuestaGenericaDTO> GuardarServicios(List<SLOPeticionesContenedores> datos)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(datos);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/GuardarServicio", content);
                var contentResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var resultado = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentResponse);
                    return resultado;
                }
                else
                {
                    // Intenta deserializar solo strMensaje desde la respuesta de error
                    try
                    {
                        var errorObj = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentResponse);
                        var mensaje = errorObj?.strMensaje ?? $"Error {response.StatusCode} sin mensaje específico.";

                        return new RespuestaGenericaDTO
                        {
                            IsSuccess = false,
                            strMensaje = mensaje
                        };
                    }
                    catch
                    {
                        return new RespuestaGenericaDTO
                        {
                            IsSuccess = false,
                            strMensaje = $"Error al guardar servicios: {response.StatusCode} - {contentResponse}"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = $"Excepción: {ex.Message}"
                };
            }
        }

        public async Task<List<CatServicios>> ObtenerServiciosPorCoincidenciaAsync(string coincidencia)
        {
            try
            {
                var url = $"{Inicializar.UrlApiCatalogos}CatServicios/ListarCoincidencia/{Uri.EscapeDataString(coincidencia)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("🔍 JSON recibido:");
                    Console.WriteLine(json);

                    using var document = JsonDocument.Parse(json);

                    if (document.RootElement.TryGetProperty("$values", out var valuesElement) && valuesElement.ValueKind == JsonValueKind.Array)
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var lista = System.Text.Json.JsonSerializer.Deserialize<List<CatServicios>>(valuesElement.GetRawText(), options);
                        return lista ?? new List<CatServicios>();
                    }
                    else
                    {
                        Console.WriteLine("⚠️ No se encontró la propiedad $values.");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error HTTP: {response.StatusCode}");
                    Console.WriteLine($"💬 Contenido de error: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🧨 Excepción al obtener servicios: {ex.Message}");
            }

            return new List<CatServicios>();
        }

        public async Task<RespuestaGenericaDTO> ActualizarServicioAsync(SLOPeticionesServicios servicio)
        {
            try
            {
                var servicioCopia = new SLOPeticionesServicios
                {
                    IdServicio = servicio.IdServicio,
                    Cantidad = servicio.Cantidad,
                    Monto = servicio.Monto,
                    IdClienteFacturarA = servicio.IdClienteFacturarA,
                    ReferenciaClienteFactura = servicio.ReferenciaClienteFactura
                };

                var json = System.Text.Json.JsonSerializer.Serialize(servicioCopia);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/EditarServicio", content);
                var contentResponse = await response.Content.ReadAsStringAsync();

                var resultado = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentResponse);

                if (response.IsSuccessStatusCode)
                {
                    return resultado;
                }
                else
                {
                    return new RespuestaGenericaDTO
                    {
                        IsSuccess = false,
                        strMensaje = $"Error al actualizar servicio: {response.StatusCode} - {contentResponse}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = $"Excepción: {ex.Message}"
                };
            }
        }

        public async Task<RespuestaGenericaDTO> EliminarServicioAsync(int idServicio)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/EliminarServicio/{idServicio}", null);
                var contentResponse = await response.Content.ReadAsStringAsync();

                var resultado = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentResponse);

                if (response.IsSuccessStatusCode)
                {
                    return resultado;
                }
                else
                {
                    return new RespuestaGenericaDTO
                    {
                        IsSuccess = false,
                        strMensaje = $"Error al eliminar servicio: {response.StatusCode} - {contentResponse}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = $"Excepción: {ex.Message}"
                };
            }
        }

        //public async Task<RespuestaGenericaDTO> EnviarAFacturarAsync(List<SLOPeticionesServicios> factura)
        //{
        //    var jsonOptions = new JsonSerializerOptions
        //    {
        //        ReferenceHandler = ReferenceHandler.Preserve,
        //        WriteIndented = false
        //    };

        //    var json = System.Text.Json.JsonSerializer.Serialize(factura, jsonOptions);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/EnviarAFacturar", content);
        //    var responseString = await response.Content.ReadAsStringAsync();

        //    if (response.IsSuccessStatusCode)
        //        return System.Text.Json.JsonSerializer.Deserialize<RespuestaGenericaDTO>(responseString);

        //    return new RespuestaGenericaDTO
        //    {
        //        IsSuccess = false,
        //        strMensaje = $"Error al enviar factura: {response.StatusCode} - {responseString}"
        //    };
        //}

        public async Task<RespuestaGenericaDTO> EnviarAFacturarAsync(Ordenes factura)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = false
            };

            var json = System.Text.Json.JsonSerializer.Serialize(factura, jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLOReferencias/EnviarAFacturar", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                //return System.Text.Json.JsonSerializer.Deserialize<RespuestaGenericaDTO>(responseString);
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };

                return System.Text.Json.JsonSerializer.Deserialize<RespuestaGenericaDTO>(responseString, options);

            }

            return new RespuestaGenericaDTO
            {
                IsSuccess = false,
                strMensaje = $"Error al enviar factura: {response.StatusCode} - {responseString}"
            };
        }
    }
}
