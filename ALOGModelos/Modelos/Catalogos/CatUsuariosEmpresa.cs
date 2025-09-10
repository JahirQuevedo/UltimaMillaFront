using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
using Newtonsoft.Json;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatUsuariosEmpresa
    {
        [Key]
        [JsonProperty("idCatUsuariosEmpresa")] public int IdCatUsuariosEmpresa { get; set; }

        [ForeignKey("CatUsuarios")]
        [JsonProperty("idCatUsuarios")] public int IdCatUsuarios { get; set; }
        public virtual CatUsuarios CatUsuarios { get; set; }

        [ForeignKey("catClientes")]
        public int? IdCatCliente { get; set; }
        public virtual CatClientes catClientes { get; set; }

        [ForeignKey("catEmpresas")]
        [JsonProperty("idCatEmpresa")]
        public int? idCatEmpresa { get; set; }
        public virtual CatEmpresas catEmpresas { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [ForeignKey("catProveedores")]
        public int? IdCatProveedor { get; set; }
        public virtual CatProveedores catProveedores { get; set; }

        //[ForeignKey("catUsuariosReg")]
        //public int IdUsuarioRegistro { get; set; }
        //public CatUsuarios catUsuariosReg { get; set; }
    }
}
