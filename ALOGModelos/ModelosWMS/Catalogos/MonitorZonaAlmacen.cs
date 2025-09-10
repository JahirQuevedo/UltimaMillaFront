namespace ALOG.Modelos;

public class MonitorZonaAlmacen : BaseEntity
{

    public int IdZonaAlmacenaje { get; set; }

    public string Clave { get; set; }

    public string Descripcion { get; set; }

    public int TotalFilas { get; set; }

    public int TotalColumnas { get; set; }


}
