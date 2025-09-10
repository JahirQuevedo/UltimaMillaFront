using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLOTControlTerrestreService
    {
        Task<RespuestaGenericaDTO> CrearTControlTerrestre(SLOTControlTerrestre sloTControlTerrestre);
        Task<RespuestaGenericaDTO> ObtenerPorIdTControlTerreste(int id);
        Task<RespuestaGenericaDTO> ActualizarTControlTerrestre(SLOTControlTerrestre sloTControlTerrestre);
        Task BajaTControlTerrestre(int id);
        Task AltaTControlTerrestre(int id);
        Task<RespuestaGenericaDTO> FinalizarOperacionControlTerrestre(SLOTControlTerrestre objSlOTControlTerrestre);
    }
}
