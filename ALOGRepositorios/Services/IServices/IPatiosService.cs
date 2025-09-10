using ALOG.Modelos.Modelos.Catalogos;

namespace ALOGRepositorios.Services.IServices
{
    public interface IPatiosService
    {

        public ICollection<CatPatios> GetPatios();
    }
}
