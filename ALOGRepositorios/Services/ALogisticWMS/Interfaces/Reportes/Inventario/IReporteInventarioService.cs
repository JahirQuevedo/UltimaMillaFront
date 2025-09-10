using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IReporteInventarioService
{

    Task<ResultBase<DocumentoBase>> ObtieneReporteDescargaInventario(ConsultaMonitorInventario datosReporteador);

}
