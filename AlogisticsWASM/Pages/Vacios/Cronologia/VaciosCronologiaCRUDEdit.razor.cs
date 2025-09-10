using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Peticiones;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Pages.Vacios.Cronologia
{
    public partial class VaciosCronologiaCRUDEdit
    {
        [Parameter] public PeticionesContenedoresCron IncidenciaAEditar { get; set; }

        [Parameter] public List<CatTipoIncidenciaEvento> RelacionIncidenciaEvento { get; set; }


        [Inject] IContenedoresCron icontenedoresCron { get; set; }
        [Inject] public SweetAlertService sweetalertService { get; set; }
        [Inject] public DialogService dialogService { get; set; }

        private PeticionesContenedoresCron objPeticionesContenedoresCron = new();
        private List<CatTipoIncidenciaCron> listaTiposIncidencia = new();
        private List<CatTipoEventosCron> listaTiposEvento = new();
        private string tipoIncidencia;
        private string tipoEvento;
        private int idTipoIncidencia;
        private int idTipoEvento;
        private string comentario;
        private DateTime fechaEvento;

        protected override void OnInitialized()
        {
            //tipoIncidencia = IncidenciaAEditar.TipoIncidencia;
            //tipoEvento = IncidenciaAEditar.TipoEvento;
            //comentario = IncidenciaAEditar.Comentario;
            //fechaEvento = IncidenciaAEditar.FechaEvento;


        }

        protected override void OnParametersSet()
        {
            if (IncidenciaAEditar != null)
            {
                objPeticionesContenedoresCron = IncidenciaAEditar;
                idTipoIncidencia = IncidenciaAEditar.catTipoIncidenciaEvento.catTipoIncidenciaCron.IdCatTipoIncidenciaCron;
                idTipoEvento = IncidenciaAEditar.catTipoIncidenciaEvento.catTipoEventosCron.IdCatTipoEventoCron;

                listaTiposIncidencia = RelacionIncidenciaEvento
                    .Select(x => x.catTipoIncidenciaCron)
                    .DistinctBy(x => x.IdCatTipoIncidenciaCron)
                    .ToList();

                listaTiposEvento = RelacionIncidenciaEvento
                    .Where(x => x.catTipoIncidenciaCron.IdCatTipoIncidenciaCron == idTipoIncidencia)
                    .Select(x => x.catTipoEventosCron)
                    .ToList();
            }
        }
        private void OnTipoIncidenciaChange(object value)
        {
            idTipoIncidencia = (int)value;
            listaTiposEvento = RelacionIncidenciaEvento
                .Where(x => x.catTipoIncidenciaCron.IdCatTipoIncidenciaCron == idTipoIncidencia)
                .Select(x => x.catTipoEventosCron)
                .ToList();

            idTipoEvento = 0; // Resetea evento al cambiar incidencia
        }

        private async Task Actualizar(PeticionesContenedoresCron formData)
        {
            var nuevaRelacion = RelacionIncidenciaEvento.FirstOrDefault(x =>
                x.catTipoIncidenciaCron.IdCatTipoIncidenciaCron == idTipoIncidencia &&
                x.catTipoEventosCron.IdCatTipoEventoCron == idTipoEvento);

            if (nuevaRelacion == null)
            {
                await sweetalertService.FireAsync("Error", "No se encontró una relación válida entre tipo de incidencia y tipo de evento.", SweetAlertIcon.Error);
                return;
            }

            //formData.IdCatTipoIncidenciaEvento = nuevaRelacion.IdCatTipoIncidenciaEvento;
            var dataEnviar = new PeticionesContenedoresCron
            {
                IdContenedorCron = formData.IdContenedorCron,
                IdCatTipoIncidenciaEvento = nuevaRelacion.IdCatTipoIncidenciaEvento,
                Comentarios = formData.Comentarios,
                FechaEvento = formData.FechaEvento
            };

            var respuesta = await icontenedoresCron.ActualizarCronologiaporContenedor(dataEnviar);

            if (respuesta.IsSuccess)
            {
                await sweetalertService.FireAsync("¡Éxito!", respuesta.strMensaje, SweetAlertIcon.Success);
                dialogService.Close(true);
            }
            else
            {
                await sweetalertService.FireAsync("Error", respuesta.strMensaje, SweetAlertIcon.Error);
            }
        }

    }
}
