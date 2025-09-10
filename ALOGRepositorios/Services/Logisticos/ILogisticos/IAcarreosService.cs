
using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.DTO.Consultas;
using ALOG.Modelos.Modelos.DTO.Respuestas;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface IAcarreosService
    {
        //Task<RespEntidadErrorDTO> CrearAcarreo(DtAcarreos pAcarreo);
        Task<RespEntidadErrorDTO> CrearAcarreo(DtAcarreos acarreo);
        Task<bool> Actualizar(DtAcarreos pAcarreo);
        Task<DtAcarreos> ObtenerAcarreo(int idAcarreo);
        Task<ICollection<RespObtenerAcarreosDTO>> ObtenerAcarreos(FiltroDtAcarreosDTO pFiltro);

        Task<bool> CambiarEstado(int pIdAcarreo, int pIdEstado);

    }
}
