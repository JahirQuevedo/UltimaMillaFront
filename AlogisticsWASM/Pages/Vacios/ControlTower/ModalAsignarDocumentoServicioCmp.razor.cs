using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.ControlTower
{
    public partial class ModalAsignarDocumentoServicioCmp
    {

        [Parameter] public SolCargarArchivoWASMDTO Documento { get; set; }
        [Parameter] public int IdCatServicio { get; set; }

        [Inject] public ICatDocumentoService CatDocumentoService { get; set; }
        [Inject] public IDocumentoService DocumentoService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public NotificationService NotificationService { get; set; }

        private ICollection<CatDocumentos> _tiposDocumento;
        private int? _idDocumentoSeleccionado;
        private CatDocumentos _documentoSeleccionado;

        private RadzenUpload _radzenUpload;
        private Radzen.FileInfo _fileInfo;
        private bool _busy;

        protected override async Task OnInitializedAsync()
        {
            _documentoSeleccionado = new CatDocumentos();
            _tiposDocumento = await CatDocumentoService.GetTiposDocumento();
            _idDocumentoSeleccionado = 0;

            if (IdCatServicio > 0)
            {

                switch (IdCatServicio)
                {
                    case 1:
                        _tiposDocumento = _tiposDocumento.Where(d => d.IdCatDocumento == 1 || d.IdCatDocumento == 2).ToList();
                        break;
                    case 2:
                        _tiposDocumento = _tiposDocumento.Where(d => d.IdCatDocumento == 2).ToList();
                        break;
                    case 4:
                        _tiposDocumento = _tiposDocumento.Where(d => d.IdCatDocumento == 3).ToList();
                        break;
                    default:
                        _tiposDocumento = new List<CatDocumentos>();
                        break;
                }

            }
        }

        private void GetSeleccionado()
        {
            _documentoSeleccionado = _tiposDocumento.FirstOrDefault(d => d.IdCatDocumento == _idDocumentoSeleccionado);
        }

        public async Task GuardarDocumentoAsync()
        {

            if (Documento == null) return;

            if (_fileInfo == null)
            {
                IJsHelper.MostrarNotificacion(NotificationService, "Debes cargar un documento", "", NotificationSeverity.Warning, 0);
                return;
            }
            _busy = true;
            GetSeleccionado();
            Documento.IdCatDocumento = _documentoSeleccionado.IdCatDocumento;
            Documento.File = _fileInfo;

            bool respuesta = await DocumentoService.SubirArchivo(Documento);
            _busy = false;
            if (respuesta)
            {
                IJsHelper.MostrarNotificacion(NotificationService, "Documento guardado", "El documento se asignó correctamente", NotificationSeverity.Success, 0);
                DialogService.Close(respuesta);
                return;
            }
            IJsHelper.MostrarNotificacion(NotificationService, "Error al guardar el documento", "Ocurrió un error inesperado al guardar el documeto. Intenta nuevamente", NotificationSeverity.Error, 0);
        }

        private async Task CargarArchivoAsync(UploadChangeEventArgs args)
        {
            try
            {
                _fileInfo = args.Files.FirstOrDefault();

                if (_fileInfo == null) return;

                using var stream = new MemoryStream();

                await _fileInfo.OpenReadStream().CopyToAsync(stream);

                var base64 = Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception ex)
            {
                IJsHelper.MostrarNotificacion(
                    NotificationService,
                    "Error al cargar el documento",
                    ex.Message,
                    NotificationSeverity.Error,
                    0
                 );
            }
        }
    }
}
