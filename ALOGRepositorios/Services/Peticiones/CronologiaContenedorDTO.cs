namespace ALOGRepositorios.Services.Peticiones
{
    public class CronologiaContenedorDTO
    {
        public int IdContenedorCron { get; set; }
        public string TipoIncidencia { get; set; } = string.Empty;
        public string TipoEvento { get; set; } = string.Empty;
        public DateTime FechaEvento { get; set; }
        public string Comentario { get; set; } = string.Empty;

        public int IdCatTipoIncidenciaEvento { get; set; }
        public int IdContenedor { get; set; }
    }
}
