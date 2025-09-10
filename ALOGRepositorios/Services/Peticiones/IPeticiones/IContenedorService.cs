using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOG.Modelos.Modelos.Vacios;

namespace ALOGRepositorios.Services.Peticiones.IPeticiones
{
    public interface IContenedorService
    {

        Task<PeticionesContenedores> CrearContenedor(PeticionesContenedores contenedor, int idReferencia);
        Task<List<PeticionesReferencias>> GetContenedores(int idReferencia);
        Task<bool> AsignarServicios(int idReferencia, int idContenedor, PeticionesServicios servicio);
        Task<bool> RemoverServicio(int idReferencia, PeticionesServicios servicio);

        Task<bool> ActualizaFolioContenedor(SolActualizarFolioManiobraDTO pSolActualizarFolioManiobraDTO);
        Task<RespuestaGenericaDTO> ActualizaEstadoContenedor(SolCambioEstadoDTO pSolCambioEstadoDTO);

        Task<List<string>> ActualizaEstadoServicio(SolCambioEstadoDTO pSolCambioEstadoDTO);
        Task<List<string>> ActualizaEstadoTodosContenedores(SolCambioEstadoDTO pSolCambioEstadoDTO);
        Task<PeticionesContenedores> ObtenerContenedor(PeticionesContenedores pContenedor);
        public Task<RespuestaGenericaDTO> AsignarPatio(PeticionesContenedores contenedor);
    }
}
