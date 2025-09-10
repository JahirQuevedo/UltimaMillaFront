using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatLineaNegocio
    {

        [Key]
        public int IdCatLineaNegocio { get; set; }
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        [MaxLength(20)]
        public string Acronimo { get; set; } //3PL 3PLQ LV
        [Required]

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]

        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuario { get; set; }


        public ICollection<CatLineaNegocioTarifa> GetCatLineaNegocioTarifas { get; set; }

    }
}
