using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.DTO.Consultas
{
    public class FiltroDtUltimaMillaDTO
    {
        public int? IdDtUltimaMillaDet { get; set; }

        public string NumeroParte { get; set; } = string.Empty;

        public int? Piezas { get; set; }

        public int? Pallet { get; set; } = 0;

        public int? IdDtUltMillaEnc { get; set; }

        public DateTime? FechaSolicitudIni { get; set; }
        public DateTime? FechaSolicitudFin { get; set; }

        public int? Viaje { get; set; }


        public int? IdCliente { get; set; }



        public string Cliente { get; set; }

        public string Factura { get; set; }

        public string FacturaCliente { get; set; }

        public string Bodega { get; set; }


        public int? IdCatEmpresa { get; set; }



        public DateTime? FechaSalidaIni { get; set; }
        public DateTime? FechaSalidaFin { get; set; }


        public int? IdTipoEstado { get; set; }

        public int? IdOrden { get; set; }
        public bool? Activo { get; set; }
        public int NumeroPagina { get; set; }
        public int NumeroRegistros { get; set; }
    }
}
