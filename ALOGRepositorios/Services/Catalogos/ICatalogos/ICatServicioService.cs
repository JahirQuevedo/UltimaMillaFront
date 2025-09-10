using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;


namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatServicioService
    {

        Task<ICollection<CatServicios>> GetServicios();

        Task<ICollection<CatServicios>> ObtenerServiciosConincidencia(string pCoincidencia);

        Task<RespuestaGenericaDTO> ObternerPorId(int pIdCatServicio);

        public Task<List<CatLineaNegocioTariPrecio>> ObtenerTarifarioServicios(string parametrosEncriptados);
    }
}
