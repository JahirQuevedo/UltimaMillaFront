using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.DTO.Consultas;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface IUltimaMillaEncabezadoService
    {

        public Task<ICollection<DtUltimaMillaEnc>> GetEncabezados(FiltroDtUltimaMillaDTO filtro);
        public Task<DtUltimaMillaEnc> GetEncabezado(int idEncabezado);
        public Task<DtUltimaMillaEnc> CrearEncabezado(DtUltimaMillaEnc encabezado);
        public Task<bool> CambiarTipoEstado(int idEncabezado, int idTipoEstado);
        public Task<bool> Actualizar(DtUltimaMillaEnc encabezado);

    }
}
