namespace ALOG.Modelos;

public class ReporteTarjaRecepcion : BaseEntity
{

    public string FolioTarja { get; set; }

    public string NombreCliente { get; set; }

    public string FolioViaje { get; set;}

    public string Buque { get; set; }   


    public string PatioRecepcion { get; set; }

    public string Fecha { get; set; }

    public List<ReporteDetalleTarjaRecepcion> ListaReporteDetalleTarjaRecepcion { get; set; }



}
