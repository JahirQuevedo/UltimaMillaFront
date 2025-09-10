using ALOGModelos.Modelos.Logisticos;
using ALOGModelos.Modelos.Dtos;
using ALOGModelos.Modelos.FiltrosBusqueda;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaPage : ComponentBase
    {

        [Inject] private IAcarreosService _acarreoService { get; set; }
        [Inject] private IUltimaMillaEncabezadoService _encabezadoService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }

        private ICollection<UltimaMillaEncabezado> _encabezados;
        private ICollection<UltimaMillaEncabezado> _encabezadosCopia;

        private UltimaMillaEncabezadoEditarDTO? _encabezadoSeleccionado;

        private ICollection<Acarreo>? _acarreos;
        private int _ordenesCompletadas = 0;
        private int _ordenesCanceladas = 0;
        private int _ordenesEnProceso = 0;

        private List<UltimaMillaEncabezado> pagedData;
        private List<UltimaMillaEncabezado> pagedDataCopy;
        private int pageSize = 10;
        private int currentPage = 1;
        private int totalPages;
        private string _tipoEstado = string.Empty;
        private string ClienteFiltrar = string.Empty;
        private UltimaMillaEncabezadoFiltro? _filtro;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _filtro = new UltimaMillaEncabezadoFiltro();
                _filtro.Activo = true;
                await ObtenerDatos();
                Console.WriteLine($"_filtro {JsonConvert.SerializeObject(_filtro)}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            //_detalles = await _detalleService.GetDetalles();
        }
        private async Task ObtenerDatos()
        {
            _encabezados = await _encabezadoService!.GetEncabezados(_filtro);
            /*_acarreos = await _acarreoService!.ObtenerAcarreos();

            if (_acarreos == null)
            {
                _acarreos = new List<Acarreo>();
            }*/

            if (_encabezados == null)
            {
                _encabezados = new List<UltimaMillaEncabezado>();
            }
            else
            {
                _encabezadosCopia = _encabezados;
                totalPages = (int)Math.Ceiling((double)_encabezadosCopia!.Count / pageSize);
                UpdatePagedData();
            }

            _ordenesCompletadas = _encabezados.Count(e => e.TipoEstado.Tipo.Equals("T"));
            _ordenesCanceladas = _encabezados.Count(e => e.TipoEstado.Tipo.Equals("C"));
            _ordenesEnProceso = _encabezados.Count(e => e.TipoEstado.Tipo.Equals("P"));
        }
        private void UpdatePagedData()
        {

            //var registrosFiltrados =  

            //pagedData = _encabezados!
            //    .Skip((currentPage - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();
            pagedData = _encabezados!.ToList();
            pagedDataCopy = pagedData;
        }

        private async Task NextPage()
        {
            //if (CanGoNext) {
            currentPage++;
            _filtro.NumeroPagina = currentPage;
            //_encabezados = await _encabezadoService!.GetEncabezados(_filtro);
            await ObtenerDatos();
            UpdatePagedData();
            //}
        }

        private async Task PrevPage()
        {
            //if (CanGoPrev) {
            currentPage--;
            _filtro.NumeroPagina = currentPage;
            //_encabezados = await _encabezadoService!.GetEncabezados(_filtro);
            await ObtenerDatos();
            UpdatePagedData();
            //}
        }
        public Task BuscarPorCliente(string valor)
        {

            ClienteFiltrar = valor.ToUpper();

            if (string.IsNullOrEmpty(valor))
            {
                pagedData = pagedDataCopy;
            }
            else
            {
                pagedData = pagedData.Where(d => d.Cliente.RazonSocial.ToUpper().Contains(valor.ToUpper())).ToList();
            }

            //ClienteFiltrar = valor.ToUpper();

            //pagedData = _encabezados;

            return Task.CompletedTask;
        }

        public void SetTipoEstato(string tipoEstado)
        {
            _tipoEstado = tipoEstado;
        }

        private bool CanGoPrev => currentPage > 1;
        private bool CanGoNext => currentPage < totalPages;

        private void CrearOrden()
        {
            _navigationManager.NavigateTo("ordenes-crear");
        }

        private void EditarOrden(UltimaMillaEncabezado encabezado)
        {
            //_estaEditando = true;
            _encabezadoSeleccionado = new UltimaMillaEncabezadoEditarDTO();
            _encabezadoSeleccionado.Id = encabezado.Id;
            _encabezadoSeleccionado.FechaSolicitud = encabezado.FechaSolicitud;
            _encabezadoSeleccionado.Viaje = encabezado.Viaje;
            _encabezadoSeleccionado.IdCliente = encabezado.IdCliente;
            _encabezadoSeleccionado.NombreCliente = encabezado.NombreCliente;
            _encabezadoSeleccionado.FacturaCliente = encabezado.FacturaCliente;
            _encabezadoSeleccionado.Bodega = encabezado.Bodega;
            _encabezadoSeleccionado.IdCatEmpresa = encabezado.IdCatEmpresa;
            _encabezadoSeleccionado.FechaSalida = encabezado.FechaSalida;
            _encabezadoSeleccionado.IdTipoEstado = encabezado.IdTipoEstado;
            _encabezadoSeleccionado.IdOrden = encabezado.IdOrden;
            _encabezadoSeleccionado.IdServicio = encabezado.IdServicio;
            _encabezadoSeleccionado.Activo = encabezado.Activo;
            _encabezadoSeleccionado.FechaRegistro = encabezado.FechaRegistro;
        }

        private void ObtenerEncabezadoEditado(UltimaMillaEncabezadoEditarDTO encabezadoDto)
        {

            Console.WriteLine($"ObtenerEncabezadoEditado {JsonConvert.SerializeObject(encabezadoDto, Formatting.Indented)}");

            if (encabezadoDto == null)
            {
                return;
            }

            var encabezado = pagedData?.FirstOrDefault(e => e.Id == encabezadoDto.Id);

            if (encabezado != null)
            {
                encabezado.Id = encabezadoDto.Id;
                encabezado.FechaSolicitud = encabezadoDto.FechaSolicitud;
                encabezado.Viaje = encabezadoDto.Viaje;
                encabezado.IdCliente = encabezadoDto.IdCliente;
                encabezado.NombreCliente = encabezadoDto.NombreCliente;
                encabezado.FacturaCliente = encabezadoDto.FacturaCliente;
                encabezado.Bodega = encabezadoDto.Bodega;
                encabezado.IdCatEmpresa = encabezadoDto.IdCatEmpresa;
                encabezado.FechaSalida = encabezadoDto.FechaSalida;
                encabezado.IdTipoEstado = encabezadoDto.IdTipoEstado;
                encabezado.IdOrden = encabezadoDto.IdOrden;
                encabezado.IdServicio = encabezadoDto.IdServicio;
                encabezado.Activo = encabezadoDto.Activo;
                encabezado.FechaRegistro = encabezadoDto.FechaRegistro;
            }

        }

        private void VerDetalles(UltimaMillaEncabezado encabezado)
        {
            _navigationManager.NavigateTo($"UltimaMillaDetalle/{encabezado.Id}");
        }
    }
}
