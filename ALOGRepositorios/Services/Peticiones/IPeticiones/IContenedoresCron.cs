using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Peticiones;

namespace ALOGRepositorios.Services.Peticiones.IPeticiones
{
    public interface IContenedoresCron
    {
        Task<List<CatTipoIncidenciaEvento>> ObtenerRelacionIncidenciaEvento();
        Task<RespuestaGenericaDTO> RegistrarIncidenciaCron(PeticionesContenedoresCron peticionesContenedoresCron);
        Task<List<PeticionesContenedoresCron>> ObtenerCronologiaPorContenedor(int idContenedor, int idServicio);
        Task<RespuestaGenericaDTO> BajaCronologiaporContenedor(int pIdContenedorCron);
        Task<RespuestaGenericaDTO> ActualizarCronologiaporContenedor(PeticionesContenedoresCron peticionesContenedoresCron);
        //public Task<List<CatTipoEventosCron>> ListaEventos();
    }
}
