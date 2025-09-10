using ALOG.Modelos.Modelos.Vacios;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Vacios
{
    public class PeticionesServiciosDTO
    {

        public int IdServicio { get; set; }
        [Required]
        public int IdTipoServicio { get; set; }
        public string DescServicio { get; set; }
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaCierre { get; set; }
        public int IdContenedor { get; set; }

        public ICollection<PeticionesDocumentosDTO> Documentos { get; set; }

        public string EstadoServicio { get; set; } = "A";
        public bool Activo { get; set; } = true;


    }
}
