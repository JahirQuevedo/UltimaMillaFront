using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;
using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Vacios;

namespace AlogisticsWASM.Layout.Servicios
{
    public partial class AsignarServicios
    {

        private IEnumerable<CatServicios> servicios = new List<CatServicios>();
        private IEnumerable<CatServicios> serviciosws = new List<CatServicios>();

        private IEnumerable<CatServicios> ServciosFiltrados = new List<CatServicios>();
        private List<CatServicios> ServiciosSeleccionados { get; set; }
        [Parameter] public EventCallback<List<PeticionesServicios>> OnServiciosSeleccionados { get; set; }
        [Inject] ICatServicioService servicioService { get; set; } = null!;

        public string NombreServicioFiltrar { get; set; } = string.Empty;

        //protected override async Task OnInitializedAsync()
        //{
        //    serviciosws = await servicioService.GetServicios();
        //    servicios = serviciosws.Select(servicio => new CatServicio
        //    {
        //        IdCatServicio = servicio.IdCatServicio,
        //        Nombre = servicio.Nombre,
        //        Descripcion = servicio.Descripcion,
        //        FechaRegistro = servicio.FechaRegistro,
        //        Activo = servicio.Activo
        //    }).ToList();

        //    if (servicios.Count() > 0)
        //    {
        //        foreach (var servicio in servicios)
        //        {
        //            servicio.IsSelected = false;
        //        }
        //        ServciosFiltrados = servicios;
        //    }

        //}

        public void ObtenerServiciosSeleccionados()
        {
            ServiciosSeleccionados = ServciosFiltrados.Where(servicio => servicio.IsSelected).ToList();
        }

        public async Task EnviarServiciosSeleccionados()
        {

            List<PeticionesServicios> peticionesServicios = new List<PeticionesServicios>();
            ServiciosSeleccionados = ServciosFiltrados.Where(servicio => servicio.IsSelected).ToList();

            foreach (var servicio in ServiciosSeleccionados)
            {
                peticionesServicios.Add(new PeticionesServicios
                {
                    IdServicio = servicio.IdCatServicio,
                    IdTipoServicio = servicio.IdCatServicio,
                    DescServicio = servicio.Descripcion,
                    FechaRegistro = servicio.FechaRegistro,
                    Activo = servicio.Activo
                });
            }

            await OnServiciosSeleccionados.InvokeAsync(peticionesServicios);
        }

        private Task BuscarPorNombreServicio(string nombreServicio)
        {

            NombreServicioFiltrar = nombreServicio.ToUpper();

            ServciosFiltrados = servicios.Where(s => s.Nombre.ToUpper().Contains(NombreServicioFiltrar)).ToList();

            return Task.CompletedTask;
        }
    }
}
