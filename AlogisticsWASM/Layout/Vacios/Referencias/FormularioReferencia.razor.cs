using ALOG.Modelos.Modelos.Catalogos;

using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;

using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace AlogisticsWASM.Layout.Vacios.Referencias
{
    public partial class FormularioReferencia
    {

        [Parameter][EditorRequired] public PeticionesReferencias Referencia { get; set; } = new PeticionesReferencias();
        [Inject] private ICatTransportistaService transportistaService { get; set; } = null!;
        [Inject] private IReferenciaService referenciaService { get; set; } = null!;
        [Inject] private IOrdenService ordenService { get; set; } = null!;
        [Parameter] public EventCallback<PeticionesReferencias> OnReferenciaCreada { get; set; }

        private string ReferenciaAlo { get; set; } = string.Empty;
        private Ordenes Orden { get; set; } = new Ordenes();
        private CatClientes Cliente { get; set; } = new CatClientes();
        public string TextBoton { get; set; } = string.Empty!;
        public string TituloFormulario { get; set; } = string.Empty!;
        private IEnumerable<CatTransportistas> transportistas = new List<CatTransportistas>();
        private CatTransportistas Transportista { get; set; } = new CatTransportistas();

        protected override void OnInitialized()
        {
            ReferenciaAlo = Referencia.IdOrden == null ? "" : Referencia.ordenes.ReferenciaALO!;
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Referencia == null)
            {
                Referencia = new PeticionesReferencias();
                await InvokeAsync(StateHasChanged);
            }

            ReferenciaAlo = Referencia.ordenes == null ? "" : Referencia.ordenes.ReferenciaALO!;

            transportistas = await transportistaService.GetTransportistas();
            var json = JsonConvert.SerializeObject(transportistas, Formatting.Indented);

            TextBoton = Referencia.Ticket == 0 ? "Crear referencia" : "Editar referencia";
            TituloFormulario = Referencia.Ticket == 0 ? "Creando referencia" : "Editando referencia";
        }

        public async Task GuardarDatos()
        {

            if (Cliente.IdCatCliente <= 0)
            {
                return;
            }

            Orden.IdCatSistema = 1;
            Orden.IdCatCliente = Cliente.IdCatCliente;
            Orden.IdCatAduana = 1;
            Orden.IdCatEmpresa = 1;
            Orden.IdCatSucursal = 1;
            Orden.IdCatLineaNegocio = 1;
            Orden.IdUsuario = 1;
            Orden.ReferenciaALO = "V-VE-24/555";

            var orden = await ordenService.CrearOrden(Orden);

            if (orden.IdOrden <= 0)
            {
                return;
            }

            Referencia.IdOrden = orden.IdOrden;
            Referencia.Transporte_RFC = Transportista.RFC;
            Referencia.Transporte_RazonSocial = Transportista.RazonSocial;
            Referencia.Transporte_Usuario = "Usuario";
            Referencia.Transporte_UsuarioEmail = Transportista.Correo;
            Referencia.TipoReferencia = 1;
            Referencia.Comentarios = Referencia.Comentarios.ToUpper();
            var referencia = referenciaService.CrearReferencia(Referencia);

            await OnReferenciaCreada.InvokeAsync(await referencia);
        }

        private void ObtenerTransportista(string rfc)
        {
            Transportista = transportistas.FirstOrDefault(t => t.RFC == rfc)!;
        }

        private void ObtenerClienteSeleccionado(CatClientes cliente)
        {
            Cliente = cliente;
        }
    }
}
