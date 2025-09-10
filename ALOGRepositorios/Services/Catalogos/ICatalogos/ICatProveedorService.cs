using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatProveedorService
    {
        Task<ICollection<RespListarCoincidenciasDTO>> ObtenerProveedoresConincidencia(string pCoincidencia);
        Task<ICollection<CatProveedores>> ObtenerProveedores();
        Task<CatProveedores> ObtenerProveedor(int pIdCatProveedor);

    }
}
