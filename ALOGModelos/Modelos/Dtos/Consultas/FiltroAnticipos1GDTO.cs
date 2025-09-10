
using ALOG.Modelos.Modelos.Vacios;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.DTO.Consultas
{
    public class FiltroAnticipos1GDTO
    {

        public int? IdIntAnticipoSol { get; set; }

        public int? IdOrden { get; set; }


        public int? IdPeticionesReferencia { get; set; }


        public int? IdPeticionesContenedor { get; set; }



        public string IdCompaniaExterna { get; set; } = string.Empty;

        public string IdSolicitudAnticipoProveedor { get; set; } = string.Empty;

        public string ClaveProveedorExterno { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public string Referencia { get; set; } = string.Empty;

        public string Contenedor { get; set; } = string.Empty;

        public double MontoTotal { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime FechaEnvio { get; set; }
        public bool Enviado { get; set; } = false;
        public string Estado1G { get; set; } = string.Empty;
        public string RespuestaWS1G { get; set; } = string.Empty;
        public string Nota1G { get; set; } = string.Empty;

    }
}
