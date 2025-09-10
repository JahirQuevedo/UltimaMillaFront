
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Orden;

namespace ALOGRepositorios.Services.Integracion1G.IIntegracion1G
{
    public interface IIntegracion1GService
    {

        public Task<RespuestaGenericaDTO> GenerarSolicitudFacturacion1G(Ordenes orden);

    }
}
