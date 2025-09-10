using ALOG.Modelos.Modelos.DTLogistico;


namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface IUltimaMillaDetalleService
    {

        public Task<ICollection<DtUltimaMillaDet>> GetDetalles();
        public Task<ICollection<DtUltimaMillaDet>> GetDetalles(int idEncabezado);
        public Task<bool> Actualizar(DtUltimaMillaDet detalle);
    }
}
