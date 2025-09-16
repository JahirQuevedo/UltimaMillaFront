using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLOTransporteSolicitudService
    {
        Task<RespuestaGenericaDTO> SLOTransporteSolicitudListar();
        Task<RespuestaGenericaDTO> SLOTransporteSolicitudCrear(SLOTransporteSolicitud transporteSolicitud);
        Task<RespuestaGenericaDTO> SLOTransporteSolicitudFinalizar(int id);
    }
}
