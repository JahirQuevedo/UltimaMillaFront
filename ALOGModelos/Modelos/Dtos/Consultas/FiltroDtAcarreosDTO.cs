using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.DTO.Consultas
{
    public class FiltroDtAcarreosDTO
    {
        public DateTime? FechaInicial { get; set; }

        public string Servicio { get; set; } = string.Empty;
        public string Contenedor { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public int? IdCliente { get; set; }
        public int? IdCatTipoEstados { get; set; }

        public int? IdEmpresa { get; set; }

        public int? IdOrden { get; set; }
        public int NumeroPagina { get; set; }
        public int NumeroRegistros { get; set; }
        public bool? Activo { get; set; }

        public DateTime FSolicitudIni { get; set; }
        public DateTime FSolicitudFin { get; set; }
        public DateTime FEntregaIni { get; set; }
        public DateTime FEntregaFin { get; set; }
        public int Mes { get; set; }


        public int? IdCatServicio { get; set; }


    }
}
