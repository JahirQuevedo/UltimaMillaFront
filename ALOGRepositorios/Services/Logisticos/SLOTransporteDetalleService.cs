using ALOG.Modelos.Modelos.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ClienteBlazorWASM.Helpers;
using Newtonsoft.Json;

namespace ALOGRepositorios.Services.Logisticos
{
    public class SLOTransporteDetalleService : ISLOTransporteDetalleService
    {
        private readonly HttpClient _httpClient;

        public SLOTransporteDetalleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SLOTransporteDetalle>> GetObtenerTransporteDetalle(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{Inicializar.UrlApiLogistico}SLOTorreControl/ListarTransporteDetalle/{id}");
                var jsonReaded = await response.Content.ReadAsStringAsync();
                var lstSlotransporteDetalle = JsonConvert.DeserializeObject<List<SLOTransporteDetalle>>(jsonReaded);
                return lstSlotransporteDetalle;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
