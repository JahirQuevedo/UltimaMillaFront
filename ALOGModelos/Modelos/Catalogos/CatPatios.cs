using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
using ALOG.Modelos.Modelos.Catalogos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatPatios
    {

        [Key]
        public int IdCatPatios { get; set; }
        [Required]
        [MaxLength(200)]
        public string RazonSocial { get; set; }

       

        public DateTime FechaRegistro { get; set; }

        [ForeignKey("catUsuarios")]
        public int IdUsuarioRegistro { get; set; }
        public CatUsuarios catUsuarios { get; set; }
        public ICollection<CatPatiosConfig> GetCatPatiosConfigs { get; set; }
        public ICollection<CatPatiosNavieras> GetCatPatiosNavieras { get; set; }
        public bool Activo { get; set; }
    }
}
