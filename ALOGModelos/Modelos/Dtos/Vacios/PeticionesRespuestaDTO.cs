using System.Net;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Vacios
{
    public class PeticionesRespuestaDTO
    {
        public PeticionesRespuestaDTO()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        //public object Result { get; set; }
        public int IdReferenciaALO { get; set; }
        public int IdOrdenServicio { get; set; }
    }
}
