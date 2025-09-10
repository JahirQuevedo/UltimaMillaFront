using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Catalogos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatLineaNegocioTariPrecio
    {
        [Key]
        public int IdCatLineNegocioTariPrecio { get; set; }
        [Required]

        public int IdCatLineaNegocioTarifa { get; set; }
        public CatLineaNegocioTarifa catLineaNegocioTarifa { get; set; }
        [Required]
        public double Precio { get; set; }
        [Required]
        public double Impuesto { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        public bool Activo { get; set; } = true;
        [Required]

        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuario { get; set; }
        [Required]

        public int IdCatAduana { get; set; }
        public CatAduana catAduana { get; set; }

        //Id
        //Id CatLineaNegocioTarifa
        //Id Padre
        //Pecios
        //Impuesto
        //Fecha de operación inicio, fin,
        //Precio        
        //Impuesto
        //Aduana

    }
}
