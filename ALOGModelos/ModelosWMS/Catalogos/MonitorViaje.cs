using ALOG.Enums;

namespace ALOG.Modelos;

public class MonitorViaje : BaseEntity
{
    public int IdViaje {get; set;}

    public int ReferenciaBuque {get; set;}

    public string FolioViaje {get; set;}

    public MonitorBarco Barco {get; set;}

    public string LineaNaviera {get; set;}

    public TipoOperacionAduanera TipoOperacion {get; set;}

    public string Operacion {get; set;}

    public bool Exterior {get; set;}

    public DateTime FechaArriboSalida {get; set;}

    public DateTime FechaFondeo {get; set;}

    public DateTime FechaAtraque {get; set;}    

    public DateTime FechaDesatraque {get; set;}

    public DateTime FechaInicioCargaDescarga {get; set;}

    public DateTime FechaFinCargaDescarga {get;set;}

    public decimal PesoBl {get; set;}
}
