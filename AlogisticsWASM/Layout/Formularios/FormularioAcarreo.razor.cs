
using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTLogistico;

using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AlogisticsWASM.Layout.Formularios
{
    public partial class FormularioAcarreo
    {

        [Parameter][EditorRequired] public DtAcarreos? Acarreo { get; set; }
        private EditContext _editContext;
        private ICollection<CatServicios> _servicios;
        private ICollection<CatClientes> _clientes;

        [Inject] private ICatServicioService _servicioService { get; set; }
        [Inject] private ICatClientesService _clienteService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Acarreo = new DtAcarreos();
            _editContext = new EditContext(Acarreo);
            _servicios = new List<CatServicios>();
            _servicios = await _servicioService!.GetServicios();
            _clientes = await _clienteService!.GetClientes();
        }

        private bool DatosValidos()
        {
            return true;
        }

        private void ObtenerClienteSeleccionado(CatClientes cliente)
        {

        }

        private void ObtenerServicioSeleccionada(CatServicios servicio)
        {

        }
    }
}
