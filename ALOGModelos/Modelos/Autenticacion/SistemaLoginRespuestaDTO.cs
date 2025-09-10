using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Autenticacion
{
    public class SistemaLoginRespuestaDTO
    {
        public string Token { get; set; }
        public string Usuario { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public bool Exito { get; set; }

    }
}
