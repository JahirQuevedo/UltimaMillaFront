namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface IDocumentoService
    {

        //public Task SubirArchivo(SolCargarArchivoDTO archivo);
        public Task<bool> SubirArchivo(SolCargarArchivoWASMDTO archivo);

    }
}
