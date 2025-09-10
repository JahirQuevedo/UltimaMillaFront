using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTLogistico;
using ALOG.Modelos.Modelos.Dtos;
using ALOG.Modelos.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Vacios;
using ALOG.Modelos.Modelos.Vacios.Peticiones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos.Modelos.Orden
{
    public class Ordenes
    {
        [Key]
        public int IdOrden { get; set; }


        [ForeignKey("catClientes")]
        public int IdCatCliente { get; set; }
        public virtual CatClientes catClientes { get; set; }


        [ForeignKey("catSistemas")]
        public int? IdCatSistema { get; set; }
        public virtual CatSistemas catSistemas { get; set; }


        [ForeignKey("catAduana")]
        public int? IdCatAduana { get; set; }
        public virtual CatAduana catAduana { get; set; }

        [ForeignKey("catProveedores")]
        public int? IdCatProveedor { get; set; }
        public virtual CatProveedores catProveedores { get; set; }


        [ForeignKey("CatEmpresas")]
        public int IdCatEmpresa { get; set; }
        public virtual CatEmpresas CatEmpresas { get; set; }


        [ForeignKey("CatSucursales")]
        public int IdCatSucursal { get; set; }
        public virtual CatSucursales CatSucursales { get; set; }



        [ForeignKey("CatLineaNegocio")]
        public int IdCatLineaNegocio { get; set; }
        public virtual CatLineaNegocio CatLineaNegocio { get; set; }


        [ForeignKey("catProyectos")]
        public int? IdCatProyecto { get; set; }
        public virtual CatProyectos catProyectos { get; set; }

        //[ForeignKey("peticionesReferencias")]
        //public int? IdPeticionReferencia { get; set; }
        //public virtual PeticionesReferencias peticionesReferencias { get; set; }

        [ForeignKey("catUsuario")]
        public int? IdUsuario { get; set; }
        public virtual CatUsuarios catUsuario { get; set; }


        //[ForeignKey("IdPeticionReferencia")]
        //public PeticionesReferencias PeticionReferencia { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;

        public string ReferenciaALO { get; set; }

        public ICollection<PeticionesReferencias> peticionesReferencias { get; set; }
        public ICollection<DtAcarreos> dtAcarreos { get; set; }
        public ICollection<DtUltimaMillaEnc> dtUltimaMillaEnc { get; set; }
        //3PL
        //SL
        //AUTOS


    }
}
