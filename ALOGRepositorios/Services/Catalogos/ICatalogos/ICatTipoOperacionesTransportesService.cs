using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatTipoOperacionesTransportesService
    {
        Task<List<CatTipoOperacionesTransportes>> GetTiposOperacionesTransportes();
    }
}
