using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.FiltrosBusqueda;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using ALOG.Modelos.Modelos.Dtos.Respuestas;

namespace AlogisticsWASM.Pages.Logisticos.Acarreos
{
    public partial class AcarreosPage : ComponentBase
    {
        #region Injections
        [Inject] private IAcarreosService _acarreosService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }

        #endregion Injections

        #region Varibales
        private FiltroDtAcarreosDTO _filtro;
        private ICollection<RespObtenerAcarreosDTO> _lstAcarreos;
        private ICollection<RespObtenerAcarreosDTO> _lstAcarreosWork;
        private ICollection<Acarreo> _objAcarreos;
        private ICollection<Acarreo> _objAcarreosWork;
        private List<RespObtenerAcarreosDTO> _lstAcarreosPage;
        private List<RespObtenerAcarreosDTO> _lstAcarreosPageWork;
        private int _ordenesCompletadas;
        private int _ordenesCanceladas;
        private int _ordenesEnProceso;
        private int _totalPaginas;
        private int _cantidadRegistros;
        private int currentPage = 1;
        private string strClienteFiltrar = string.Empty;
        private string _strTipoEstado = string.Empty;
        private Acarreo _objAcarreoSeleccionado = new Acarreo();

        #endregion Variables

        protected override async Task OnInitializedAsync()
        {
            #region Variables

            _ordenesCanceladas = 0;
            _ordenesCompletadas = 0;
            _ordenesEnProceso = 0;
            #endregion Variables
            try
            {

                _filtro = new FiltroDtAcarreosDTO();
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
            _lstAcarreos = await _acarreosService!.ObtenerAcarreos(_filtro);
            Console.WriteLine("Cuantos registros: " + _lstAcarreos.Count());


            if (_lstAcarreos == null)
            {
                _lstAcarreos = new List<RespObtenerAcarreosDTO>();
            }
            else
            {
                _lstAcarreosWork = _lstAcarreos;
                _totalPaginas = (int)Math.Ceiling((double)_lstAcarreosWork!.Count / _cantidadRegistros);
                UpdatePagedData();
            }


            _ordenesCompletadas = _lstAcarreos.Count(e => e.IdCatTipoEstado == 3);
            _ordenesCanceladas = _lstAcarreos.Count(e => e.IdCatTipoEstado == 4);
            _ordenesEnProceso = _lstAcarreos.Count(e => e.IdCatTipoEstado == 2);
        }
        private void UpdatePagedData()
        {

            //var registrosFiltrados =  

            //pagedData = _encabezados!
            //    .Skip((currentPage - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();
            _lstAcarreosPage = _lstAcarreos!.ToList();
            _lstAcarreosPageWork = _lstAcarreosPage;
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


            strClienteFiltrar = valor.ToUpper();

            if (string.IsNullOrEmpty(valor))
            {
                _lstAcarreosPage = _lstAcarreosPageWork;
            }
            else
            {
                _lstAcarreosPage = _lstAcarreosPage.Where(d => d.NombreCliente.ToUpper().Contains(valor.ToUpper())).ToList();
            }

            //ClienteFiltrar = valor.ToUpper();

            //pagedData = _encabezados;

            return Task.CompletedTask;
        }

        public void SetTipoEstato(string tipoEstado)
        {
            _strTipoEstado = tipoEstado;
        }

        private bool CanGoPrev => currentPage > 1;
        private bool CanGoNext => currentPage < _totalPaginas;

        private void CrearOrden()
        {
            _navigationManager.NavigateTo("ordenes-crear");
        }

        private void EditarOrden(RespObtenerAcarreosDTO pAcarreo)
        {
            //_estaEditando = true;
            _objAcarreoSeleccionado = new Acarreo();
            _objAcarreoSeleccionado.Id = pAcarreo.IdDtAcarreos;
            _objAcarreoSeleccionado.Fecha = pAcarreo.Fecha;
            _objAcarreoSeleccionado.Mes = pAcarreo.Mes;
            _objAcarreoSeleccionado.NombreServicio = pAcarreo.NombreServicio;
            _objAcarreoSeleccionado.NumeroContenedor = pAcarreo.NumeroContenedor;
            _objAcarreoSeleccionado.IdCliente = pAcarreo.IdCliente;
            _objAcarreoSeleccionado.IdCatTipoEstado = pAcarreo.IdCatTipoEstado;
            _objAcarreoSeleccionado.IdEmpresa = pAcarreo.IdEmpresa;
            _objAcarreoSeleccionado.NombreCliente = pAcarreo.NombreCliente;
            _objAcarreoSeleccionado.IdOrden = pAcarreo.IdOrden;
            _objAcarreoSeleccionado.Activo = pAcarreo.Activo;
            _objAcarreoSeleccionado.FechaRegistro = pAcarreo.FechaRegistro;
        }

        private void ObtenerEncabezadoEditado(Acarreo pAcarreosDTO)
        {

            Console.WriteLine($"ObtenerEncabezadoEditado {JsonConvert.SerializeObject(pAcarreosDTO, Formatting.Indented)}");

            if (pAcarreosDTO == null)
            {
                return;
            }

            var objlstAcarreos = _lstAcarreosPage?.FirstOrDefault(e => e.IdDtAcarreos == pAcarreosDTO.Id);

            if (pAcarreosDTO != null)
            {
                objlstAcarreos.IdDtAcarreos = pAcarreosDTO.Id;
                objlstAcarreos.Fecha = pAcarreosDTO.Fecha;
                objlstAcarreos.Mes = pAcarreosDTO.Mes;
                objlstAcarreos.NombreServicio = pAcarreosDTO.NombreServicio;
                objlstAcarreos.NumeroContenedor = pAcarreosDTO.NombreCliente;
                objlstAcarreos.IdCliente = pAcarreosDTO.IdCliente;
                objlstAcarreos.IdCatTipoEstado = pAcarreosDTO.IdCatTipoEstado;
                objlstAcarreos.NombreCliente = pAcarreosDTO.NombreCliente;
                objlstAcarreos.IdEmpresa = pAcarreosDTO.IdEmpresa;
                objlstAcarreos.IdOrden = pAcarreosDTO.IdOrden;
                objlstAcarreos.Activo = pAcarreosDTO.Activo;

            }

        }

        private void VerDetalles(RespObtenerAcarreosDTO encabezado)
        {
            _navigationManager.NavigateTo($"AcarreosEditar/{encabezado.IdDtAcarreos}/E");
        }
    }
}
