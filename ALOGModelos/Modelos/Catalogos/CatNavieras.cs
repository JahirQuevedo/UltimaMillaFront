using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatNavieras
    {
        [Key]
        public int IdCatNaviera { get; set; } = 0;
        public string RazonSocial { get; set; } = string.Empty;
        public string Acronimo { get; set; } = string.Empty;
        public string RFC { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
