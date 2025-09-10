namespace ALOG.Modelos;

public class PaginadoResultJson : PaginadoResult
{

    public PaginadoResultJson(string dataJson, int pageNo, int pageSize, long totalRecordCount)
    {
        this.DataJson = dataJson;
        this.Paginado = new PaginadoInfo()
        {
        NumeroDePagina = pageNo,
        RegistrosPorPagina = pageSize,
        TotalRegistros = totalRecordCount,
        TotalDePaginas = totalRecordCount > 0L ? (int) Math.Ceiling((double) totalRecordCount / (double) pageSize) : 0
        };
    }

    public PaginadoResultJson(string dataJson, PaginadoInfo paginadoInfo)
    {
        this.DataJson = dataJson;
        this.Paginado = new PaginadoInfo()
        {
        NumeroDePagina = paginadoInfo.NumeroDePagina,
        RegistrosPorPagina = paginadoInfo.RegistrosPorPagina,
        TotalRegistros = paginadoInfo.TotalRegistros,
        TotalDePaginas = paginadoInfo.TotalRegistros > 0L ? (int) Math.Ceiling((double) paginadoInfo.TotalRegistros / (double) paginadoInfo.RegistrosPorPagina) : 0
        };
    }

    public PaginadoResultJson()
    {
    }

    public string DataJson { get; private set; }

    public PaginadoInfo Paginado { get; private set; }
}
