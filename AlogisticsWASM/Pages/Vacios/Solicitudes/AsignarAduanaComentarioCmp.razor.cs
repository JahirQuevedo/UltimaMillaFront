using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Vacios;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Pages.Vacios.Solicitudes
{
    public partial class AsignarAduanaComentarioCmp
    {

        [Parameter] public PeticionesReferenciasClienteExternoDTO PeticionReferencia { get; set; }

        #region Servicios
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private ICatAduanaService AduanaService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        #endregion Servicios

        private ICollection<CatAduana> _aduanas;
        private Dictionary<int, string> _datos;
        private int _idAduanaSeleccionada;
        private string _comentarios;
        private bool _estaCargando;

        protected override async Task OnInitializedAsync()
        {
            _idAduanaSeleccionada = 0;
            _comentarios = "";
            _aduanas = await AduanaService.GetAduanas();
            await InitAduanasAsync();
        }

        protected override void OnParametersSet()
        {
            if (PeticionReferencia != null)
            {
                _idAduanaSeleccionada = PeticionReferencia.IdCatAduana;
                _comentarios = PeticionReferencia.Comentarios;
            }
        }

        private async Task InitAduanasAsync()
        {
            try
            {
                _estaCargando = true;
                _aduanas = await AduanaService.GetAduanas();
                _estaCargando = false;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error al cargar aduanas",
                    Detail = ex.Message
                });
            }
            finally
            {
                _estaCargando = false;
            }
        }

        private void Aceptar()
        {

            if (_idAduanaSeleccionada == 0)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Falta indicar la aduana",
                    Detail = "Debes seleccionar una aduana para realizar la solicitud"
                });
                return;
            }

            PeticionReferencia.IdCatAduana = _idAduanaSeleccionada;
            PeticionReferencia.Comentarios = _comentarios;

            DialogService.Close(PeticionReferencia);
        }

        private void Cancelar()
        {
            DialogService.Close(null);
        }
        private void OnChange(string value, string name)
        {

        }
    }
}
