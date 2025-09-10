using System.Collections.Generic;
using System.Net;

namespace ALOG.Modelos;

public class PaginadoResult : ResultBase
{

}

public class PaginadoResult<T> : PaginadoResult
{
public PaginadoResult(IEnumerable<T> items, int pageNo, int pageSize, long totalRecordCount)
{
    this.Data = new List<T>(items);
    this.Paginado = new PaginadoInfo()
    {
    NumeroDePagina = pageNo,
    RegistrosPorPagina = pageSize,
    TotalRegistros = totalRecordCount,
    TotalDePaginas = totalRecordCount > 0L ? (int) Math.Ceiling((double) totalRecordCount / (double) pageSize) : 0
    };
}

public PaginadoResult(IEnumerable<T> items, PaginadoInfo paginadoInfo)
{
    this.Data = new List<T>(items);
    this.Paginado = new PaginadoInfo()
    {
    NumeroDePagina = paginadoInfo.NumeroDePagina,
    RegistrosPorPagina = paginadoInfo.RegistrosPorPagina,
    TotalRegistros = paginadoInfo.TotalRegistros,
    TotalDePaginas = paginadoInfo.TotalRegistros > 0L ? (int) Math.Ceiling((double) paginadoInfo.TotalRegistros / (double) paginadoInfo.RegistrosPorPagina) : 0
    };
}

public PaginadoResult()
{
}

public List<T> Data { get; set; }

public T Registro { get; set; }

public PaginadoInfo Paginado { get; set; }

}
