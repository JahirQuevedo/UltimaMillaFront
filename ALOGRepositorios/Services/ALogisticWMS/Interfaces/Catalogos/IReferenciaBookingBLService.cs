using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IReferenciaBookingBLService
{

    Task<ResultBase<MonitorReferenciaBookingBL>> Guardar(MonitorReferenciaBookingBL referenciaBookingBL);

    Task<ResultBase> Eliminar(int idReferenciaBookingBl);

    Task<ResultBase<MonitorReferenciaBookingBL>> ObtenerPorId(int idReferenciaBookingBl);

}
