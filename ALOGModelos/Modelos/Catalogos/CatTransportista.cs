namespace ALOG.Modelos.Modelos.Catalogos {
    public class CatTransportista {

        public int IdCatTransportista { get; set; } = 0;
        public string RazonSocial { get; set; } = string.Empty;
        public string Rfc { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono {  get; set; } = string.Empty;
        public string Calle {  get; set; } = string.Empty;
        public string NumeroEterior { get; set; } = string.Empty;
        public string Colonia {  get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Ciudad {  get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string NumeroInterior { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public string Acronimo { get; set; } = string.Empty;
        public int IdCatPaises {get; set; } = 0;
        public int IdCatPaisEstados { get; set; } = 0;
        public string FechaRegistro { get; set; } = string.Empty;
        public int IdUsuarioRegistro { get; set; } = 0;
        public int IdCatEmpresas { get; set; } = 0;
        
    }
}
