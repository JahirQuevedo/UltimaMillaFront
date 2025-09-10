
using ALOG.Modelos.Modelos.Catalogos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALOG.Modelos.Modelos.Logisticos
{
    public class Acarreo
    {

        [JsonProperty("idDtAcarreos")] public int Id { get; set; } = 0;

        [JsonProperty("mes")] public int Mes { get; set; } = 0;

        [JsonProperty("fecha")]

        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Debe indicar la fecha de registro.")]
        [JsonProperty("fechaRegistro")] public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [JsonProperty("servicio")] public string NombreServicio { get; set; } = string.Empty;
        [JsonProperty("contenedor")]
        [Required(ErrorMessage = "El número de contenedor no puedo estar vacío")]
        [RegularExpression(@"^[A-Z]{4}\d{7}$", ErrorMessage = "El número de contenedor debe contener 4 letras seguidas de 7 números.")]
        public string NumeroContenedor { get; set; } = string.Empty;
        [JsonProperty("idCliente")] public int IdCliente { get; set; } = 0;
        [JsonProperty("catClientes")]
        public CatClientes Cliente { get; set; }
        [Required(ErrorMessage = "Debe indicar el estado de la orden.")]
        [JsonProperty("idCatTipoEstado")] public int IdCatTipoEstado { get; set; } = 0;
        [JsonProperty("catTipoEstados")]
        public CatTipoEstado catTipoEstados { get; set; }
        [JsonProperty("cliente")] public string NombreCliente { get; set; } = string.Empty;
        [JsonProperty("idEmpresa")] public int IdEmpresa { get; set; } = 0;
        [JsonProperty("idOrden")] public int IdOrden { get; set; } = 0;
        [JsonProperty("activo")] public bool Activo { get; set; } = true;

        [JsonProperty("idCatServicio")] public int IdCatServicio { get; set; } = 0;
        [JsonProperty("catServicios")]
        public CatServicios Servicio { get; set; }


        public int IdCatProveedor { get; set; }
        public CatProveedores catProveedores { get; set; }

        [JsonProperty("idUsuarioRegistro")] public int IdUsuarioRegistro { get; set; }

        [JsonProperty("catUsuario")] public CatUsuarios catUsuario { get; set; }
    }
}
