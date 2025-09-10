using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ALOGRepositorios.Services.Catalogos;

public class CatPaisesServices : ICatPaisesServices
{

    private readonly HttpClient _httpClient;
    private string token;

    public CatPaisesServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ICollection<CatPaises>> ObtenerPaisesConincidencia(string pConincidencia)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        ////Console.WriteLine($"{Inicializar.UrlApiCatalogos}CatPaises/ListarCoincidencia/{pConincidencia}");
        var response = await _httpClient.GetAsync($"{Inicializar.UrlApiCatalogos}CatPaises/ListarCoincidencia/{pConincidencia}");
        var content = await response.Content.ReadAsStringAsync();
        ////Console.WriteLine(content);
        var objrespuesta = JsonConvert.DeserializeObject<ICollection<CatPaises>>(content);
        return objrespuesta;
    }
}
