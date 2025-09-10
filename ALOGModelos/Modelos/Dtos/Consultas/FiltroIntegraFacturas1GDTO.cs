using ALOG.Modelos.Modelos;
using ALOG.Modelos.Modelos.Vacios;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ALOG.Modelos.Modelos.DTO.Consultas
{
    public class FiltroIntegraFacturas1GDTO
    {
        public int? IdIntFacturaEnc { get; set; }

        public int? IdOrden { get; set; }

        public int? IdPeticionesReferencia { get; set; }



        public string IdSolicitudFacturacion { get; set; } = string.Empty;

        public string IdCompaniaExterna { get; set; } = string.Empty;

        public string ClaveClienteExterno { get; set; } = string.Empty;

        public string Fecha { get; set; } = string.Empty;

        public string ConceptoFacturacion { get; set; } = string.Empty;

        public string Comentario { get; set; } = string.Empty;

        public string Nota { get; set; } = string.Empty;

        public string Referencia { get; set; } = string.Empty;

        public string ClaveSATMoneda { get; set; } = string.Empty;

        public string ClaveSATUsoCFDI { get; set; } = string.Empty;

        public string RFC { get; set; } = string.Empty;

        public bool FacturacionAutomatica { get; set; } = false;


        public bool CierreReferencia { get; set; } = false;

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public DateTime FechaEnvio { get; set; }

        public bool Enviado { get; set; } = false;

        public string Estado1G { get; set; } = string.Empty;
        public string RespuestaWS1G { get; set; } = string.Empty;


        public int? IdIntFacturaDet { get; set; }
        public string ClaveServicio { get; set; } = string.Empty;

        public string Cantidad { get; set; } = string.Empty;

        public string Precio { get; set; } = string.Empty;


        public int? IdPeticionesContenedor { get; set; }

        public string Contenedor { get; set; } = string.Empty;

        public string CentroCostos { get; set; } = string.Empty;

        public string EIR { get; set; } = string.Empty;

        /*
         * Filtro para Estado de Factura 1G
         */

        public int? IdIntFacturaEst { get; set; }


        public string Mensaje { get; set; } = string.Empty;


        public string FolioFactura { get; set; } = string.Empty;


        public double? MontoTotal { get; set; }

        public double? MontoPagado { get; set; }

        public double? MontoNotaCredito { get; set; }

        public double? SaldoFactura { get; set; }

        public DateTime? FechaUltimoPago { get; set; }

        public string EstatusFactura { get; set; } = string.Empty;

        public string EstatusSolicitud { get; set; } = string.Empty;


    }
}
