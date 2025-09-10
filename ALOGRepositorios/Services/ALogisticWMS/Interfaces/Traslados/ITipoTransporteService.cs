using ALOG.Modelos;


namespace ALOGRepositorios.Services;

public interface ITipoTransporteService
{

    Task<ResultBase<List<TipoTransporte>>> ObtenerPorNombreContiene(string nombre);

}
