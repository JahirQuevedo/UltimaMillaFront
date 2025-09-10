using ALOGModelos.Modelos.catalogos;
using ALOGModelos.Modelos.Dtos;
using ALOGModelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace AlogisticsWASM.Layout.Logisticos
{
    public partial class FormCRUDAcarreo : ComponentBase
    {
        [Parameter][EditorRequired] public Acarreo objAcarreo { get; set; }
        [Parameter] public EventCallback<Acarreo> OnEncabezadoEditado { get; set; }
        [Parameter] public EventCallback<bool> OnEstaEditado { get; set; }
        [Parameter][EditorRequired] public bool EsEditable { get; set; } = false;
        [Inject] public ICatServicioService ServicioService { get; set; }
        [Inject] public IAcarreosService acarreosService { get; set; }
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
                objAcarreo = objAcarreo ?? new Acarreo();
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
            objAcarreo.IdCliente = (_clienteSeleccionado == null) ? objAcarreo.IdCliente : _clienteSeleccionado.IdCatCliente;
            objAcarreo.NombreCliente = (_clienteSeleccionado == null) ? objAcarreo.NombreCliente : _clienteSeleccionado.RazonSocial;
            objAcarreo.IdCatServicio = (_servicioSeleccionado == null) ? objAcarreo.IdCatServicio : _servicioSeleccionado.IdCatServicio;
            bool respuesta = await acarreosService!.Actualizar(objAcarreo!);

            if (respuesta)
            {
                await OnEncabezadoEditado.InvokeAsync(objAcarreo);
                EsEditable = false;
                await OnEstaEditado.InvokeAsync(EsEditable);

            }

            return respuesta;
        }

        private bool FormularioValido()
        {

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(objAcarreo);
            bool isValid = Validator.TryValidateObject(objAcarreo, validationContext, validationResults, true);

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
            _clienteSeleccionado = _clienteSeleccionado ?? _clientes!.FirstOrDefault(c => c.IdCatCliente == objAcarreo!.IdCliente);
        }

        private void ObtenerServicioSeleccionado(CatServicios servicio)
        {
            _servicioSeleccionado = _servicios!.FirstOrDefault(s => s.IdCatServicio == servicio.IdCatServicio)!;

            _servicioSeleccionado = _servicioSeleccionado ?? _servicios!.FirstOrDefault(s => s.IdCatServicio == objAcarreo!.IdCatServicio);
        }
    }



}

