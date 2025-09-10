using System.Data;

namespace ALOG.Modelos;

public class MonitorTarjaInventario : BaseEntity
{

    public MonitorTarja Tarja { get; set; }

    public List<MonitorPartida> ListaPartidas { get; set; }

}
