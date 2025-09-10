namespace ALOG.Modelos;


  public class DocumentoBase : BaseEntity
  {

    public int Clave { get; set; }

    public string Archivo { get; set; }

    public string Ruta { get; set; }

    public string ContentType { get; set; }

    public byte[] Content { get; set; }
  }
