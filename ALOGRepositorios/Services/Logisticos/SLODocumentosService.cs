using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Logistica;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Radzen;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace ALOGRepositorios.Services.Logisticos
{

    //public class SLOCargarArchivo
    //{
    //    public int IdOrden { get; set; }
    //    public int IdUsuario { get; set; }
    //    public string TipoDocumento { get; set; }
    //    public string Identificador { get; set; }
    //    public IBrowserFile File { get; set; }

    //}
    public class SLOCargarArchivo
    {
        public int IdOrden { get; set; }
        public int IdUsuario { get; set; }
        public string TipoDocumento { get; set; }
        public string Identificador { get; set; }

        // Información del archivo
        public string NombreArchivo { get; set; }       // Nombre original del archivo
        public long SizeFile { get; set; }              // Tamaño en bytes
        public string ContentType { get; set; }         // MIME type (ej: "application/pdf")
        public byte[] FileBytes { get; set; }           // Contenido del archivo en bytes
    }


    public class SLODocumentosService : ISLODocumentosService
    {
        private readonly HttpClient _httpClient;

        public SLODocumentosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RespuestaGenericaDTO> SLOUploadFile(SLOCargarArchivo sloCargarArchivoDTO)
        {
            RespuestaGenericaDTO respuestaGenericaDto = new RespuestaGenericaDTO();
            respuestaGenericaDto.lstrErrorMessages.Add("Error al procesar el documeno.");
            respuestaGenericaDto.IsSuccess = false;
            try
            {
                var content = new MultipartFormDataContent();

                // Crear StreamContent a partir de los bytes
                if (sloCargarArchivoDTO.FileBytes != null)
                {
                    var streamContent = new ByteArrayContent(sloCargarArchivoDTO.FileBytes);
                    streamContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue(sloCargarArchivoDTO.ContentType ??
                                                                         "application/octet-stream");
                    content.Add(streamContent, "File", sloCargarArchivoDTO.NombreArchivo);
                }
                else
                {
                    return respuestaGenericaDto; // No hay archivo que enviar
                }

                // Agregar datos adicionales
                content.Add(new StringContent(sloCargarArchivoDTO.IdOrden.ToString()),
                    nameof(sloCargarArchivoDTO.IdOrden));
                content.Add(new StringContent(sloCargarArchivoDTO.Identificador ?? ""),
                    nameof(sloCargarArchivoDTO.Identificador));
                content.Add(new StringContent(sloCargarArchivoDTO.TipoDocumento ?? ""),
                    nameof(sloCargarArchivoDTO.TipoDocumento));
                content.Add(new StringContent(sloCargarArchivoDTO.IdUsuario.ToString()),
                    nameof(sloCargarArchivoDTO.IdUsuario));

                // Enviar al API
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLODocumentos/subirArchivo",
                    content);

                var jsonReaded = await response.Content.ReadAsStringAsync();
                respuestaGenericaDto = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(jsonReaded);

                if (response.IsSuccessStatusCode)
                {

                    return respuestaGenericaDto;
                }
                else
                {
                    return respuestaGenericaDto;
                }

            }
            catch(Exception ex)
            {
                respuestaGenericaDto.lstrErrorMessages.Add(ex.ToString());
                return respuestaGenericaDto;
            }
        }

        //public async Task<RespuestaGenericaDTO> SLOUploadFileGet(SLOCargarArchivo sloCargarArchivoDTO)
        //{
        //    RespuestaGenericaDTO respuestaGenericaDto = new RespuestaGenericaDTO();
        //    respuestaGenericaDto.IsSuccess = false;

        //    try
        //    {
        //        var content = new MultipartFormDataContent();

        //        // Crear StreamContent a partir de los bytes
        //        if (sloCargarArchivoDTO.FileBytes != null)
        //        {
        //            var streamContent = new ByteArrayContent(sloCargarArchivoDTO.FileBytes);
        //            streamContent.Headers.ContentType =
        //                new System.Net.Http.Headers.MediaTypeHeaderValue(sloCargarArchivoDTO.ContentType ??
        //                                                                 "application/octet-stream");
        //            content.Add(streamContent, "File", sloCargarArchivoDTO.NombreArchivo);
        //        }
        //        else
        //        {
        //            return respuestaGenericaDto; // No hay archivo que enviar
        //        }

        //        // Agregar datos adicionales
        //        content.Add(new StringContent(sloCargarArchivoDTO.IdOrden.ToString()),
        //            nameof(sloCargarArchivoDTO.IdOrden));
        //        content.Add(new StringContent(sloCargarArchivoDTO.Identificador ?? ""),
        //            nameof(sloCargarArchivoDTO.Identificador));
        //        content.Add(new StringContent(sloCargarArchivoDTO.TipoDocumento ?? ""),
        //            nameof(sloCargarArchivoDTO.TipoDocumento));
        //        content.Add(new StringContent(sloCargarArchivoDTO.IdUsuario.ToString()),
        //            nameof(sloCargarArchivoDTO.IdUsuario));

        //        // Enviar al API
        //        var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLODocumentos/subirArchivo",
        //            content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        [Inject] private JSRuntime JS { get; set; }
        [Inject] private NotificationService notificationService { get; set; }
        public async Task<List<SLOSolicitudesDocumentos>> sloGetFilesTask(FiltroGenericoDTO filtroGenericoDTO)
        {
            var JsonObject = JsonConvert.SerializeObject(filtroGenericoDTO);
            var content = new StringContent(JsonObject, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}SLODocumentos/listarArchivo",content);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(
                        $"Error al obtener documentos. Código {response.StatusCode}. Detalle: {json}");

                var dto = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(json);

                // Entidad puede venir como JArray
                if (dto?.Entidad is JArray arr)
                    return arr.ToObject<List<SLOSolicitudesDocumentos>>() ?? new List<SLOSolicitudesDocumentos>();

                // o como List directamente
                if (dto?.Entidad is List<SLOSolicitudesDocumentos> lista)
                    return lista;

                // fallback
                return new List<SLOSolicitudesDocumentos>();
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"[HTTP] {httpEx.Message}");

                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SLODocumentosService] sloGetFilesTask - {ex}");
                throw;
            }

        }

        public async Task<RespuestaGenericaDTO> BajaDocumento(SLOSolicitudesDocumentos archivoBaja)
        {
            RespuestaGenericaDTO respuestaGenericaDto = new RespuestaGenericaDTO();

            var response = await _httpClient.DeleteAsync($"{Inicializar.UrlApiLogistico}SLODocumentos/eliminarArchivo/{archivoBaja.DocumentoUUID}");
            var json = await response.Content.ReadAsStringAsync();
            respuestaGenericaDto = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(json);

            if (response.IsSuccessStatusCode)
            {
                respuestaGenericaDto.IsSuccess = true;
                respuestaGenericaDto.StatusCode = response.StatusCode;
                return respuestaGenericaDto;
            }
            else
            {
                respuestaGenericaDto.IsSuccess = false;
                respuestaGenericaDto.StatusCode = response.StatusCode;
                return respuestaGenericaDto;
            }
        }

        public async Task<List<SLOSolicitudesDocumentos>> SLOListarArchivosSolicitud(int IdSolicitud)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}SLODocumentos/listarArchivoSolicitud/{IdSolicitud}");

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(
                        $"Error al obtener documentos. Código {response.StatusCode}. Detalle: {json}");

                var dto = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(json);

                // Entidad puede venir como JArray
                if (dto?.Entidad is JArray arr)
                    return arr.ToObject<List<SLOSolicitudesDocumentos>>() ?? new List<SLOSolicitudesDocumentos>();

                // o como List directamente
                if (dto?.Entidad is List<SLOSolicitudesDocumentos> lista)
                    return lista;

                // fallback
                return new List<SLOSolicitudesDocumentos>();
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"[HTTP] {httpEx.Message}");

                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SLODocumentosService] sloGetFilesTask - {ex}");
                throw;
            }
        }
    }
}
