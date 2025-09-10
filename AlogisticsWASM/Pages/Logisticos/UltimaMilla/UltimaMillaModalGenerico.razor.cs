using ALOG.Modelos.Modelos.Logisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using Microsoft.AspNetCore.Components;
using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Pages.Logisticos.UltimaMilla
{
    public partial class UltimaMillaModalGenerico
    {
        #region CLASES PARAMETRO
        [Parameter] public string ModoModal { get; set; }
        [Inject] IServiceProvider ServiceProvider { get; set; }
        // Parámetros y clases necesarias para las diferentes funcionalidades del modal
        //Traking
            [Parameter] public SLOTControlTerrestre TControlTerrestre { get; set; }
            [Parameter] public UsuarioTokenDTO UsuarioDTO { get; set; }
            private ISLOTControlTerrestreService _servicioControlTerrestreService;  

        #endregion

        #region INICIALIZACIÓN
        protected override async Task OnInitializedAsync()
        {
            // Código de inicialización
            if (ModoModal == "ADDTRACKING")
            {
                #region SERVICIOS ADDTRACKING
                var servicioControlTerrestreService = ServiceProvider.GetRequiredService<ISLOTControlTerrestreService>();
                var catTipoEventosContronService = ServiceProvider.GetRequiredService<ICatTipoEventosCronService>();
                var dialogService = ServiceProvider.GetRequiredService<DialogService>();
                var TransporteCronService = ServiceProvider.GetRequiredService<ISLOTransporteCronService>();
                var sweetAlertService = ServiceProvider.GetRequiredService<SweetAlertService>();
                var sloDocumentosService = ServiceProvider.GetRequiredService<ISLODocumentosService>();

                
                #endregion

            #region OBJETOS Y LISTAS ADDTRACKING
                SLOTControlTerrestre objTControlTerrestre = new();
                SLOTransportesCron transporteCron = new SLOTransportesCron();
                ICollection<CatTipoEventosCron> listaTiposEventos;
                
                objTControlTerrestre = TControlTerrestre;
                
                #endregion

            }
        }
        #endregion

        #region AGREGAR TRACKING
        // Código para agregar tracking
        //public async Task InicializarTraking()
        //{
        //    #region OBJETOS Y LISTAS ADDTRACKING
        //        SLOTControlTerrestre objTControlTerrestre = new();
        //        SLOTransportesCron transporteCron = new SLOTransportesCron();
        //        ICollection<CatTipoEventosCron> listaTiposEventos;

        //        objTControlTerrestre = TControlTerrestre;
        //        listaTiposEventos = await CatTipoEventosCron.CatTipoEventosCronListar();

        //    #endregion


        //}

        #endregion

        #region SUBIR DOCUMENTOS


        #endregion

        #region AGREGAR UBICACIÓN CLIENTE


        #endregion

    }
}
