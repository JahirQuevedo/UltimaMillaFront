
using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatAduanaService
    {
        Task<ICollection<CatAduana>> GetAduanas();
        Task<ICollection<RespListarCoincidenciasDTO>> GetAduanasConincidencia(string pCoincidencia);
    }
}
