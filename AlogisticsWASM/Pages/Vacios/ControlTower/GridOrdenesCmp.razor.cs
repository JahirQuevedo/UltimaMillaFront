using ALOG.Modelos.Modelos.Orden;
using ALOGRepositorios.Services.Control.IControl;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.ControlTower
{
    public partial class GridOrdenesCmp
    {

        [Parameter] public EventCallback<IList<Ordenes>> OnOrdenesSeleccionadasChanged { get; set; }
        [Parameter] public EventCallback<int> OnIdOrdenCancelar { get; set; }
        [Parameter] public List<Ordenes> Ordenes { get; set; }
        [Parameter] public RenderFragment<Ordenes> ChildContent { get; set; }
        [Parameter] public bool EstaCargando { get; set; } = true;
        [Parameter][EditorRequired] public bool MostrarChecks { get; set; }
        [Parameter][EditorRequired] public bool MostrarBotonesAccion { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }
        [Inject] public IControlService ControlService { get; set; }
        [Inject] public IOrdenService OrdenService { get; set; }


        private EventCallback<IList<Ordenes>> _onValueChanged;
        private List<Ordenes> _ordenes;
        private IList<Ordenes> _ordenesSeleccionadas;

        private RadzenDataGrid<Ordenes> _gridOrdenes;

        private bool _allowRowSelectOnRowClick = true;
        private Ordenes _ordenExpandida;

        private IEnumerable<Ordenes> _ieOrdenes;
        private bool _primeraFilaExpandida = false;

        protected override async Task OnInitializedAsync()
        {
            _ordenesSeleccionadas = new List<Ordenes>();
            //_ordenes = Ordenes;
            _ordenes = new List<Ordenes>();
            _onValueChanged = EventCallback.Factory.Create<IList<Ordenes>>(this, OnValueChanged);
            _ordenes = Ordenes ?? new List<Ordenes>();


            if (_gridOrdenes != null)
            {
                await _gridOrdenes.Reload();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            _ordenes = Ordenes ?? new List<Ordenes>();
            _ieOrdenes = _ordenes;
            _ordenes = Ordenes ?? new List<Ordenes>();
            _ieOrdenes = Ordenes;
            if (_gridOrdenes != null)
            {
                await _gridOrdenes.Reload();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_primeraFilaExpandida)
            {
                await Task.Delay(300);
                var primerOrdenExpandible = _ordenes
                    ?.FirstOrDefault(o => o.peticionesReferencias?.Any(r => r.Contenedores?.Any() == true) == true);
                if (primerOrdenExpandible != null && _gridOrdenes != null)
                {
                    // Forzamos render completo del gridSolicitudesDetalle
                    await InvokeAsync(StateHasChanged);
                    await Task.Delay(100);
                    await _gridOrdenes.ExpandRow(primerOrdenExpandible);
                    await _gridOrdenes.Reload();
                    _primeraFilaExpandida = true;

                }
                return;
            }

            if (firstRender == false && !_primeraFilaExpandida)
            {
                await Task.Delay(300);
                var primerOrdenExpandible = _ordenes
                    ?.FirstOrDefault(o => o.peticionesReferencias?.Any(r => r.Contenedores?.Any() == true) == true);
                if (primerOrdenExpandible != null && _gridOrdenes != null)
                {
                    // Forzamos render completo del gridSolicitudesDetalle
                    await InvokeAsync(StateHasChanged);
                    await Task.Delay(100);
                    await _gridOrdenes.ExpandRow(primerOrdenExpandible);
                    await _gridOrdenes.Reload();
                    _primeraFilaExpandida = true;
                }
            }

        }

        public async Task RecargarTabla()
        {
            await _gridOrdenes.Reload();
            await OnSelectAllChanged(false);
        }

        private void OnRowRender(RowRenderEventArgs<Ordenes> args)
        {
            args.Expandable = args.Data?.peticionesReferencias?.Any(r => r.Contenedores?.Any() == true) == true;
        }

        private void OnRowExpand(Ordenes orden)
        {
            var r = orden.peticionesReferencias.Where(r => r.IdOrden == orden.IdOrden);
            var c = r.Select(r => r.Contenedores);
        }

        private async Task CancelarOrden(Ordenes orden)
        {

            var result = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "¡Advertencia!",
                Text = $"¿Está seguro de cancelar la orden # {orden.IdOrden}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí",
                CancelButtonText = "No"
            });

            if (string.IsNullOrEmpty(result.Value))
            {
                return;
            }

            var respuestaCancelarOrden = await OrdenService.CancelarOrdenes(new List<int> { orden.IdOrden });

            if (_ordenes.Contains(orden))
            {
                _ordenes.Remove(orden);
                await _gridOrdenes.Reload();
            }

            //string comentario = await MostrarModalComentarios();

            //if (string.IsNullOrEmpty(comentario)) {
            //    return;
            //}

            //Reset(contenedor);

            //if (_contenedores.Contains(contenedor)) {
            //    _contenedores.Remove(contenedor);
            //    await _gridContenedores.Reload();
            //}
        }

        #region Métodos para controlar el cambio de los checks y notificarlo al componente padre

        private async Task OnValueChanged(IList<Ordenes> seleccionados)
        {
            _ordenesSeleccionadas = seleccionados;
            await OnOrdenesSeleccionadasChanged.InvokeAsync(_ordenesSeleccionadas);
        }

        private async Task OnSelectAllChanged(bool? isChecked)
        {
            _ordenesSeleccionadas = isChecked == true ? _ordenes.ToList() : new List<Ordenes>();
            //Console.WriteLine($"OnSelectAllChanged => _ordenesSeleccionadas.Count() {_ordenesSeleccionadas.Count()}");
            await OnOrdenesSeleccionadasChanged.InvokeAsync(_ordenesSeleccionadas);
        }

        #endregion Métodos para controlar el cambio de los checks y notificarlo al componente padre
    }
}
