using ALOG.Modelos.Modelos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.DTO.Consultas
{
    public class FiltroIntegraReferencias1GDTO
    {
        public int? IdIntReferencia { get; set; }

        public int? IdOrden { get; set; }

        public string IdCompaniaExterna { get; set; } = string.Empty;

        public string ReferenciaALO { get; set; } = string.Empty;

        public string ReferenciaClienteExterno { get; set; } = string.Empty;

        public string ClaveClienteExterno { get; set; } = string.Empty;

        public string Aduana { get; set; } = string.Empty;

        public bool? Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime FechaEnvio { get; set; }

        public bool? Enviado { get; set; } = false;
        public string Estado1G { get; set; } = string.Empty;
        public string RespuestaWS1G { get; set; } = string.Empty;

    }
}
