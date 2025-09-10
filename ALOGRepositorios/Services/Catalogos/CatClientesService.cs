using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Catalogos
{
    public class CatClientesService : ICatClientesService
    {

        private readonly HttpClient _httpClient;

        public CatClientesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICollection<CatClientes>> GetClientes()
        {
            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatClientes/Listar");
            var content = await response.Content.ReadAsStringAsync();
            var clientes = JsonConvert.DeserializeObject<ICollection<CatClientes>>(content);
            return clientes;
        }

        public async Task<ICollection<RespListarCoincidenciasDTO>> GetClientesConincidencia(string pCoincidencia)
        {
            ////Console.WriteLine($"{Inicializar.UrlApi}CatClientes/ListarCoincidencia/{pCoincidencia}");

            var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatClientes/ListarCoincidenciaWMS/{pCoincidencia}");

            var content = await response.Content.ReadAsStringAsync();
            var clientes = JsonConvert.DeserializeObject<ICollection<RespListarCoincidenciasDTO>>(content);
            return clientes;
        }

        //public async Task<Cliente> CrearCliente(Cliente cliente) { 

        //    var content = JsonConvert.SerializeObject(cliente);
        //    var bodyContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync($"{Inicializar.UrlApi}");
        //}
    }
}
