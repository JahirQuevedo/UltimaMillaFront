using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ALOG.Modelos.Modelos.Dtos;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatUsuariosPermisos  
    {
        [Key]
        public int IdCatUsuariosPermisos { get; set; }

        [ForeignKey("CatUsuarios")]
        public int IdCatUsuarios { get; set; }
        public CatUsuarios CatUsuarios { get; set; }

        [ForeignKey("CatPermisos")]
        public int IdCatPermisos { get; set; }
        public CatPermisos CatPermisos { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }= DateTime.Now;

        public bool Crear { get; set; } = true;
        public bool Guardar { get; set; } = true;
        public bool Actualizar  { get; set; } = true;
        public bool Eliminar { get; set; } = true;
        public bool Impresion { get; set; } = true;
        public bool Exportar { get; set; } = true;
        public bool Notificar { get; set; } = true;
        public bool Autorizar { get; set; } = false;
        public bool Enviar { get; set; } = false;

    }
}
