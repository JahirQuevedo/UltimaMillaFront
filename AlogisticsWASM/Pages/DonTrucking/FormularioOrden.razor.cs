
using ALOGModelos.Modelos.Logisticos;
using ALOGModelos.Modelos.Graficas;
using ALOGRepositorios.Services.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using ALOGModelos.Modelos.Catalogos;
using ALOGModelos.Modelos.Dtos;
using ALOGModelos.Modelos.DTLogistico;


namespace AlogisticsWASM.Pages.DonTrucking
{
    public partial class FormularioOrden : ComponentBase
    {

        IJSObjectReference modulo;

        private DtUltimaMillaEnc Encabezado { get; set; } = new DtUltimaMillaEnc();
        private DtUltimaMillaDet Detalle { get; set; } = new DtUltimaMillaDet();
        private List<DtUltimaMillaDet> _detalles;
        private ICollection<CatClientes> _clientes;
        private ICollection<CatServicios> _servicios;
        private CatClientes _cliente;
        private string _facturaCliente = string.Empty;
        private string _bodega = string.Empty;
        private string _numeroParte = string.Empty;

        private EditContext _editContextEncabezado;
        private EditContext _editContextDetalle;
        private int pasoActual { get; set; } = 1;
        [Inject] IJSRuntime JS { get; set; }
        [Inject] ICatClientesService clienteService { get; set; }
        [Inject] ICatServicioService servicioService { get; set; }
        [Inject] IUltimaMillaEncabezadoService encabezadoService { get; set; }
        [Inject] public NavigationManager NavigationManaget { get; set; }
        [Inject] public IOrdenService OrdenService { get; set; }

        private List<string> _errores = new List<string>();

        protected override async Task OnInitializedAsync()
        {

            _editContextEncabezado = new EditContext(Encabezado);
            _editContextDetalle = new EditContext(Detalle);

            Encabezado.catServicios = new CatServicios();

            _detalles = new List<DtUltimaMillaDet>();
            _servicios = new List<CatServicios>();
            _clientes = new List<CatClientes>();


            Encabezado.IdTipoEstado = 1;
            Encabezado.FechaSolicitud = DateTime.Now;
            Detalle.Pallet = 0;
            Detalle.Piezas = 0;

            _servicios = await servicioService.GetServicios();
            _clientes = await clienteService.GetClientes();



        }

        private async Task AgregarDetalle(DtUltimaMillaDet detalle)
        {

            Console.WriteLine($"detalle {JsonConvert.SerializeObject(detalle, Formatting.Indented)} ");

            bool response = DatosValidoEncabezado();

            Console.WriteLine($"response {response}");

            if (response)
            {
                DtUltimaMillaDet _nuevoDetalle = new DtUltimaMillaDet()
                {
                    IdDtUltimaMillaDet = 0,
                    NumeroParte = detalle.NumeroParte,
                    Pallet = detalle.Pallet,
                    Piezas = detalle.Piezas
                };
                _detalles.Add(_nuevoDetalle);
            }


        }

        private async Task CrearOrden()
        {
            Encabezado.IdCatEmpresa = 1;
            Encabezado.IdOrden = 1;
            Encabezado.IdTipoEstado = 1;
            Encabezado.DtUltimaMillaDets = _detalles;

            Orden orden = new Orden();

            orden.IdCatCliente = Encabezado.catClientes.IdCatCliente;
            orden.IdCatSistema = 1;
            orden.IdCatAduana = 1;
            orden.IdCatEmpresa = 1;
            orden.IdCatSucursal = 1;
            orden.IdCatLineaNegocio = 1;
            orden.IdUsuario = 1;
            orden.FechaRegistro = DateTime.Now;

            //var ordenResponse = await OrdenService!.CrearOrden(orden);
            Encabezado.IdOrden = 1;

            //sdasd _editContextEncabezado.Validate()

            if (!_editContextEncabezado.Validate() || !_editContextDetalle.Validate())
            {
                await JS.InvokeVoidAsync("bootstrapInterop.showModal", "myModal");
            }
            bool respuesta = EncabezadoValido(Encabezado);
            Console.WriteLine($"Respuesta crear {respuesta}");

            if (respuesta)
            {
                var encabezado = await encabezadoService!.CrearEncabezado(Encabezado);
                Console.WriteLine($"encabezado creado {JsonConvert.SerializeObject(encabezado)}");
                if (encabezado != null)
                {
                    if (encabezado.IdDtUltMillaEnc > 0)
                    {
                        NavigationManaget!.NavigateTo("/ordenes");
                    }
                }
                else
                {
                    //await JS.InvokeVoidAsync("bootstrapInterop.showModal", "myModal");
                }
                Console.WriteLine($"CrearOrden {JsonConvert.SerializeObject(Encabezado)}");
            }


        }

