using System.Net;

namespace ALOG.Modelos.Modelos.DTO.Control
{
    public class SistemaLoginRespuestaDTO
    {
        public string Token { get; set; }
        public string Usuario { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }




    }
}
