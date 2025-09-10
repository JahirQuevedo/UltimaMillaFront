using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatProveedorService : ICatProveedorService
    {
        private readonly HttpClient _httpClient;
        public CatProveedorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CatProveedores> ObtenerProveedor(int pIdCatProveedor)
        {
            ////Console.WriteLine($"{Inicializar.UrlApi}APICatalogos/CatProveedores/Obtener/{pIdCatProveedor}");
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatProveedores/Obtener/{pIdCatProveedor}");
            var content = await response.Content.ReadAsStringAsync();
            var objRespuesta = JsonConvert.DeserializeObject<CatProveedores>(content);
            return objRespuesta;
        }

        public async Task<ICollection<CatProveedores>> ObtenerProveedores()
        {
            ////Console.WriteLine($"{Inicializar.UrlApi}APICatalogos/CatProveedores/Listar");
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatProveedores/Listar");

            var content = await response.Content.ReadAsStringAsync();
            var objrespuesta = JsonConvert.DeserializeObject<ICollection<CatProveedores>>(content);
            return objrespuesta;
        }

        public async Task<ICollection<RespListarCoincidenciasDTO>> ObtenerProveedoresConincidencia(string pCoincidencia)
        {
            ////Console.WriteLine($"{Inicializar.UrlApi}APICatalogos/CatProveedores/ListarCoincidencia/{pCoincidencia}");
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatProveedores/ListarCoincidencia/{pCoincidencia}");

            var content = await response.Content.ReadAsStringAsync();
            var objrespuesta = JsonConvert.DeserializeObject<ICollection<RespListarCoincidenciasDTO>>(content);
            return objrespuesta;
        }
    }
}
