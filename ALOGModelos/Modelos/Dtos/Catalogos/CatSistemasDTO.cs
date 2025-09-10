using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatSistemasDTO
    {
        public int IdCatSistema { get; set; }
        [Required]
        [StringLength(150)]
        public string nombre { get; set; }
        [Required]
        public bool activo { get; set; } = true;
        [Required]
        public string userSistema { get; set; }
        [Required]
        public string passSistema { get; set; }
        [Required]
        public string salt { get; set; }

        public string rol { get; set; }

        
        public int IdCatCliente { get; set; }
        public CatClientes CatClientes { get; set; }
        
        public int IdCatEmpresas { get; set; } = 1;
        public virtual CatEmpresas catEmpresas { get; set; }
    }
}
