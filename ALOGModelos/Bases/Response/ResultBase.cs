using System.Net;

namespace ALOG.Modelos;

public class ResultBase
{

    public bool Success
    {
      get
      {
        return string.IsNullOrWhiteSpace(this.MensajeRespuesta) || this.MensajeRespuesta.Equals("OK", StringComparison.InvariantCultureIgnoreCase);
      }
    }

    public int Id { get; set; }

    public string MensajeRespuesta { get; set; } = "OK";

    public DateTime FechaRespuesta { get; set; } = DateTime.Now;

    public HttpStatusCode ResponseCode { get; set; } = HttpStatusCode.OK;

}

  public class ResultBase<T> : ResultBase, IResultBase<T>
  {
    public T Data { get; set; }
  }
