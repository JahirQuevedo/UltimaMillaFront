using ALOG.Modelos.Modelos.DTO.Vacios;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.Solicitudes
{
    public partial class AsignarSoporteNavieraCmp
    {

        [Parameter] public EventCallback<List<PeticionesContenedoresClienteExternoDTO>> OnContenedores { get; set; }
        [Parameter] public List<PeticionesContenedoresClienteExternoDTO> Contenedores { get; set; }
        [Parameter] public bool DisabledButton { get; set; }

        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }

        private RadzenDataGrid<PeticionesContenedoresClienteExternoDTO> _grid;

        private List<PeticionesContenedoresClienteExternoDTO> _contenedoresCopia;
        private IList<PeticionesContenedoresClienteExternoDTO> _contenedoresSeleccionados;
        public static int ARCHIVOS_PDF = 1;
        private Radzen.FileInfo _fileInfo;
        private RadzenUpload _uploadSoporte;
        private bool _estaCargando;

        public const string RFC_NAVIERA_HYUNDAI = "HYN140815591";

        protected override async Task OnInitializedAsync()
        {
            _contenedoresCopia = new List<PeticionesContenedoresClienteExternoDTO>();
            _estaCargando = false;
            // Muestra mensaje descriptivo al usuario para la asignación del soporte de naviera
            await MostrarSwitAlert();
            // Obtiene la lista de contenedores que tienen asignados la naviera Hyundai
            GenerateCopyContenedores();
        }

        private async Task MostrarSwitAlert()
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Asiganación de soporte - Naviera Hyundai",
                Html = @"
                        <div style='text-align: justify;'>
                            Se detectaron contenedores asignados a la naviera <strong>Hyundai</strong>.<br>
                            <strong>Para continuar, debes cargar el documento soporte obligatorio</strong> 
                            para cada uno de estos contenedores mostrados a continuación.<br>
                            Sin este documento, no es posible avanzar con la solicitud de servicio.
                        </div>",
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = false
            });
        }

        private async Task CargarSoporteAsync(UploadChangeEventArgs args)
        {

            _estaCargando = true;
            // Se carga el archivo
            OnUploadFile(args);
            // Si no se ha seleccionado un documento, no realiza nada
            if (_fileInfo == null) return;
            // Asigna el soporte a los contenedores
            await AsignarSoporteAsync();
            _estaCargando = false;
        }

        private void OnUploadFile(UploadChangeEventArgs args)
        {
            try
            {
                _fileInfo = args.Files.FirstOrDefault();
                if (_fileInfo == null)
                {
                    // Des asocia el documento de los contenedores
                    // regresa el objecto SoporteNaviera a su estado inicial
                    DesAsociarDocumentoSoporte();
                    NotificarMensage("Advertencia", "Debes cargar el documento soporte para la naviera", NotificationSeverity.Warning);
                    return;
                }
                NotificarMensage("Documento asociado", $"El documento {_fileInfo.Name} se asoció correctamente a los contenedores", NotificationSeverity.Success);
            }
            catch (Exception ex)
            {
                NotificarMensage("Error al cargar soporte de naviera", ex.Message, NotificationSeverity.Error);
            }
        }

        private void GenerateCopyContenedores()
        {
            _contenedoresCopia = JsonConvert.DeserializeObject<List<PeticionesContenedoresClienteExternoDTO>>(
                JsonConvert.SerializeObject(
                    Contenedores.Where(c => c.Servicios.FirstOrDefault()?.NavieraRFC == RFC_NAVIERA_HYUNDAI).ToList()
                )
            );
        }

        private async Task AsignarSoporteAsync()
        {
            var stream = new MemoryStream();
            await _fileInfo.OpenReadStream().CopyToAsync(stream);
            //string rutaDocumento = file.Name;
            var base64 = Convert.ToBase64String(stream.ToArray());
            // Recorre la lista para asignar el documento soporte a los contenedores
            foreach (var cont in _contenedoresCopia)
            {
                var servicio = cont.Servicios.FirstOrDefault();
                servicio.SoporteNaviera = new PeticionesDocumentosClienteExternoDTO();
                servicio.SoporteNaviera.IdTipoDocumento = 10;
                servicio.SoporteNaviera.Nombre = _fileInfo.Name;
                servicio.SoporteNaviera.MimeType = _fileInfo.ContentType;
                servicio.SoporteNaviera.Base64 = base64;
            }
        }

        private void NotificarMensage(string titulo, string mensaje, NotificationSeverity notificationSeverity)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = notificationSeverity,
                Summary = titulo,
                Detail = mensaje,
                Duration = 8000
            });
        }

        private void DesAsociarDocumentoSoporte()
        {
            foreach (var cont in _contenedoresCopia)
            {
                var soporte = cont.Servicios.FirstOrDefault().SoporteNaviera;
                soporte.IdTipoDocumento = 0;
                soporte.Nombre = "";
                soporte.MimeType = "";
                soporte.Base64 = "";
            }
        }

        private void Aceptar()
        {

            if (_fileInfo == null)
            {
                NotificarMensage("¡Advertencia!", "Debes cargar el documento soporte de naviera", NotificationSeverity.Warning);
                return;
            }
            DialogService.Close(_contenedoresCopia);
        }

        //private void Cancelar() {
        //    if (_fileInfo == null) {
        //        NotificarMensage("¡Advertencia!", "Debes cargar el documento soportede de naviera", NotificationSeverity.Warning);
        //    }
        //}

    }
}
