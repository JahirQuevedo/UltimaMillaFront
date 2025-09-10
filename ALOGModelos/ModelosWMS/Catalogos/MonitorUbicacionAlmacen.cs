namespace ALOG.Modelos;

public class MonitorUbicacionAlmacen : BaseEntity
{

    public int IdUbicacionAlmacen { get; set; }
    
    public string Clave { get; set; }

    public string Descripcion { get; set; }

    public int IdInventario { get; set; }   

    public int IdZonaAlmacenaje { get; set; }

    public string ClaveZona { get; set; }

    public string DescripcionZona { get; set; }

    public bool Disponible { get; set; }

}

