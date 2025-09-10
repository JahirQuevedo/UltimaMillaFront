using ALOG.Modelos.Modelos.Vacios;

namespace ALOGRepositorios.Services.IServices
{
    public interface IContenedoresService
    {

        public ICollection<PeticionesContenedores> GetContenedores();
    }
}
