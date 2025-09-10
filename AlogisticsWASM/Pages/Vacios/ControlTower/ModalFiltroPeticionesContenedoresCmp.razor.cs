using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Pages.Vacios.ControlTower
{
    public partial class ModalFiltroPeticionesContenedoresCmp
    {

        [Inject] private ICatClientesService ClienteService { get; set; }
        [Inject] private ICatNavieraService NavieraService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        //FiltroOrdenesReferenciasDTO
        private ICollection<CatClientes> _clientes;
        private ICollection<CatNavieras> _navieras;
        private int _idCatClienteSeleccionado;
        private int _idCatNavieraSeleccionado;
        private string _referenciaCliente;

        protected override async Task OnInitializedAsync()
        {

            _clientes = await ClienteService.GetClientes();
            _navieras = await NavieraService.GetNavieras();

        }

        private void Aceptar()
        {
            DialogService.Close();
        }

        private void Cancelar()
        {
            DialogService.Close();
        }
    }
}
