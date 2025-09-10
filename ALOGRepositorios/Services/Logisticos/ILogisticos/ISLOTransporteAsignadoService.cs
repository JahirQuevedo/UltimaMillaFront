using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLOTransporteAsignadoService
    {
        Task<RespuestaGenericaDTO> SLOTransporteAsignadoCrear(SLOTransporteAsignado objSLOTransporteAsignado);
    }
}
