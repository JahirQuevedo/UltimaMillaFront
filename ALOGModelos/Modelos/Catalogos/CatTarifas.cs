using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatTarifas
    {

        [Key]
        public int IdCatTarifas { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }
        [MaxLength(500)]
        public string Descripcion { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        public int IdCatUsuario { get; set; }

        public int Clave1G { get; set; }

        //Tipo Proveedor , Patio
        [MaxLength(50)]
        public string TipoTarifa { get; set; }
        //[Required]
        //public double Precio { get; set; }
        //[Required]
        //public double Impuesto { get; set; }





    }
}
