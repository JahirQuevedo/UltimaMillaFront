using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IManiobristaService
{

    Task<ResultBase<List<MonitorManiobrista>>> ObtenerPorRazonSocialContiene(string razonSocial);

}
