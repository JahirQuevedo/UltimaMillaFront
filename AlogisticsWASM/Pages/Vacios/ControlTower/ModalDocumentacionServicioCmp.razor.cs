using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Utilerias;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using AlogisticsWASM.Layout.Utilerias;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services.Control.IControl;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ClienteBlazorWASM.Helpers;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.ControlTower
{
    public partial class ModalDocumentacionServicioCmp
    {
        [Parameter] public PeticionesContenedores Contenedor { get; set; }
        [Parameter] public Ordenes Orden { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }
        [Inject] public HttpClient Http { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        [Inject] public IControlService ControlService { get; set; }
        [Inject] public IOrdenService OrdenService { get; set; }
        [Inject] public NotificationService NotificationService { get; set; }

        private RadzenDataGrid<PeticionesServicios> _gridServicios;

        private List<PeticionesServicios> _servicios;
        private List<CatReferenciaEstado> _estadosReferencia;
        private UtileriasPage _utileriasPage;
        private FiltrosCancelacionElementoDTO _filtroCancelacion;

        protected override void OnInitialized()
        {
            _filtroCancelacion = new FiltrosCancelacionElementoDTO();
            _estadosReferencia = new List<CatReferenciaEstado>();
            _servicios = new List<PeticionesServicios>();
            _utileriasPage = new UtileriasPage();

            _estadosReferencia = _utileriasPage.ObtenerEstadosReferencia();
        }

        protected override void OnParametersSet()
        {

            _servicios = Contenedor.Servicios.ToList();

            if (_servicios.Count() > 0)
            {
                foreach (var servicio in _servicios)
                {
                    var estados = _estadosReferencia.Where(estado => estado.IdCatReferenciaEstado == servicio.IdEstadoServicio);
                    servicio.catReferenciaEstado = estados.FirstOrDefault() ?? new CatReferenciaEstado() { Nombre = "" };
                    foreach (var documento in servicio.Documentos)
                    {
                        documento.TipoDocumentoNombre = GetTipoDocumento(documento.IdTipoDocumento);
                    }
                }
            }
        }

        private void OnRowRender(RowRenderEventArgs<PeticionesServicios> args)
        {
            args.Expandable = args.Data.Documentos?.Any() == true;
        }

        private async Task AbrirDocumento(PeticionesDocumentos doc)
        {

            var response = await Http.GetAsync($"{Inicializar.UrlApiLogistico}Documentos/obtenerArchivo/{doc.DocumentoUUID}");

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var base64 = Convert.ToBase64String(fileBytes);
                var fileUrl = $"data:application/octet-stream;base64,{base64}";
                await JS.InvokeVoidAsync("downloadFile", doc.NombreDocumento, fileUrl);
            }
            else
            {
                // Manejar el error
            }
        }

        private string GetTipoDocumento(int idTipoDocumento)
        {

            return idTipoDocumento switch
            {
                1 => "MANIOBRA DE VACIO",
                2 => "EIR DE VACIO",
                3 => "CARTA CORTE DE DEMORAS",
                4 => "RECUPERACION DE GARANTIAS",
                5 => "FORMATO ICA",
                6 => "FACTURA CXP",
                7 => "SOPORTES ANTICIPOS",
                8 => "EIR DE LLENO",
                9 => "BL",
                10 => "SOPORTE LOGISTICO NAVIERA",
            };
        }

        private async Task CancelarServicioAsync(PeticionesServicios servicio)
        {
            var result = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "¡Advertencia!",
                Text = $"¿Está seguro de cancelar el servicio {servicio.DescServicio}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí",
                CancelButtonText = "No"
            });

            if (string.IsNullOrEmpty(result.Value))
            {
                return;
            }

            //string comentario = await MostrarModalComentarios();

            //if (string.IsNullOrEmpty(comentario)) {
            //    return;
            //}

            _filtroCancelacion.IdContenedor = Contenedor.IdContenedor;
            _filtroCancelacion.IdOrden = Orden.IdOrden;
            _filtroCancelacion.IdServicio = servicio.IdServicio;

            string filtroCifrado = await ControlService.GetFiltroCifrado(_filtroCancelacion);
            var respuestaCancelacion = await OrdenService.CancelarElemento(filtroCifrado);

            if (respuestaCancelacion.IsSuccess && respuestaCancelacion.StatusCode == System.Net.HttpStatusCode.OK && respuestaCancelacion.ErrorMessages.Count() == 0)
            {
                IJsHelper.MostrarNotificacion(
                    NotificationService,
                    "Servicio cancelado con éxito",
                    $"Se canceló el servicio {servicio.catServicios.Nombre}",
                    NotificationSeverity.Success,
                    0
                );
            }
            else
            {

                var mensaje = string.Join("<br/>", respuestaCancelacion.ErrorMessages);

                IJsHelper.MostrarNotificacion(
                        NotificationService,
                        "El servicio no se pudo cancelar por lo siguiente",
                        mensaje,
                        NotificationSeverity.Error,
                        0
                    );
            }

            if (_servicios.Contains(servicio))
            {
                _servicios.Remove(servicio);

                if (Contenedor.Servicios.Count() == 0)
                {
                    // Se deberá cancelar el contenedor
                }
                await _gridServicios.Reload();
            }

            var orden = await OrdenService.GetOrden(Orden.IdOrden);
            var existenContenedoresPendientes = orden.peticionesReferencias.FirstOrDefault().Contenedores.Any(c => c.IdEstadoContenedor == 7 || c.IdEstadoContenedor == 4);
            var contenedor = orden.peticionesReferencias.FirstOrDefault().Contenedores.Where(c => c.IdContenedor == Contenedor.IdContenedor).FirstOrDefault();
            var existenServiciosPendientes = contenedor.Servicios.Any(s => s.IdEstadoServicio == 7);

            if (!existenServiciosPendientes)
            {
                DialogService.Close(true);
            }

        }

    }
}
