using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.Solicitudes
{
    public partial class SolicitarDatosHazesaCmp
    {

        [Inject] NotificationService NotificationService { get; set; }
        [Inject] DialogService DialogService { get; set; }

        private string _nombreChofer;
        private string _numeroLicenciaChofer;
        private string _numeroUnidad;
        private string _numeroPlacas;
        private string _comentarios;
        private string _rutaDocumento;
        private List<string> _errores;

        private RadzenUpload _uploadBl;

        protected override void OnInitialized()
        {
            _errores = new List<string>();
        }

        private bool DatosValidos()
        {

            _errores.Clear();

            if (string.IsNullOrEmpty(_rutaDocumento))
                _errores.Add("Debe cargar el documento EIR de lleno");

            if (string.IsNullOrEmpty(_nombreChofer))
                _errores.Add("Debe indicar el nombre del chofer de la unidad");

            if (string.IsNullOrEmpty(_numeroLicenciaChofer))
                _errores.Add("Debe indicar el número de licencia del chofer de la unidad");

            if (string.IsNullOrEmpty(_numeroUnidad))
                _errores.Add("Debe indicar el número de la unidad");

            if (string.IsNullOrEmpty(_numeroPlacas))
                _errores.Add("Debe indicar el número de placas de la unidad");

            return _errores.Count() == 0;


        }

        private async Task CargarDocumentoAsync(UploadChangeEventArgs args)
        {
            try
            {
                var file = args.Files.FirstOrDefault();
                if (file == null)
                {
                    NotificarErrores("Archivo no válido", new List<string> { "Debes seleccionar un documento PDF." });
                    return;
                }

                var stream = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(stream);

                // Simulamos ruta en servidor (puedes cambiarla según tu backend real)
                string rutaDocumento = $"uploads/{Guid.NewGuid()}_{file.Name}";
                //string rutaDocumento = file.Name;
                //var base64 = Convert.ToBase64String(stream.ToArray());
                //string rutaDocumento = $"data:application/pdf;base64,{base64}";
                // Aquí deberías guardar el archivo en backend o en wwwroot si aplica
                // await ServicioUpload.Guardar(stream, rutaDocumento);

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Documento asociado",
                    Detail = "",
                    Duration = 4000
                });
            }
            catch (Exception ex)
            {
                NotificarErrores("Error al subir el documento", new List<string> { ex.Message });
            }
        }

        private void NotificarErrores(string titulo, List<string> errores)
        {
            string html = "<ul>";
            foreach (var e in errores)
            {
                html += $"<li>{e}</li>";
            }
            html += "</ul>";

            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = titulo,
                Detail = html,
                Duration = 50000,
                CloseOnClick = true,
                Payload = DateTime.Now
            });
        }

        private void Aceptar()
        {

            if (!DatosValidos())
            {
                NotificarErrores("Faltan datos importantes para la solicitud", _errores);
                return;
            }


            DialogService.Close(null);
        }

        private void Cancelar()
        {
            if (!DatosValidos())
            {
                NotificarErrores("Faltan datos importantes para la solicitud", _errores);
                return;
            }
        }

    }
}
