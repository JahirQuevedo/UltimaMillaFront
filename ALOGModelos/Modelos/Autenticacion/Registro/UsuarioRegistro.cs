using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Autenticacion.Registro
{
    public class UsuarioRegistro
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
