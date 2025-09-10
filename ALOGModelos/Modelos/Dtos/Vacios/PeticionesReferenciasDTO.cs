using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Vacios;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos.Vacios;
namespace ALOG.Modelos.Modelos.Dtos.Vacios
{
    public class PeticionesReferenciasDTO
    {

        public int IdReferencia { get; set; }

        [Required]
        public int Ticket { get; set; }
        [Required]
        public string Transporte_RFC { get; set; }
        [Required]
        public string Transporte_RazonSocial { get; set; }

        public int TrasporteId { get; set; }
        [Required]
        public string Transporte_Usuario { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Transporte_UsuarioEmail { get; set; }
        public string Comentarios { get; set; }
        [Required]
        public int TipoReferencia { get; set; }
        public ICollection<PeticionesContenedoresDTO> Contenedores { get; set; }
        public CatPatios catPatios { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        public DateTime FechaProcesado { get; set; }
        public string EstadoReferencia { get; set; } = "A";

        public int IdCatReferenciaEstado { get; set; }

        public string Procesado { get; set; }

        public int IdOrden { get; set; }

        public bool Activo { get; set; } = true;



    }
}
