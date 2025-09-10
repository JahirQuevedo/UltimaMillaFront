using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;


namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLOSolicitudesService
    {
        public Task<ICollection<SLOSolicitudes>> GetSolicitudes(FiltroSLOSolicitudes filtro);
        public Task<ICollection<SLOSolicitudes>> GetSolicitudesListar();
        public Task<RespuestaGenericaDTO> CrearSolicitudSLO(SLOSolicitudes solicitud);
        public Task<RespuestaGenericaDTO> ActualizarSolicitudSLO(SLOSolicitudes solicitud);
        public Task<RespuestaGenericaDTO> BajaSolicitudSLO(int id);
        public Task<SLOSolicitudes> ObtenerSolicitudId(int id);
    }
}
