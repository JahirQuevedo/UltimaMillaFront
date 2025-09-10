using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using ALOG.Modelos.Modelos.Dtos;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatClientesContactos  
    {
        [Key]
        public int IdCatCteContacto { get; set; }
        [Required]
        [ForeignKey("catClientes")]
        public int IdCatCliente { get; set; }
        public CatClientes catClientes { get; set; }

        [Required]
        [ForeignKey("catTipoContactos")]
        public int IdCatTipoContacto { get; set; }
        public CatTipoContacto catTipoContacto { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
