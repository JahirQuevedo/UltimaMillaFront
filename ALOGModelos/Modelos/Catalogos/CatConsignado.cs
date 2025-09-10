using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Catalogos {
    public class CatConsignado {
        [Key]
        public int Id {  get; set; }
        public string RazonSocial { get; set; }
        public string Rfc {  get; set; }
    }
}
