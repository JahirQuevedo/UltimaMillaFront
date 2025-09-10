using ALOG.Modelos.Modelos.Catalogos;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Dtos.Catalogos
{
    public class CatUsuariosPermisosDTO
    {
        public int IdCatUsuariosPermisos { get; set; }


        public int IdCatUsuarios { get; set; }
        public CatUsuarios CatUsuarios { get; set; }


        public int IdCatPermisos { get; set; }
        public CatPermisos CatPermisos { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }
    }
}
