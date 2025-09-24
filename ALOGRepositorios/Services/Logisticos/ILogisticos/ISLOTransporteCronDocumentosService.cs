using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLOTransporteCronDocumentosService
    {
        Task<RespuestaGenericaDTO> CrearSLOTransporteCronDocumeto(List<SLOTransporteCronDocumentos> lsttransporteCronDocumentos);
        Task<List<SLOTransporteCronDocumentos>> ObtenerSLOTranporteCronDocumentos(int IdTransporteCron);
    }
}
