using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Logistica;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Integracion1G;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Orden;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLOReferenciasService
    {
        Task<IEnumerable<Ordenes>> ObtenerReferencias(FiltroOrdenesReferenciasDTO filtro);
        Task<List<SLOIntegraFacturaEnc>> ObtenerFacturas(int idOrden);
        Task<Ordenes> ObtenerServicios(int idOrden);
        Task<RespuestaGenericaDTO> CrearReferenciaALO(SLOReferenciasDTO SLOrefe);
        Task<RespuestaGenericaDTO> EditarReferenciaCliente(Ordenes orden);
        Task<RespuestaGenericaDTO> GuardarServicios(List<SLOPeticionesContenedores> datos);
        Task<List<CatServicios>> ObtenerServiciosPorCoincidenciaAsync(string coincidencia);
        Task<RespuestaGenericaDTO> ActualizarServicioAsync(SLOPeticionesServicios servicio);
        Task<RespuestaGenericaDTO> EliminarServicioAsync(int idServicio);
        Task<RespuestaGenericaDTO> EnviarAFacturarAsync(Ordenes factura);

    }
}
