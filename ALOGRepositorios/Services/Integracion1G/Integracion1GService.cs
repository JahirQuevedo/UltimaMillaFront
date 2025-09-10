
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.Orden;
using ALOGRepositorios.Services.Integracion1G.IIntegracion1G;

namespace ALOGRepositorios.Services.Integracion1G
{
    public class Integracion1GService : IIntegracion1GService
    {

        private readonly HttpClient _httpClient;

        public Integracion1GService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public Task<RespuestaGenericaDTO> GenerarSolicitudFacturacion1G(Ordenes orden)
        {
            throw new NotImplementedException();
        }
    }
}
