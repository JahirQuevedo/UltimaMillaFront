using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Catalogos {
    public class CatServicio {
        
        public int IdCatServicio { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int IdCatEmpresas { get; set; } = 0;
        public int IdUsuarioRegistro { get; set; } = 0;
        public bool IsSelected { get; set; } = false;
    }
}
