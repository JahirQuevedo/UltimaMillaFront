namespace ALOGRepositorios.Services.Utilerias.IUtilerias
{
    public interface IExportarExcelService
    {
        Task ExportarExcelAsync<T>(List<T> datos, string nombreArchivo = "Reporte.xlsx");
        Task<List<Dictionary<string, string>>> CargarExcelAsync(MultipartFormDataContent contenido);
    }
}
