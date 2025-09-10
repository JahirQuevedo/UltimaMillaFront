using ALOG.Modelos.Modelos.Logisticos;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLOTransporteDetalleService
    {
        Task<List<SLOTransporteDetalle>> GetObtenerTransporteDetalle(int id);
    }
}
