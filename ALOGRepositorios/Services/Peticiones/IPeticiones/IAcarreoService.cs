
using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.DTO.Consultas;

namespace ALOGRepositorios.Services.Peticiones.IPeticiones
{
    public interface IAcarreoService
    {

        public Task<DtAcarreos> ObtenerAcarreo(int idAcarreo);
        public Task<ICollection<DtAcarreos>> ObtenerAcarreos(FiltroDtAcarreosDTO pFiltro);
        public Task<DtAcarreos> CrearAcarreo(DtAcarreos acarreo);

    }
}
