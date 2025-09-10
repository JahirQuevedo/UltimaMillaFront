using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Pages.Vacios.Solicitudes
{
    public partial class SolicitudesCRUDCMP
    {
        #region Parametros
        //[Parameter] public UsuarioTokenDTO paramUsuarioTokenDTO { get; set; }
        #endregion Parametros

        [Inject] private DialogService iDialogService { get; set; }
        [Inject] private ICatTipoContenedorService iCatTipoContenedorService { get; set; }

        // Modelo del ticket
        SolTicketDTO ticket = new SolTicketDTO();
        private List<CatTipoContenedor> lstCatTipoContenedors;

        // Bandera para diferenciar entre modo creación y actualización
        bool isUpdateMode = false;

        // Las siguientes listas simulan datos para los combos/autocompletar.
        // En producción, deberás invocar a tus servicios (API Rest) para obtenerlos.


        protected override void OnInitialized()
        {
            #region ValidarAcceso
            ////Console.WriteLine("ValidarAcceso:" + _UsuarioTokenDTO.catUsuarios.Nombre);
            #endregion ValidarAcceso
            //lstCatTipoContenedors = iCatTipoContenedorService.GetTiposContenedor().ToList();
        }

        // Clase auxiliar para representar los elementos de los combos
        public class Item
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        // Evento que se ejecuta al enviar el formulario (crear o actualizar)
        async Task OnValidSubmit()
        {
            iDialogService.Close(ticket);

            // Invocar endpoint de validación de ticket (ejemplo)
            // bool valido = await ValidarTicket(ticket);
            // if (!valido)
            // {
            //     // Aquí podrías utilizar NotificationService o SweetAlert para notificar errores.
            //     return;
            // }

            // // Dependiendo del modo, invocar el endpoint para crear o actualizar
            // if (isUpdateMode)
            // {
            //     await ActualizarTicket(ticket);
            // }
            // else
            // {
            //     await CrearTicket(ticket);
            // }
        }

        // Método simulado para validar ticket (reemplaza con tu lógica/API Rest)
        async Task<bool> ValidarTicket(SolTicketDTO ticket)
        {
            // Ejemplo: await OrdenService.ValidarTicket(ticket);
            await Task.Delay(500);
            return true;
        }

        // Método simulado para crear ticket (reemplaza con tu lógica/API Rest)
        async Task CrearTicket(SolTicketDTO ticket)
        {
            // Ejemplo: await OrdenService.GenerarSolicitud(ticket);
            await Task.Delay(500);
            // Notificar y limpiar formulario o redirigir según sea necesario
        }

        // Método simulado para actualizar ticket (reemplaza con tu lógica/API Rest)
        async Task ActualizarTicket(SolTicketDTO ticket)
        {
            // Solo se permite actualizar si el estado es 7
            // if (ticket.IdCatReferenciaEstado != 7)
            // {
            //     // Mostrar mensaje de error (por ejemplo, usando NotificationService)
            //     return;
            // }
            // Lógica de actualización
            await Task.Delay(500);
            // Notificar éxito y redirigir o limpiar formulario
        }

        void OnCancel()
        {
            DialogService.Close(null);

            // Lógica para cancelar y/o limpiar el formulario
        }
    }
}
