using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Autenticacion.Registro
{
    public class RespuestaRegistro
    {
        public bool registroCorrecto { get; set; }
        public IEnumerable<string> Errores { get; set; }
    }
}
