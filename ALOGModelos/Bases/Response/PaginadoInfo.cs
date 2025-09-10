namespace ALOG.Modelos;

public class PaginadoInfo {

    public int NumeroDePagina { get; set; } = 1;

    public int RegistrosPorPagina { get; set; } = 10;

    public int TotalDePaginas { get; set; }

    public long TotalRegistros { get; set; }

}
