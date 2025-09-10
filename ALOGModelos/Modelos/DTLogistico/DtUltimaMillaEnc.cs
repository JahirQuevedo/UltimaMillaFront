using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ALOG.Modelos.Modelos.Orden;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.DTLogistico
{
    public class DtUltimaMillaEnc
    {
        [Key]
        public int IdDtUltMillaEnc { get; set; }

        public DateTime FechaSolicitud { get; set; }
        [Required]
        public int Viaje { get; set; }

        [ForeignKey("catClientes")]
        public int? IdCliente { get; set; }
        public CatClientes catClientes { get; set; }

        [Required]
        public string Cliente { get; set; }

        [Required]
        public string FacturaCliente { get; set; }

        public string Bodega { get; set; }

        [ForeignKey("catEmpresa")]
        public int IdCatEmpresa { get; set; }
        public CatEmpresas catEmpresa { get; set; }

        public DateTime FechaSalida { get; set; }

        [ForeignKey("catTipoEstados")]
        public int IdTipoEstado { get; set; }
        public CatTipoEstados catTipoEstados { get; set; }

        [ForeignKey("ordenes")]
        public int IdOrden { get; set; }
        public Ordenes ordenes { get; set; }

        [ForeignKey("catServicios")]
        public int IdCatServicio { get; set; }
        public CatServicios catServicios { get; set; }

        [ForeignKey("catProveedores")]
        public int IdCatProveedor { get; set; }
        public CatProveedores catProveedores { get; set; }

        public ICollection<DtUltimaMillaDet> DtUltimaMillaDets { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }


    }
}
