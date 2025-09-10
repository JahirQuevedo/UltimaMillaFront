using ALOG.Modelos;
using ALOG.Modelos.ModelosWMS.Reportes.Salidas;

namespace ALOGRepositorios.Services;

public interface IReporteLiberacionService
{
    Task<ResultBase<DocumentoBase>> ObtieneReporteTarjaSalidaLiberacion(ConsultaLiberacionReporteador datosReporteador);

}
