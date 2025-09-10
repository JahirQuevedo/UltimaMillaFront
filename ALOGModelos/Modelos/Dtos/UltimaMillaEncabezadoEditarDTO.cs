using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Logisticos;
using ALOG.Modelos.Modelos.Vacios.Peticiones;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ALOG.Modelos.Modelos.Dtos
{
    public class UltimaMillaEncabezadoEditarDTO
    {
        [JsonProperty("idDtUltMillaEnc")]
        public int Id { get; set; } = 0;

        [JsonProperty("fechaSolicitud")]
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        [JsonProperty("viaje")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de Viaje no puede ser menor a cero.")]
        public int Viaje { get; set; } = 0;

        [JsonProperty("idCliente")]
        [Range(1, int.MaxValue, ErrorMessage = "El cliente seleccionado no es válido.")]
        public int IdCliente { get; set; } = 0;

        [JsonProperty("cliente")]
        public string NombreCliente { get; set; } = string.Empty;

        [JsonProperty("facturaCliente")]
        [Required(ErrorMessage = "Debe indicar el folio de factura.")]
        public string FacturaCliente { get; set; } = string.Empty;

        [JsonProperty("bodega")]
        [Required(ErrorMessage = "Debe indicar la bodega.")]
        public string Bodega { get; set; } = string.Empty;

        [JsonProperty("idCatEmpresa")]
        public int IdCatEmpresa { get; set; } = 0;

        [JsonProperty("fechaSalida")]
        public DateTime FechaSalida { get; set; } = DateTime.Now;

        [JsonProperty("idTipoEstado")]
        public int IdTipoEstado { get; set; } = 0;

        [JsonProperty("idOrden")]
        public int IdOrden { get; set; } = 0;

        [JsonProperty("idCatServicio")]
        public int IdServicio { get; set; } = 0;
        [JsonProperty("activo")]
        public bool Activo { get; set; } = true;
        [JsonProperty("fechaRegistro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
