using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOG.Modelos.Modelos.Vacios;
using AlogisticsWASM.Layout.Utilerias;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ALOGRespositorios.Modelos.Dtos.Control;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Pages.Vacios.ControlTower.OrdenesEnProceso
{
    public partial class CambiarEstadoServicioCmp
    {

        [Parameter] public EventCallback<bool> ServicioEstaTerminado { get; set; }
        [Parameter][EditorRequired] public PeticionesContenedores Contenedor { get; set; }
        [Parameter] public EventCallback<PeticionesServicios> EstadoCambiado { get; set; }
        [Parameter][EditorRequired] public PeticionesServicios Servicio { get; set; }
        [Parameter][EditorRequired] public UsuarioTokenDTO UsuarioTokenDTO { get; set; }

        [Inject] public NotificationService NotificationService { get; set; }
        [Inject] public IContenedorService ContenedorService { get; set; }

        private UtileriasPage _utileriasPage;
        private bool _estaEditando;
        private bool _cargando;
        private int _estadoTemporal;
        private List<CatReferenciaEstado> _estadosReferencias;
        private List<string> _errores;
        private bool _servicioEstaTerminado;

        protected override void OnInitialized()
        {
            _utileriasPage = new UtileriasPage();
            _errores = new List<string>();
            var e = _utileriasPage.ObtenerEstadosReferencia();

            _estadosReferencias = e.Where(es => es.IdCatReferenciaEstado == 4 || es.IdCatReferenciaEstado == 5).ToList();
        }

        private void IniciarEdicion()
        {

            if (Servicio.IdEstadoServicio is 5 or 6)
                return;

            if (UsuarioTokenDTO.Rol.Equals("ADMIN") || UsuarioTokenDTO.Rol.Equals("ADMINUSER"))
            {
                _estadoTemporal = Servicio.IdEstadoServicio;
                _estaEditando = true;
            }
        }

        private async Task OnCambioEstado(object valor)
        {
            if (valor is not int nuevoEstado)
                return;

            Servicio.IdEstadoServicio = nuevoEstado;
            _cargando = true;
            StateHasChanged();

            try
            {
                if (ServicioPuedeCambiarEstado())
                {
                    await CambiarEstadoServicioAsync(Servicio);
                    //await EstadoCambiado.InvokeAsync(Servicio);
                    await Task.Delay(1000);
                    MostrarNotificacion(
                        "Actualización de estado de servicio éxitosa",
                        $"Se cambio el estado del servicio {Servicio.DescServicio} de manera éxistosa",
                        NotificationSeverity.Success,
                        0
                    );
                    _servicioEstaTerminado = true;
                }
                else
                {
                    CancelarEdicion();
                    string mensaje = string.Join("<br/>", _errores);
                    MostrarNotificacion(
                        $"No se puede terminar el servicio {Servicio.DescServicio}",
                        mensaje,
                        NotificationSeverity.Warning,
                        5000
                    );
                    _errores.Clear();
                    _servicioEstaTerminado = false;
                }
            }
            finally
            {
                _cargando = false;
                _estaEditando = false;
                StateHasChanged();
            }
            await ServicioEstaTerminado.InvokeAsync(_servicioEstaTerminado);
        }

        private bool ServicioPuedeCambiarEstado()
        {

            int idCatServicio = Servicio.catServicios.IdCatServicio;
            ICollection<PeticionesDocumentos> documentos = Servicio.Documentos;

            _errores.Clear();

            switch (idCatServicio)
            {
                case 1:
                    if (!documentos.Any(documento => documento.IdTipoDocumento == 1))
                        _errores.Add("- Falta el documento correspondiente a MANIOBRA DE VACIO");
                    if (!documentos.Any(documento => documento.IdTipoDocumento == 2))
                        _errores.Add("- Falta el documento correspondiente a EIR DE VACIO");
                    break;
                case 2:
                    if (!documentos.Any(documento => documento.IdTipoDocumento == 2))
                        _errores.Add("- Falta el documento correspondiente a EIR DE VACIO");
                    break;
                case 4:
                    if (!documentos.Any(documento => documento.IdTipoDocumento == 3))
                        _errores.Add("- Falta el documento correspondiente a CARTA CORTE DE DEMORAS");
                    break;
            }

            return _errores.Count() == 0;

            //return idCatServicio switch {
            //    1 => documentos.Any(documento => documento.IdTipoDocumento == 1) && documentos.Any(documento => documento.IdTipoDocumento == 2),
            //    2 => documentos.Any(documento => documento.IdTipoDocumento == 2),
            //    4 => documentos.Any(documento => documento.IdTipoDocumento == 3),
            //    _ => true
            //};
        }

        private void CancelarEdicion()
        {
            Servicio.IdEstadoServicio = _estadoTemporal;
            _estaEditando = false;
        }

        private async Task CambiarEstadoServicioAsync(PeticionesServicios servicio)
        {
            SolCambioEstadoDTO estado = new SolCambioEstadoDTO();

            estado.IdReferencia = Contenedor.IdReferencia;
            estado.IdCatReferenciaEstado = servicio.IdEstadoServicio;
            estado.IdContenedor = Contenedor.IdContenedor;
            estado.IdServicio = servicio.IdServicio;
            await ContenedorService.ActualizaEstadoServicio(estado);
        }

        private void MostrarNotificacion(string titulo, string mensaje, NotificationSeverity notificationSeverity, int duration)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = notificationSeverity,
                Summary = titulo,
                Detail = mensaje,
                Duration = duration == 0 ? 4000 : duration
            });
        }
    }
}
