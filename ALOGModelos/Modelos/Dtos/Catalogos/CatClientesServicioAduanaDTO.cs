using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatClientesServicioAduanaDTO
    {
        public int IdCatCteServAduana { get; set; }        
        public int IdCatClientes { get; set; }        
        [Required]        
        public int IdCatAduana { get; set; }
        public CatAduanaDTO catAduanas { get; set; }
        
        [Required]        
        public int IdCatServicio { get; set; }
        public CatServiciosDTO catServicios { get; set; }

        [Required]
        public bool Activo { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        [Required]        
        public int IdUsuarioRegistro { get; set; }

        public ICollection<CatClienteTarifaDTO> GetCatClienteTarifas { get; set; }



    }
}
