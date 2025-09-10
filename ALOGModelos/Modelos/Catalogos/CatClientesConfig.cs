using ALOG.Modelos.Modelos.Dtos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatClientesConfig  
    {
        [Key]
        public int IdCatClientesConfig { get; set; }
        [Required]
        [ForeignKey("CatClientes")]
        public  int IdCatClientes { get; set; }
        public virtual CatClientes CatClientes { get; set; }

        [Required]
        [ForeignKey("catTiposConfig")]
        public int IdCatTipoConfig { get; set; }
        public virtual CatTiposConfig catTiposConfig { get; set; }
        [Required]
        public bool Activo { get; set; } = true;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
       

        public Double Valor1 { get; set; }
        [MaxLength(1500)]
        public String Valor2 { get; set; }

        [Required]        
        [ForeignKey("catUsuarios")]
        public int IdUsuario { get; set; }
        public virtual CatUsuarios catUsuarios { get; set; }
    }
}
