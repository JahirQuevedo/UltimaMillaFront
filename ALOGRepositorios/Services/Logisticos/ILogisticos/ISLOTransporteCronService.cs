using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLOTransporteCronService
    {
        Task<RespuestaGenericaDTO> SLOTransporteCronCrear(SLOTransportesCron trasnporteCron);
        Task<List<SLOTransportesCron>> SLOTransporteCronListar(int idTransAsignado);
    }
}
