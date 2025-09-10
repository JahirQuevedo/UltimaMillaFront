using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface ILineaOperadorService
{

    Task<ResultBase<List<MonitorLineaOperador>>> ObtenerPorNombreContiene(int idCatTransportista, string nombre);

}
