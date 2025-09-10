using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Vacios;

namespace ALOGRepositorios.Services.Peticiones.IPeticiones
{
    public interface IReferenciaService
    {

        Task<ICollection<RespObtenerReferenciasDTO>> GetReferencias(FiltroOrdenesReferenciasDTO filtro);
        Task<PeticionesReferencias> CrearReferencia(PeticionesReferencias referencia);
        Task<bool> BajaReferencia(RespObtenerReferenciasDTO referencia);

        Task<PeticionesReferencias> ObtenerReferencia(int idReferencia);
    }
}
