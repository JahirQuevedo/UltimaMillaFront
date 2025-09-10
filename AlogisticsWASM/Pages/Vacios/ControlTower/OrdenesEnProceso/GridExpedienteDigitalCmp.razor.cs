using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using ALOGRepositorios.Services.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Net.Http.Headers;

namespace AlogisticsWASM.Pages.Vacios.ControlTower.OrdenesEnProceso
{
    public partial class GridExpedienteDigitalCmp
    {

        [Parameter] public PeticionesServicios Servicio { get; set; }
        [Parameter] public Ordenes Orden { get; set; }
        [Inject] public IDocumentoService DocumentoService { get; set; }
        [Inject] public HttpClient Http { get; set; }
        [Inject] IJSRuntime JS { get; set; }

        private RadzenDataGrid<PeticionesDocumentos> _gridDocumentos;
        private List<PeticionesDocumentos> _documentos;
        private RadzenUpload _uploadFile;
        private Radzen.FileInfo _fileInfo;

        protected override async Task OnParametersSetAsync()
        {
            _documentos = Servicio.Documentos.ToList();
            //Console.WriteLine($"_documentos OnParametersSetAsync => {JsonConvert.SerializeObject(_documentos, Formatting.Indented)}");
            if (_gridDocumentos != null)
                await _gridDocumentos.Reload();
        }

        private async Task AbrirDocumento(PeticionesDocumentos doc)
        {

            var response = await Http.GetAsync($"{Inicializar.UrlApiLogistico}Documentos/obtenerArchivo/{doc.DocumentoUUID}");
            if (response.IsSuccessStatusCode)
            {

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var base64 = Convert.ToBase64String(fileBytes);
                var fileUrl = $"data:application/octet-stream;base64,{base64}";
                //await JS.InvokeVoidAsync("window.open", fileUrl, "_blank");
                await JS.InvokeVoidAsync("downloadFile", doc.NombreDocumento, fileUrl);
            }
            else
            {
                // Manejar el error
            }
        }

        private async Task SubirArchivo(InputFileChangeEventArgs e)
        {
            var archivo = e.File;

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(archivo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);

            content.Add(fileContent, "File", archivo.Name);
            content.Add(new StringContent("123"), "IdOrden");
            content.Add(new StringContent("456"), "IdReferencia");
            // etc...

            var response = await Http.PostAsync("https://tu-api/api/documentos/subir", content);
        }

        private async Task AgregarDocumento()
        {

            SolCargarArchivoWASMDTO archivo = new SolCargarArchivoWASMDTO();

            archivo.IdOrden = Orden.IdOrden;
            archivo.IdReferencia = Orden.peticionesReferencias.FirstOrDefault().IdReferencia;
            archivo.IdContenedor = Servicio.IdContenedor;
            archivo.IdServicio = Servicio.IdServicio;
            archivo.IdCatDocumento = 2;
            archivo.IdCatLineaNegocio = 1;
            archivo.File = _fileInfo;

            await DocumentoService.SubirArchivo(archivo);

            //Console.WriteLine($"Servicio {JsonConvert.SerializeObject(Servicio, Formatting.Indented)}");
            //Console.WriteLine($"IdOrden {Orden.IdOrden}");
            //Console.WriteLine($"IdReferencia {Orden.peticionesReferencias.FirstOrDefault().IdReferencia}");
        }

        private async Task OnDocumentoCargado(UploadChangeEventArgs args)
        {
            try
            {

                _fileInfo = args.Files.FirstOrDefault();
                if (_fileInfo == null)
                {
                    return;
                }

                using var stream = new MemoryStream();
                await _fileInfo.OpenReadStream().CopyToAsync(stream);

                // Simulamos ruta en servidor (puedes cambiarla según tu backend real)
                string rutaDocumento = $"uploads/{Guid.NewGuid()}_{_fileInfo.Name}";
                //string rutaDocumento = file.Name;
                var base64 = Convert.ToBase64String(stream.ToArray());

                AgregarDocumento();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
