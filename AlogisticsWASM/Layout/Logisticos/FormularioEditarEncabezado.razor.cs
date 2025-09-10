using ALOGModelos.Modelos.catalogos;
using ALOGModelos.Modelos.Dtos;
using ALOGModelos.Modelos.Vacios.Peticiones;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AlogisticsWASM.Layout.Logisticos
{
    public partial class FormularioEditarEncabezado : ComponentBase
    {

        [Parameter][EditorRequired] public UltimaMillaEncabezadoEditarDTO Encabezado { get; set; }
        [Parameter] public EventCallback<UltimaMillaEncabezadoEditarDTO> OnEncabezadoEditado { get; set; }
        [Parameter] public EventCallback<bool> OnEstaEditado { get; set; }
        [Parameter][EditorRequired] public bool EsEditable { get; set; } = false;
        [Inject] public ICatServicioService ServicioService { get; set; }
        [Inject] public IUltimaMillaEncabezadoService encabezadoService { get; set; }
        [Inject] public ICatClientesService ClienteService { get; set; }

        private ICollection<CatServicios> _servicios;
        private ICollection<CatClientes> _clientes;
        private CatServicios _servicioSeleccionado;
        private CatClientes _clienteSeleccionado;

        //private EditContext? _editContext;

        protected override async Task OnInitializedAsync()
        {

            try
            {
                Encabezado = Encabezado ?? new UltimaMillaEncabezadoEditarDTO();
                //_editContext = new EditContext(Encabezado);
                _clientes = await ClienteService!.GetClientes();
                _servicios = await ServicioService!.GetServicios();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnParametersSet()
        {
        }

        private async Task<bool> Actualizar()
        {
            FormularioValido();
            Encabezado.IdCliente = (_clienteSeleccionado == null) ? Encabezado.IdCliente : _clienteSeleccionado.IdCatCliente;
            Encabezado.NombreCliente = (_clienteSeleccionado == null) ? Encabezado.NombreCliente : _clienteSeleccionado.RazonSocial;
            Encabezado.IdServicio = (_servicioSeleccionado == null) ? Encabezado.IdServicio : _servicioSeleccionado.IdCatServicio;
            bool respuesta = await encabezadoService!.Actualizar(Encabezado!);

            if (respuesta)
            {
                await OnEncabezadoEditado.InvokeAsync(Encabezado);
                EsEditable = false;
                await OnEstaEditado.InvokeAsync(EsEditable);

            }

            return respuesta;
        }

        private bool FormularioValido()
        {

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(Encabezado);
            bool isValid = Validator.TryValidateObject(Encabezado, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (var error in validationResults)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }

            return true;
        }

        private void ObtenerClienteSeleccionado(CatClientes cliente)
        {
            _clienteSeleccionado = _clientes!.FirstOrDefault(c => c.IdCatCliente == cliente.IdCatCliente!);
            _clienteSeleccionado = _clienteSeleccionado ?? _clientes!.FirstOrDefault(c => c.IdCatCliente == Encabezado!.IdCliente);
        }

        private void ObtenerServicioSeleccionado(CatServicios servicio)
        {
            _servicioSeleccionado = _servicios!.FirstOrDefault(s => s.IdCatServicio == servicio.IdCatServicio)!;

            _servicioSeleccionado = _servicioSeleccionado ?? _servicios!.FirstOrDefault(s => s.IdCatServicio == Encabezado!.IdServicio);
        }
    }
}
