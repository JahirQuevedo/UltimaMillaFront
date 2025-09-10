using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.DTO.Consultas
{
    public class FiltroGenericoDTO
    {
        public int? IdCliente { get; set; }
        public int? IdOrden { get; set; }
        public int? IdLNegocio { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdSucursal { get; set; }

        public int? IdServicio { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdAduana { get; set; }
        public int? IdProveedor { get; set; }
        public int? IdTransportista { get; set; }
        public int? IdPais { get; set; }
        public int? IdPaisEstado { get; set; }
        public int? IdTipoPuesto { get; set; }
        public int? IdPermiso { get; set; }
        public int? IdRol { get; set; }
        public int? IdTipoDocumento { get; set; }
        public int? IdDocumento { get; set; }
        public int? IdProyecto { get; set; }
        public int? Id { get; set; }
        public string Nombre { get; set; }=string.Empty;
        public string ApellidoM { get; set; } = string.Empty;
        public string ApellidoP { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public string RFC { get; set; } = string.Empty;
        public string CURP { get; set; } = string.Empty;
        public string Calle { get; set; } = string.Empty;
        public string CP { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;



        public bool? Activo { get; set; }
        public DateTime? FechaRegistro { get; set; } 
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }



    }
}
