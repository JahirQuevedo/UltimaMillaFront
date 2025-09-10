using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Dtos.Solicitudes
{
    public class SolDatosUsuariosDTO
    {
        public int IdCatUsuario { get; set; }
        public int IdCatRoles { get; set; }
        public int IdCatPermiso { get; set; }
        public bool Activo { get; set; }
        public int IdCatEmpresa { get; set; }


    }
}
