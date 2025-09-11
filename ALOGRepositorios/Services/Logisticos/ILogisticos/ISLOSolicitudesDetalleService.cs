using ALOG.Modelos.Modelos.Logisticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOGRepositorios.Services.Logisticos.ILogisticos
{
    public interface ISLOSolicitudesDetalleService
    {
        Task<List<SLOSolicitudesDetalle>> SLOSolicitudesDetalleObtener(int id);
    }
}
