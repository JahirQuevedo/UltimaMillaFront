using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ALOGModelos.Modelos.Logisticos
{
    public class UltimaMillaDetalle
    {

        [JsonProperty("idDtUltimaMillaDet")]
        public int Id { get; set; } = 0;

        [JsonProperty("numeroParte")]
        [Required(ErrorMessage = "Debes indicar el número de parte.")]
        public string NumeroParte { get; set; } = string.Empty;

        [JsonProperty("piezas")]
        [Range(1, int.MaxValue, ErrorMessage = "El número de piezas no puede ser menor a cero.")]
        public int Piezas { get; set; } = 0;

        [JsonProperty("pallet")]
        [Range(1, int.MaxValue, ErrorMessage = "El número de pallets no puede ser menor a cero.")]
        public int Pallet { get; set; } = 0;

        [JsonProperty("idUltimaMilla")]
        public int IdUltimaMillaEncabezado { get; set; } = 0;
    }
}
