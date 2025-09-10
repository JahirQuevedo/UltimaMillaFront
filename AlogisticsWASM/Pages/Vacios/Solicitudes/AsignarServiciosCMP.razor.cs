using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Pages.Vacios.Solicitudes
{
    public partial class AsignarServiciosCMP
    {

        private IList<CatServicios> _serviciosSeleccionados;
        private List<CatServicios> _serviciosFiltrados;
        private List<CatServicios> _selectedServicios;
        private List<CatServicios> _servicios;

        private bool _allowRowSelectOnRowClick;

        private RadzenDataGrid<CatServicios> _grid;
        private string _servicioNombre;

        [Inject] private ICatServicioService CatServicioService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        protected override async Task OnInitializedAsync()
        {

            _selectedServicios = new List<CatServicios>();
            _servicios = new List<CatServicios>();
            _serviciosFiltrados = new List<CatServicios>();

            await CargarServiciosAsync();
        }

        private async Task CargarServiciosAsync()
        {

            ICollection<CatServicios> catServicios = await CatServicioService.GetServicios();
            foreach (var servicioDto in catServicios)
            {
                _servicios.Add(new CatServicios
                {
                    IdCatServicio = servicioDto.IdCatServicio,
                    Nombre = servicioDto.Nombre,
                    Descripcion = servicioDto.Descripcion,
                    Activo = servicioDto.Activo,
                    FechaRegistro = servicioDto.FechaRegistro,
                    IdCatEmpresas = servicioDto.IdCatEmpresas,
                    IdUsuarioRegistro = servicioDto.IdUsuarioRegistro
                });
            }
            _serviciosFiltrados = new List<CatServicios>(_servicios);
            await _grid.RefreshDataAsync();
            _grid.ResetLoadData();
        }

        private void OnCheckBoxChange(bool isChecked, CatServicios servicio)
        {

            if (servicio is null)
            {
                return;
            }

            if (isChecked)
            {
                if (!_selectedServicios.Contains(servicio))
                {
                    _selectedServicios.Add(servicio);
                }
            }
            else
            {
                if (_selectedServicios.Contains(servicio))
                {
                    _selectedServicios.Remove(servicio);
                }
            }
        }

        private void AplicarServicios()
        {


        }

        private void Cancelar()
        {
            DialogService.Close(null);
        }
        private async Task FiltrarServiciosAsync(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                _serviciosFiltrados = new List<CatServicios>(_servicios); // Restaurar la lista completa si el filtro está vacío
            }
            else
            {
                _serviciosFiltrados = _servicios
                    .Where(s => s.Nombre.Contains(valor, StringComparison.OrdinalIgnoreCase)) // Filtrar por coincidencias
                    .ToList();

            }
            await _grid.RefreshDataAsync();
            _grid.ResetLoadData();
        }

    }
}