        private bool EncabezadoValido(DtUltimaMillaEnc encabezado)
        {

            if (encabezado.Viaje <= 0)
            {
                _errores.Add("El número del viaje es inválido");
            }

            if (encabezado.IdCliente <= 0)
            {
                _errores.Add("No seleccionó un cliente válido");
            }

            if (string.IsNullOrEmpty(encabezado.FacturaCliente))
            {
                _errores.Add("No ingresó una factura válida");
            }

            if (string.IsNullOrEmpty(encabezado.Bodega))
            {
                _errores.Add("No ingresó una bodega válida");
            }

            if (encabezado.IdCatServicio <= 0)
            {
                _errores.Add("No seleccionó un servicio válido");
            }

            if (encabezado.DtUltimaMillaDets == null || encabezado.DtUltimaMillaDets.Count() <= 0)
            {
                _errores.Add("La orden no tiene detalles");
            }

            foreach (var detalle in encabezado.DtUltimaMillaDets)
            {

                if (string.IsNullOrEmpty(detalle.NumeroParte))
                {
                    _errores.Add("El número de parte no es válido");
                    break;
                }

                if (detalle.Piezas <= 0)
                {
                    _errores.Add("El número de piezas no es válido");
                    break;
                }

                if (detalle.Pallet <= 0)
                {
                    _errores.Add("El número de pallets no es válido");
                    break;
                }
            }

            return _errores.Count() == 0;
        }

        public void ObtenerClienteSeleccionado(CatClientes cliente)
        {
            Encabezado.IdCliente = cliente.IdCatCliente;
            Encabezado.catClientes.IdCatCliente = cliente.IdCatCliente;
            Encabezado.catClientes.RazonSocial = cliente.RazonSocial;
        }

        private void ObtenerServicioSeleccionado(CatServicios servicio)
        {
            Encabezado.IdCatServicio = servicio.IdCatServicio;
            Encabezado.catServicios = servicio;
        }

        private void SiguientePaso()
        {

            if (DatosValidoEncabezado())
            {
                if (pasoActual < 3)
                {
                    pasoActual++;
                }
            }


        }

        public void SetPasoActual(int _pasoActual)
        {
            pasoActual = _pasoActual;
        }

        private Task SetFacturaCliente(string facturaCliente)
        {
            _facturaCliente = facturaCliente.ToUpper();
            Encabezado.FacturaCliente = _facturaCliente;
            return Task.CompletedTask;
        }

        private Task SetBodega(string bodega)
        {
            _bodega = bodega.ToUpper();
            Encabezado.Bodega = _bodega;
            return Task.CompletedTask;
        }

        private Task SetNumeroParte(string numeroParte)
        {
            _numeroParte = numeroParte.ToUpper();
            Detalle.NumeroParte = _numeroParte;
            return Task.CompletedTask;
        }

        private bool DatosValidoEncabezado()
        {

            if (pasoActual == 1)
            {
                Console.WriteLine($"_editContextEncabezado is nul {_editContextEncabezado is null}");
                Console.WriteLine($"_editContextEncabezado.Validate() {_editContextEncabezado.Validate()}");

                if (_editContextEncabezado != null && _editContextEncabezado.Validate())
                {
                    return true;
                }
                else
                {
                    //await JS.InvokeVoidAsync("bootstrapInterop.showModal", "myModal");
                }
            }
            else if (pasoActual == 2)
            {
                Console.WriteLine($"_editContextDetalle is nul {_editContextDetalle is null}");
                Console.WriteLine($"_editContextDetalle.Validate() {_editContextDetalle.Validate()}");

                if (_editContextDetalle != null && _editContextDetalle.Validate())
                {
                    return true;
                }
                else
                {
                    //await JS.InvokeVoidAsync("bootstrapInterop.showModal", "myModal");
                }
            }

            return true;

        }

        private bool DatosValidoDetalle()
        {
            if (_editContextDetalle != null && _editContextDetalle.Validate())
            {
                return true;
            }
            return false;
        }

    }
}
