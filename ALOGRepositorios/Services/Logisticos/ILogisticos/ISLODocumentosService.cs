using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Vacios;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLODocumentosService
    {
        Task<bool> SLOUploadFile(SLOCargarArchivo sloCargarArchivoDTO);
        Task<List<SLOSolicitudesDocumentos>> sloGetFilesTask(int idSLOTransporteSolicitud);
        //Task AbrirDocumentoCont(SLOSolicitudesDocumentos doc);
        Task<RespuestaGenericaDTO> BajaDocumento(SLOSolicitudesDocumentos archivoBaja);
    }
}
