using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Peticiones;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Peticiones
{
    public class ContenedoresCron : IContenedoresCron
    {
        private readonly HttpClient _httpClient;
        public ContenedoresCron(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatTipoIncidenciaEvento>> ObtenerRelacionIncidenciaEvento()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatTipoIncidenciasCron/ObtenerRelacionIncidenciaEvento");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<CatTipoIncidenciaEvento>>(content);

                return result;
            }
            else
                return new List<CatTipoIncidenciaEvento>();
        }

        public async Task<RespuestaGenericaDTO> RegistrarIncidenciaCron(PeticionesContenedoresCron peticionesContenedoresCron)
        {
            try
            {
                // Enviar la solicitud a la API
                var json = JsonConvert.SerializeObject(peticionesContenedoresCron);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}VaciosControlTower/AgregarCronologiaContenedor", content);

                var ContentResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(ContentResponse);

                return result;
            }
            catch (Exception ex)
            {
                return new RespuestaGenericaDTO();
            }
        }

        //public async Task<List<CronologiaContenedorDTO>> ObtenerCronologiaPorContenedor(int idContenedor)
        //{
        //    var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}VaciosControlTower/ObtenerCronologiaPorContenedor/{idContenedor}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var json = await response.Content.ReadAsStringAsync();
        //        var respuesta = JsonConvert.DeserializeObject<List<PeticionesContenedoresCron>>(json);

        //        if (respuesta == null || !respuesta.Any())
        //            return new List<CronologiaContenedorDTO>();

        //        // Mapear a DTO
        //        var lista = respuesta
        //            .Select(x => new CronologiaContenedorDTO
        //            {
        //                //IdContenedorCron = (int)(obj["idContenedorCron"] ?? 0),
        //                //TipoIncidencia = obj["catTipoIncidenciaEvento"]?["catTipoIncidenciaCron"]?["nombre"]?.ToString(),
        //                //TipoEvento = obj["catTipoIncidenciaEvento"]?["catTipoEventosCron"]?["nombre"]?.ToString(),
        //                //FechaEvento = DateTime.Parse(obj["fechaEvento"]?.ToString() ?? DateTime.MinValue.ToString()),
        //                //Comentario = obj["comentarios"]?.ToString()
        //                IdContenedorCron = x.IdContenedorCron,
        //                IdCatTipoIncidenciaEvento = x.IdCatTipoIncidenciaEvento,
        //                IdContenedor = x.IdContenedor,
        //                TipoIncidencia = x.catTipoIncidenciaEvento?.catTipoIncidenciaCron?.Nombre ?? string.Empty,
        //                TipoEvento = x.catTipoIncidenciaEvento?.catTipoEventosCron?.Nombre ?? string.Empty,
        //                FechaEvento = x.FechaEvento,
        //                Comentario = x.Comentarios

        //            })
        //            .ToList();

        //        return lista;
        //    }

        //    return new List<CronologiaContenedorDTO>();
        //}

        public async Task<List<PeticionesContenedoresCron>> ObtenerCronologiaPorContenedor(int idContenedor, int idServicio)
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}VaciosControlTower/ObtenerCronologiaPorContenedor/{idContenedor}/{idServicio}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var lista = JsonConvert.DeserializeObject<List<PeticionesContenedoresCron>>(json);

                return lista ?? new List<PeticionesContenedoresCron>();
            }

            return new List<PeticionesContenedoresCron>();
        }


        public async Task<RespuestaGenericaDTO> BajaCronologiaporContenedor(int pIdContenedorCron)
        {
            try
            {
                var response = await _httpClient.PostAsync(
                    $"{Inicializar.UrlApiLogistico}VaciosControlTower/BajaCronologiaPorContenedor/{pIdContenedorCron}",
                    null // No se necesita body si es tipo POST con solo parámetro en la URL
                );

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(content);

                return result ?? new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = "No se pudo procesar la respuesta del servidor."
                };
            }
            catch (Exception ex)
            {
                return new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = $"Error al dar de baja el registro: {ex.Message}"
                };
            }
        }

        public async Task<RespuestaGenericaDTO> ActualizarCronologiaporContenedor(PeticionesContenedoresCron peticionesContenedoresCron)
        {
            try
            {
                var recibido = JsonConvert.SerializeObject(peticionesContenedoresCron);
                //Console.WriteLine("JSON recibido: " + recibido);

                // Serializar el objeto
                var json = JsonConvert.SerializeObject(peticionesContenedoresCron);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Enviar la solicitud POST al endpoint de actualización
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}VaciosControlTower/ActualizarCronologiaporContenedor", content);

                var contentResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(contentResponse);

                return result ?? new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = "No se pudo procesar la respuesta del servidor."
                };
            }
            catch (Exception ex)
            {
                return new RespuestaGenericaDTO
                {
                    IsSuccess = false,
                    strMensaje = $"Error al actualizar el evento: {ex.Message}"
                };
            }
        }

    }
}
