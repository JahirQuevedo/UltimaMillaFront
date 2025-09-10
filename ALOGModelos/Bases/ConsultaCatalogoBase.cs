namespace ALOG.Modelos;

public class ConsultaCatalogoBase<T> : PaginadoInfo where T : BaseEntity
{
    public T Entidad { get; set; }
    
}

