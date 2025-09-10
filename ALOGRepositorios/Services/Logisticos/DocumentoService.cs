using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace ALOGRepositorios.Services.Logisticos
{

    public class SolCargarArchivoWASMDTO
    {
        public int IdOrden { get; set; }
        public int IdReferencia { get; set; }
        public int IdContenedor { get; set; }
        public int IdServicio { get; set; }
        public int IdCatDocumento { get; set; }
        public int IdCatLineaNegocio { get; set; }
        public IBrowserFile File { get; set; }
    }

    public class DocumentoService : IDocumentoService
    {

        private readonly HttpClient _httpClient;

        public DocumentoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SubirArchivo(SolCargarArchivoWASMDTO dto)
        {
            var content = new MultipartFormDataContent();

            var streamContent = new StreamContent(dto.File.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.File.ContentType);
            content.Add(streamContent, "File", dto.File.Name);

            content.Add(new StringContent(dto.IdOrden.ToString()), nameof(dto.IdOrden));
            content.Add(new StringContent(dto.IdReferencia.ToString()), nameof(dto.IdReferencia));
            content.Add(new StringContent(dto.IdContenedor.ToString()), nameof(dto.IdContenedor));
            content.Add(new StringContent(dto.IdServicio.ToString()), nameof(dto.IdServicio));
            content.Add(new StringContent(dto.IdCatDocumento.ToString()), nameof(dto.IdCatDocumento));
            content.Add(new StringContent(dto.IdCatLineaNegocio.ToString()), nameof(dto.IdCatLineaNegocio));

            var response = await _httpClient.PostAsync($"{Inicializar.UrlApiLogistico}Documentos/subirArchivo", content);
            return response.IsSuccessStatusCode;
        }
    }
}
