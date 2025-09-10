namespace ALOG.Modelos;

public class MonitorLineaOperador : BaseEntity
{
    public int IdLineaOperador { get; set; }

    public int IdCatTransportista { get; set; }

    public string RazonSocialLineaTransporte { get; set; }

    public string Nombre { get; set; }

    public string ApellidoPaterno { get; set; }

    public string ApellidoMaterno { get; set; }

    public string NombreCompleto { get {

        return Nombre + " " + ApellidoPaterno + " " + ApellidoMaterno;

    } }

}
