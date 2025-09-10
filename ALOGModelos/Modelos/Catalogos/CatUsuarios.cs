using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatUsuarios
    {
        [Key]
        public int IdCatUsuarios { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }
        [MaxLength(100)]
        public string ApellidoMaterno { get; set; }
        [MaxLength(100)]
        [Required]
        public string ApellidoPaterno { get; set; }
        [Required]
        [MaxLength(20)]
        public string RFC { get; set; }
        [StringLength(25)]
        public string CURP { get; set; } = null;
        [Required]
        public string Correo { get; set; }
        [Required]
        [MaxLength(100)]
        public string Puesto { get; set; }
        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; }
        [MaxLength(10)]
        public string Extension { get; set; }
        [MaxLength(20)]
        public string Celular { get; set; }
        [MaxLength(100)]
        public string Usuario { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
        [JsonIgnore]
        [Required]
        [MaxLength(500)]
        public string passSistema { get; set; }
        [JsonIgnore]
        [Required]
        [MaxLength(500)]
        public string salt { get; set; }

        [ForeignKey("catTipoPuesto")]
        public int? IdCatTipoPuesto { get; set; }
        public CatTipoPuesto catTipoPuesto { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public ICollection<CatUsuarioRoles> catUsuarioRoles { get; set; }
        public ICollection<CatUsuariosAduanas> catUsuariosAduanas { get; set; }
        [JsonProperty("catUsuariosEmpresas")] public ICollection<CatUsuariosEmpresa> catUsuariosEmpresas { get; set; }
        public ICollection<CatUsuariosPermisos> catUsuariosPermisos { get; set; }


    }

}
