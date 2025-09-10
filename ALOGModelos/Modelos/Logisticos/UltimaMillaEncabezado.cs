using ALOGModelos.Modelos.Catalogos;
using ALOGModelos.Modelos.Dtos;
using ALOGModelos.Modelos.Orden;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ALOGModelos.Modelos.Logisticos
{
    public class UltimaMillaEncabezado
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
        [ForeignKey("catClientes")]
        public int IdCliente { get; set; } = 0;
        public CatClientes catClientes { get; set; }

        [JsonProperty("cliente")]
        public string NombreCliente { get; set; } = string.Empty;

        //[JsonProperty("factura")]
        //public string Factura { get; set; } = string.Empty;

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
        [ForeignKey("catTipoEstados")]
        public int IdTipoEstado { get; set; } = 0;
        public CatTipoEstados catTipoEstados { get; set; }

        [ForeignKey("ordenes")]
        [JsonProperty("idOrden")]
        public int IdOrden { get; set; } = 0;
        public Ordenes ordenes { get; set; }

        [ForeignKey("catServicios")]
        [JsonProperty("idCatServicio")]
        public int IdServicio { get; set; } = 0;
        public CatServicios catServicios { get; set; }

        [JsonProperty("dtUltimaMillaDets")]
        public ICollection<UltimaMillaDetalle> Detalles { get; set; }
        //[JsonProperty("catClientes")]
        //public CatClientes Cliente { get; set; }
        //[JsonProperty("catTipoEstados")]
        //public CatTipoEstado TipoEstado { get; set; }
        //[JsonProperty("ordenes")]
        //public Ordenes Orden { get; set; }
        //[JsonProperty("catServicios")]
        //public CatServicios Servicio { get; set; }
        [JsonProperty("activo")]
        public bool Activo { get; set; } = true;
        [JsonProperty("fechaRegistro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public int IdCatProveedor { get; set; }

    }
}
