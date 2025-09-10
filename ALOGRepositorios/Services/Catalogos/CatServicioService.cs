using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatServicioService : ICatServicioService
    {

        private readonly HttpClient _httpClient;

        public CatServicioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ICollection<CatServicios>> GetServicios()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatServicios/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var servicios = JsonConvert.DeserializeObject<ICollection<CatServicios>>(content);
            return servicios;
        }

        public async Task<ICollection<CatServicios>> ObtenerServiciosConincidencia(string pCoincidencia)
        {
            ////Console.WriteLine($"{Inicializar.UrlApi}APICatalogos/CatServicios/ListarCoincidencia/{pCoincidencia}");
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatServicios/ListarCoincidencia/{pCoincidencia}");

            var content = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine(content);
            var objrespuesta = JsonConvert.DeserializeObject<ICollection<CatServicios>>(content);
            return objrespuesta;
        }

        public async Task<List<CatLineaNegocioTariPrecio>> ObtenerTarifarioServicios(string parametroEncriptado)
        {

            List<CatLineaNegocioTariPrecio> tarifarioServicios = new List<CatLineaNegocioTariPrecio>();

            try
            {
                var response = await _httpClient.GetAsync($"{Inicializar.UrlApiLogistico}operativo/VReferencias/ObtenerTarifarioServicios/{parametroEncriptado}");
                var content = await response.Content.ReadAsStringAsync();
                tarifarioServicios = JsonConvert.DeserializeObject<List<CatLineaNegocioTariPrecio>>(content);
            }
            catch (Exception ex)
            {

            }
            return tarifarioServicios;
        }

        public async Task<RespuestaGenericaDTO> ObternerPorId(int pIdCatServicio)
        {
            ////Console.WriteLine($"{Inicializar.UrlApi}APICatalogos/CatServicios/Obtener/{pIdCatServicio}");
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatServicios/Obtener/{pIdCatServicio}");

            var content = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine("ObternerId:" + content);
            var objrespuesta = JsonConvert.DeserializeObject<RespuestaGenericaDTO>(content);
            return objrespuesta;
        }


    }
}
