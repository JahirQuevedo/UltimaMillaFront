namespace ALOGRepositorios.Services.Control.IControl
{
    public interface IControlService
    {

        public Task<string> GetFiltroCifrado(object filtro);
    }
}
