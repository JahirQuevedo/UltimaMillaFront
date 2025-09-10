using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IReporteTarjaServicie
{

    Task<ResultBase<DocumentoBase>> ObtieneReporteTarjaPatioExterno(ConsultaTarjaReporteador datosReporteador);

}
