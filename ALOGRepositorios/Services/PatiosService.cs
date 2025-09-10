using ALOG.Modelos.Modelos.Catalogos;
using ALOGRepositorios.Services.IServices;

namespace ALOGRepositorios.Services
{
    public class PatiosService : IPatiosService
    {
        public ICollection<CatPatios> GetPatios()
        {

            ICollection<CatPatios> patios = new List<CatPatios>();

            //patios = new List<CatPatios> {
            //    new CatPatios{ IdCatPatios = 1, RazonSocial = "PATIO 1", IdAduana = 1, IdProveedor = 1, Active = true, FechaRegistro = DateTime.Now, IdUsuarioRegistro = 1},
            //    new CatPatios{ IdCatPatios = 2, RazonSocial = "PATIO 2", IdAduana = 1, IdProveedor = 1, Active = true, FechaRegistro = DateTime.Now, IdUsuarioRegistro = 1},
            //    new CatPatios{ IdCatPatios = 3, RazonSocial = "PATIO 3", IdAduana = 1, IdProveedor = 1, Active = true, FechaRegistro = DateTime.Now, IdUsuarioRegistro = 1}
            //};

            return patios;
        }
    }
}
