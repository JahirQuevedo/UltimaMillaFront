using ALOG.Modelos;

namespace ALOGRepositorios.Services;

public interface IServicioFotograficoService
{
    Task<ResultBase> Guardar(CargaArchivoFotografico cargaArchivoFotografico);

    Task<ResultBase> Eliminar(int idServicioFotografico);

    Task<PaginadoResult<MonitorServicioFotografico>> ObtenerListaPaginada(ConsultaCatalogoBase<ConsultaServicioFotografico> entidad);

    Task<ResultBase<ArchivoBase>> ObtenerZipArchivos(MultipleSeleccionArchivoBase multipleSeleccionArchivoBase);

}
