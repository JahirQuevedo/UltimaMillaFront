using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatClientesUbicacionesService
    {
        public Task<List<CatClientesUbicaciones>> CatClientesUbicacionesListar();
        public Task<RespuestaGenericaDTO> CatClientesUbicacionesCrear(CatClientesUbicaciones ubicacionCliente);
    }
}
