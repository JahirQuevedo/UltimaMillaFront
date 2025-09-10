using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatFormatoReferenciasDTO
    {
        public int IdCatFormatoRef { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        
        public int IdCatLineaNegocio { get; set; }
        
        public int IdUsuarioRegistro { get; set; }

        public ICollection<CatFormatoRefDetDTO> CatFormatoRefDet { get; set; }


    }
}
