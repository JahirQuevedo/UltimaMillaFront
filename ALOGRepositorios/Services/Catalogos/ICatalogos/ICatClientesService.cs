using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatClientesService
    {

        public Task<ICollection<CatClientes>> GetClientes();
        public Task<ICollection<RespListarCoincidenciasDTO>> GetClientesConincidencia(string pCoincidencia);
    }
}
