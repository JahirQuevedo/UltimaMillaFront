
using ALOG.Modelos.Modelos.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ALOG.Modelos.Modelos.Catalogos
{
    
    public class CatSistemas  
    {

        [Key]
        public int IdCatSistema { get; set; }
        [Required]
        [StringLength(150)]
        public string nombre { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string userSistema { get; set; }
        [JsonIgnore]
        [Required]
        [MaxLength(500)]
        public string passSistema { get; set; }
        [JsonIgnore]
        [Required]
        [MaxLength(500)]
        public string salt { get; set; }
        [MaxLength(50)]
        public string rol { get; set; }

        [ForeignKey("CatClientes")]
        public int IdCatCliente { get; set; } 
        public CatClientes CatClientes { get; set; }
        [ForeignKey("catEmpresas")]
        public int IdCatEmpresas { get; set; } = 1;
        public virtual CatEmpresas catEmpresas { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }= DateTime.Now;
    }
}
