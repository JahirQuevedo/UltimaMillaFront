using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System.Reflection.Metadata;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaModalDocumentos
    {
        [Parameter] public List<Radzen.FileInfo> Archivos { get; set; } // Archivos seleccionados previamente
        [Parameter] public string Modo { get; set; }
        [Inject] public NotificationService NotificationService { get; set; }
        [Inject] public ICatDocumentoService CatDocumentoService { get; set; }
        [Inject] public DialogService DialogService { get; set; }

        private List<DocumentoUploadItem> Documentos { get; set; } = new();
        private ICollection<CatDocumentos> _listaDocumentos;
        private ICollection<CatDocumentos> _listDocumentosFiltrado;

        protected override async Task OnInitializedAsync()
        {
            _listaDocumentos = await CatDocumentoService.GetTiposDocumento();

            if (Modo == "TRANSPORTE")
            {
                _listDocumentosFiltrado =
                    _listaDocumentos.Where(doc => doc.Acronimo == "CARTAPORTE" || doc.Acronimo == "POD")
                        .ToList();
            }

            if (Modo == "INCIDENCIA")
            {
                _listDocumentosFiltrado = _listaDocumentos.Where(doc => doc.Acronimo == "INCIDENCIA")
                    .ToList();
            }

            if (Modo == "TIMELINE")
            {
                _listDocumentosFiltrado =
                    _listaDocumentos.Where(doc =>
                        doc.Acronimo == "CARTAPORTE" || doc.Acronimo == "POD" || doc.Acronimo == "INCIDENCIA").ToList();
            }

            if (Archivos != null && Archivos.Any())
            {
                // Inicializar la lista con los archivos seleccionados
                foreach (var f in Archivos)
                {
                    if (f.Size > 2.1 * 1024 * 1024) // 10 MB en bytes
                    {
                        IJsHelper.MostrarNotificacion(
                            NotificationService,
                            "Validación",
                            $"El archivo {f.Name} excede el tamaño máximo permitido de 2 MB.",
                            NotificationSeverity.Warning,
                            5000
                        );
                        continue; // No lo agregamos a la lista
                    }

                    Documentos.Add(new DocumentoUploadItem
                    {
                        FileInfo = f,
                        NombreArchivo = f.Name,
                        SizeFile = f.Size
                    });
                }
                if(Documentos.Count == 0)
                {
                    DialogService.Close(false);
                }
            }
            else
            {
                Documentos.Add(new DocumentoUploadItem());
            }
        }


        private void AgregarDocumento()
        {
            Documentos.Add(new DocumentoUploadItem());
        }

        private void EliminarDocumento(DocumentoUploadItem item)
        {
            Documentos.Remove(item);
        }

        public async Task Guardar()
        {
            // Validar tipo de documento y archivo cargado
            var invalidos = Documentos
                .Where(d => string.IsNullOrEmpty(d.AcronimoDocumento) || d.FileInfo == null)
                .ToList();

            if (invalidos.Any())
            {
                IJsHelper.MostrarNotificacion(
                    NotificationService,
                    "Validación",
                    "Todos los documentos deben tener seleccionado un tipo para continuar.",
                    NotificationSeverity.Warning,
                    4000
                );
                return;
            }

            // Validar tamaño máximo de archivo
            var sobrepeso = Documentos.Where(d => d.SizeFile > 2.1 * 1024 * 1024).ToList();
            if (sobrepeso.Any())
            {
                var listaNombres = string.Join(", ", sobrepeso.Select(s => s.NombreArchivo));
                IJsHelper.MostrarNotificacion(
                    NotificationService,
                    "Validación",
                    $"Los siguientes archivos exceden el tamaño máximo de 10 MB: {listaNombres}.",
                    NotificationSeverity.Warning,
                    6000
                );
                return;
            }

            // Retorna los documentos al componente padre
            DialogService.Close(Documentos);

            //IJsHelper.MostrarNotificacion(
            //    NotificationService,
            //    "Éxito",
            //    "Todos los documentos se agregaron exitosamente.",
            //    NotificationSeverity.Success,
            //    3000
            //);
        }


        private void Cerrar(MouseEventArgs arg)
        {
            Archivos = new();
            DialogService.Close(false);
        }

        public class DocumentoUploadItem
        {
            public string AcronimoDocumento { get; set; } // Tipo del documento
            public string NombreArchivo { get; set; }
            public long SizeFile { get; set; }
            public Radzen.FileInfo FileInfo { get; set; }
        }
    }
}