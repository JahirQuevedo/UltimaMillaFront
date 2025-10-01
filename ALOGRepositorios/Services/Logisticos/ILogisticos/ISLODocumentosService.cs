using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Vacios;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLODocumentosService
    {
        Task<RespuestaGenericaDTO> SLOUploadFile(SLOCargarArchivo sloCargarArchivoDTO);
        Task<List<SLOSolicitudesDocumentos>> sloGetFilesTask(FiltroGenericoDTO filtroGenericoDTO);
        //Task AbrirDocumentoCont(SLOSolicitudesDocumentos doc);
        Task<RespuestaGenericaDTO> BajaDocumento(SLOSolicitudesDocumentos archivoBaja);
        Task<List<SLOSolicitudesDocumentos>> SLOListarArchivosSolicitud(int IdSolicitud);
    }
}
