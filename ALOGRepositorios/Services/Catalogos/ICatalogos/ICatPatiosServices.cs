using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatPatiosServices
    {
        public Task<ICollection<CatPatios>> GetPatios();
        public Task<ICollection<RespuestaGenericaCatalogosDTO>> GetPatiosConincidencia(string pCoincidencia, int? pIdAduana);

        public Task<ICollection<CatPatios>> ObtenerListaPatiosFiltrados(string parametroEncriptado);
    }
}
