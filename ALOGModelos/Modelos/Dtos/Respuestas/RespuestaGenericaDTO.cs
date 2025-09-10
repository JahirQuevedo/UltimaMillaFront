using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Dtos.Respuestas
{
    public class RespuestaGenericaDTO
    {

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string strMensaje { get; set; }
        public List<string> lstrErrorMessages { get; set; }
        public object Entidad { get; set; }
        public List<object> Entidades { get; set; }
    }
}
