using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOG.Modelos.Modelos.DTO.Vacios;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;

namespace ALOGRepositorios.Services.Peticiones.IPeticiones
{
    public interface IOrdenService
    {

        public Task<Ordenes> CrearOrden(Ordenes orden);
        public Task<Ordenes> GetOrden(int idOrden);
        Task<SolTicketDTO> ValidarSolicitud(SolTicketDTO solTicket);
        Task<SolTicketDTO> GenerarSolicitud(SolTicketDTO solTicket);
        Task<RespuestaGenericaDTO> ObtenerPath(SolPathOrdenDTO pSolPathOrdenDTO);

        Task<PeticionesRespuestaDTO> CrearOrdenServicio(PeticionesReferenciasClienteExternoDTO orden);

        public Task<PeticionesRespuestaDTO> IniciarProceso(List<int> ordenes);

        public Task<PeticionesRespuestaDTO> AgregarContenedorAReferencia(int IdReferencia, PeticionesContenedoresClienteExternoDTO contenedor);
        public Task<PeticionesRespuestaDTO> AgregarServicioAContenedor(int IdReferencia, int IdContenedor, PeticionesServiciosClienteExternoDTO servicio);

        public Task<PeticionesRespuestaDTO> CancelarOrdenes(List<int> ordenes);
        public Task<PeticionesRespuestaDTO> CancelarElemento(string parametrosEncriptados);
        public Task<PeticionesRespuestaDTO> Integracion1G(int idOrden);
        public Task<PeticionesRespuestaDTO> GenerarAnticipo(int idOrden);
        public Task<PeticionesContenedores> ObtenerContenedor(int idContenedor);

    }
}
