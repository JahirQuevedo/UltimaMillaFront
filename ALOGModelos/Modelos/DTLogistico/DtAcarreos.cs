using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
using ALOG.Modelos.Modelos.Orden;
namespace ALOG.Modelos.Modelos.DTLogistico
{
    public class DtAcarreos
    {
        [Key]
        public int IdDtAcarreos { get; set; }
        public int Mes { get; set; }
        public DateTime Fecha { get; set; }
        public string Servicio { get; set; }
        public string Contenedor { get; set; }
        public string Cliente { get; set; }
        [ForeignKey("catClientes")]
        public int IdCliente { get; set; }
        public CatClientes catClientes { get; set; }

        [ForeignKey("catEmpresa")]
        public int IdEmpresa { get; set; }
        public CatEmpresas catEmpresa { get; set; }

        [ForeignKey("ordenes")]
        public int IdOrden { get; set; }
        public Ordenes ordenes { get; set; }

        public bool Activo { get; set; } = true;

        [ForeignKey("catTipoEstados")]
        public int IdCatTipoEstado { get; set; }
        public CatTipoEstados catTipoEstados { get; set; }

        [ForeignKey("catServicios")]
        public int IdCatServicio { get; set; }
        public CatServicios catServicios { get; set; }
        public DateTime FechaRegistro { get; set; }

        [ForeignKey("catUsuario")]
        public int IdUsuarioRegistro { get; set; }

        public CatUsuarios catUsuario { get; set; }

        [ForeignKey("catProveedores")]
        public int IdCatProveedor { get; set; }

        public CatProveedores catProveedores { get; set; }


        //[ForeignKey("catProvedores")]
        //public int? IdCatProveedor { get; set; }
        //public CatProveedores catProvedores { get; set; }


    }
}
