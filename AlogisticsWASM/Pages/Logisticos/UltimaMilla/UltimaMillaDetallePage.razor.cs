
using ALOGModelos.Modelos.Logisticos;
using ALOGModelos.Modelos.Dtos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using ALOGModelos.Modelos.Catalogos;
using ALOGModelos.Modelos.DTLogistico;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaDetallePage : ComponentBase
    {
        [Parameter] public int IdEncabezado { get; set; } = 0;
        [Inject] public IUltimaMillaDetalleService detalleService { get; set; }
        [Inject] public IUltimaMillaEncabezadoService encabezadoService { get; set; }
        [Inject] public ICatTipoEstadoService tipoEstadoService { get; set; }
        [Inject] public IJSRuntime Js { get; set; }
        private ICollection<DtUltimaMillaDet> _detalles;
        private List<DtUltimaMillaDet>? pagedData;
        private ICollection<CatTipoEstado> _tiposEstado;
        private int pageSize = 10;
        private int currentPage = 1;
        private int totalPages;
        private DtUltimaMillaEnc _encabezado;
        private DtUltimaMillaEnc _encabezadoCopia;
        private int _idTipoEstadoSeleccionado;
        private string _disabled;
        IJSObjectReference modulo;
        private bool mostrarToast = false;
        private List<string> _errores = new List<string>();
        private UltimaMillaEncabezadoEditarDTO? _encabezadoSeleccionado;
        private bool _esEditable = false;
        private int? filaEnEdicionId = null;

        protected override async Task OnInitializedAsync()
        {
            //modulo = await Js.InvokeAsync<IJSObjectReference>("import", "./assets/js/utilerias.js");


            _tiposEstado = await tipoEstadoService.GetTiposEstado();
            _detalles = new List<DtUltimaMillaDet>();
            _encabezado = await encabezadoService.GetEncabezado(IdEncabezado);
            _encabezadoCopia = _encabezado;
            _idTipoEstadoSeleccionado = _encabezado.IdTipoEstado;
            _detalles = await detalleService.GetDetalles(IdEncabezado);

            _disabled = _encabezado.IdTipoEstado == 3 || _encabezado.IdTipoEstado == 4 ? "true" : "false";
            SetEncabezadoDto();
            //if ( _detalles.Count() > 0 ) { 
            //    _detalles = _detalles.Where(d => d.IdUltimaMillaEncabezado == IdEncabezado).ToList();
            //}

            totalPages = (int)Math.Ceiling((double)_detalles!.Count / pageSize);
            UpdatePagedData();
        }

        private async Task ActualizarTipoEstatdo(int idTipoEstado)
        {
            int idTipoEstadoActual = _idTipoEstadoSeleccionado;
            bool respuesta = await encabezadoService.CambiarTipoEstado(IdEncabezado, idTipoEstado);

            if (respuesta)
            {
                _encabezado.IdTipoEstado = idTipoEstado;
                _idTipoEstadoSeleccionado = idTipoEstado;
                mostrarToast = true;
                //await modulo.ToastrSuccess("Post borrado correctamente");
            }
            else
            {
                _encabezado.IdTipoEstado = idTipoEstadoActual;
                var tipoEstado = _tiposEstado.FirstOrDefault(t => t.Id == idTipoEstadoActual);
                _errores.Add($"El estado actual de la orden es {tipoEstado.Nombre}, no es permitido efectuar cambios de estatus");
                await Js.InvokeVoidAsync("bootstrapInterop.showModal", "myModal");
            }
        }

        private void SetEncabezadoDto()
        {
            _encabezadoSeleccionado = new UltimaMillaEncabezadoEditarDTO();
            _encabezadoSeleccionado.Id = _encabezado.IdDtUltMillaEnc;
            _encabezadoSeleccionado.FechaSolicitud = _encabezado.FechaSolicitud;
            _encabezadoSeleccionado.Viaje = _encabezado.Viaje;
            _encabezadoSeleccionado.IdCliente = (int)_encabezado.IdCliente;
            _encabezadoSeleccionado.NombreCliente = _encabezado.catClientes.RazonSocial;
            _encabezadoSeleccionado.FacturaCliente = _encabezado.FacturaCliente;
            _encabezadoSeleccionado.Bodega = _encabezado.Bodega;
            _encabezadoSeleccionado.IdCatEmpresa = _encabezado.IdCatEmpresa;
            _encabezadoSeleccionado.FechaSalida = _encabezado.FechaSalida;
            _encabezadoSeleccionado.IdTipoEstado = _encabezado.IdTipoEstado;
            _encabezadoSeleccionado.IdOrden = _encabezado.IdOrden;
            _encabezadoSeleccionado.IdServicio = _encabezado.IdCatServicio;
            _encabezadoSeleccionado.Activo = _encabezado.Activo;
            _encabezadoSeleccionado.FechaRegistro = _encabezado.FechaRegistro;
        }

        private void ObtenerEncabezadoEditado(UltimaMillaEncabezadoEditarDTO encabezadoDTO)
        {
            Console.WriteLine($"Recibo el encabezado editado {JsonConvert.SerializeObject(encabezadoDTO)}");
        }

        public void ObtenerEstaEditando(bool estaEditando)
        {
            _esEditable = estaEditando;
        }

        private void SetEstaEditando(int id)
        {
            filaEnEdicionId = id;
        }

        private void SetEsEditableEncabezado()
        {
            _esEditable = !_esEditable;
        }

        private void EditarEncabezado()
        {

        }

        private void CancelarAccion()
        {
            _esEditable = false;

        }

        private async Task GuardarCambios(DtUltimaMillaDet detalle)
        {
            Console.WriteLine($"detalle recibido {JsonConvert.SerializeObject(detalle)}");
            await detalleService.Actualizar(detalle);
            // Aquí puedes guardar los cambios en la base de datos o la lista
            filaEnEdicionId = null; // Salir del modo edición
        }

        private void CancelarEdicion()
        {
            filaEnEdicionId = null; // Salir del modo edición sin guardar
        }

        private void UpdatePagedData()
        {

            var registrosFiltrados =

            pagedData = _detalles!
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        private void NextPage()
        {
            if (CanGoNext)
            {
                currentPage++;
                UpdatePagedData();
            }
        }

        private void PrevPage()
        {
            if (CanGoPrev)
            {
                currentPage--;
                UpdatePagedData();
            }
        }

        private bool CanGoPrev => currentPage > 1;
        private bool CanGoNext => currentPage < totalPages;
    }
}
