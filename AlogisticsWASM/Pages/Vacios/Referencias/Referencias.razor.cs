using ALOGModelos.Modelos.Dtos.Respuestas;
using ALOGModelos.Modelos.FiltrosBusqueda;
using ALOGModelos.Modelos.Vacios.Peticiones;
using ALOGRepositorios.Services.Peticiones;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace AlogisticsWASM.Pages.Vacios.Referencias
{
    public partial class Referencias
    {
        private bool MostrarModal { get; set; } = false;
        private ICollection<RespObtenerReferenciasDTO> referencias { get; set; }
        private string TipoOperacion { get; set; } = string.Empty!;
        private PeticionesReferencias ReferenciaSeleccionada { get; set; } = null!;
        [Inject] private IReferenciaService referenciaService { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] private IOrdenService ordenService { get; set; } = null!;
        private FiltroReferencia FiltroReferencia { get; set; } = new FiltroReferencia();
        private ICollection<RespObtenerReferenciasDTO> ReferenciasFiltradas { get; set; } = new List<RespObtenerReferenciasDTO>();
        private string TicketFiltrar { get; set; } = string.Empty;
        private bool _estaEditando = false;

        protected override async Task OnInitializedAsync()
        {
            ReferenciaSeleccionada = new PeticionesReferencias();
            await ObtenerReferencias();
        }

        private async Task ObtenerReferencias()
        {
            //FiltroReferencia.IdAduana = 1;
            //FiltroReferencia.IdEmpresa = 1;
            FiltroReferencia.IdLNegocio = 1;
            referencias = await referenciaService.GetReferencias(FiltroReferencia);

            if ((referencias != null))
            {
                ReferenciasFiltradas = referencias;
            }
        }

        private void CrearReferencia()
        {
            ObtenerReferenciaSeleccionada(new RespObtenerReferenciasDTO());
            _estaEditando = false;
            MostrarModal = true;
        }

        private void VerDetalles(RespObtenerReferenciasDTO referencia)
        {
            ObtenerReferenciaSeleccionada(referencia);
            navigationManager.NavigateTo($"/contenedores/{referencia.IdReferencia}");
        }

        private void EditarReferencia(RespObtenerReferenciasDTO referencia)
        {
            ObtenerReferenciaSeleccionada(referencia);
            _estaEditando = true;
            MostrarModal = true;
        }

        private async void ObtenerReferenciaSeleccionada(RespObtenerReferenciasDTO referencia)
        {
            var obj =
                (referencia == null)
                ? new RespObtenerReferenciasDTO()
                : referencias.FirstOrDefault(r => r.Ticket == referencia.Ticket)!;
            //ConvertirModal en RespObtenerReferenciasDTO
            ReferenciaSeleccionada = new PeticionesReferencias();
        }

        private async Task BajaReferencia(RespObtenerReferenciasDTO referencia)
        {
            var jsonObject = JsonConvert.SerializeObject(referencia);
            Console.WriteLine(jsonObject);
            await referenciaService.BajaReferencia(referencia);
        }

        private async Task ObtenerReferenciaCreada(PeticionesReferencias referencia)
        {
            //if (referencia == null)
            //{
            //    return;
            //}

            //if (referencia.IdReferencia <= 0)
            //{
            //    return;
            //}

            //var orden = await ordenService.GetOrden(referencia.IdOrden);
            //referencia.Orden = orden;

            //referencias.Add(referencia);
            //InvokeAsync(StateHasChanged);
        }

        private void FiltrosAplicados(Dictionary<string, object> filtros)
        {

            if (filtros != null || filtros.Count() > 0)
            {

                int ticket = int.Parse(filtros["Ticket"].ToString()!);
                string transportistaRazonSocial = filtros["TransporteRazonSocial"].ToString()!;

                bool filtroEstaVacio = (ticket == 0 && string.IsNullOrWhiteSpace(transportistaRazonSocial));

                if (filtroEstaVacio)
                {
                    ReferenciasFiltradas = referencias;
                    return;
                }

                ReferenciasFiltradas = referencias.Where(r =>
                        (ticket == 0 || r.Ticket == ticket) &&
                         (string.IsNullOrWhiteSpace(transportistaRazonSocial) || r.Transporte_RazonSocial.Contains(transportistaRazonSocial, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
            }

        }

        private Task BuscarReferenciasPorTicket(string ticket)
        {
            TicketFiltrar = ticket;
            ReferenciasFiltradas = referencias.Where(r => r.Ticket.ToString().Contains(TicketFiltrar)).ToList();
            return Task.CompletedTask;
        }

    }
}
