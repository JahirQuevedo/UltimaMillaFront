using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Dtos.Solicitudes
{
    public class SolCambioEstadoDTO
    {
        public string TipoCambio { get; set; }
        public int IdReferencia { get; set; }
        public int IdOrden { get; set; }
        public int IdContenedor { get; set; }
        public int IdServicio { get; set; }
        public int IdDocumento { get; set; }
        public string ReferenciaCliente { get; set; }
        public int IdCatReferenciaEstado { get; set; }
    }
}
