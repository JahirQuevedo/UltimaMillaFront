using ALOG.Modelos.Modelos.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOGRepositorios.Services.Catalogos.ICatalogos
{
    public interface ICatDocumentosLNegocioService
    {
        Task<List<CatDocumentos>> ListarDocumentosLNegocio(int IdLineaNegocio);
    }
}
