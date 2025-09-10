using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatNavieraService
    {

        Task<ICollection<CatNavieras>> GetNavieras();

        Task<ICollection<RespuestaGenericaCatalogosDTO>> GetNavierasConincidencia(string pCoincidencia);
        Task<List<CatNavieras>> FiltrarNavierasAsync(string razonSocial, string rfc);
    }
}
