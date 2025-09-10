using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UbicacionForm
    {
        #region
        [Parameter]
        public int IdCatCliente { get; set; }
        #endregion
        #region SERVICIOS
        [Inject] ICatClientesUbicacionesService CatClientesUbicacionesService { get; set; }
        [Inject] ICatPaisesService CatPaisesService { get; set; }
        [Inject] ICatPaisEstadosService CatPaisEstadosService { get; set; }
        [Inject] ICatPaisMunicipiosService catPaisMunicipiosService { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; } // Ensure NotificationService is injected
        [Inject] DialogService DialogService { get; set; }
        #endregion

        #region VARIABLES
        private CatClientesUbicaciones objClientesUbicaciones = new CatClientesUbicaciones();

        private List<CatPaises> lstPaises;
        private List<CatPaisEstados> lstPaisEstados;
        private List<CatPaisMunicipios> lstMunicipios;
        #endregion

        #region FUNCIONES

        protected override async Task OnInitializedAsync()
        {
            var tipoPaisesTask = CatPaisesService.CatPaisesListar();
            var tipoPaisEstadoTask = CatPaisEstadosService.CatPaisEstadosListar();
            var tipoMunicipiosTask = catPaisMunicipiosService.CatPaisMunicipiosListar();

            await Task.WhenAll(
                tipoPaisesTask,
                tipoPaisEstadoTask,
                tipoMunicipiosTask
                );

            lstPaises = await tipoPaisesTask;
            lstPaisEstados = await tipoPaisEstadoTask;
            lstMunicipios = await tipoMunicipiosTask;

        }

        private async Task AgregarUbicacion()
        {
            RespuestaGenericaDTO respuestaGenericaDto = new RespuestaGenericaDTO();
            try
            {
                objClientesUbicaciones.Activo = true;
                objClientesUbicaciones.FechaRegistro = DateTime.Now;
                objClientesUbicaciones.IdCatCliente = IdCatCliente;
                objClientesUbicaciones.IdCatUsuario = 1;

                respuestaGenericaDto =
                    await CatClientesUbicacionesService.CatClientesUbicacionesCrear(objClientesUbicaciones);
                if (respuestaGenericaDto.IsSuccess)
                {
                    objClientesUbicaciones = new CatClientesUbicaciones();
                    DialogService.Close(true);

                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Error al agregar la ubicación del cliente");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
    }
}
