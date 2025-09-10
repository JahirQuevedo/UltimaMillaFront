using ALOG.Enums;

namespace ALOG.Modelos;

public partial class ArchivoBase : DocumentoBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ArchivoBase()
    {

    }

    #region Propiedades


    /// <summary>
    /// Id de la relacion
    /// </summary>
    public int IdRelacion { get; set; }


    public TipoProcesoArchivoControlDocumento TipoProcesoArchivoControlDocumento { get; set; }

    /// <summary>
    /// Ruta y Nombre de la fotografia LOGINTUD MÁXIMA [500]
    /// </summary>
    public string RutaNombreArchivo { get; set; }

    /// <summary>
    /// Rutabase del servidor generada en configuración de sistema.
    /// </summary>
    public string RutaBase { get; set; } = String.Empty;

    /// <summary>
    /// Extensión del archivo
    /// </summary>
    public string Extension { get; set; } = String.Empty;

    /// <summary>
    /// Ruta relativa que representa el subdirectorio principal del archivo a procesar.
    /// </summary>
    public string RutaRelativa { get; set; } = String.Empty;

    /// <summary>
    /// Corresponde al nombre de la carpeta final donde se almacenaran por registro los archivos. Este campo es obligatorio.
    /// Los valores pueden ser un folio, razon social, fecha, id, etc. Esta propiedad permite crear una ultima carpeta que anidadara todos los archivos.
    /// </summary>
    public string CarpetaIdentificacion { get; set; }

    /// <summary>
    /// Contenido del archivo en Base64
    /// </summary>
    public string ContenidoBase64 { get; set; }

    /// <summary>
    /// Indica si el archivo es una imagen
    /// </summary>
    public bool EsImagen { get; set; }

    /// <summary>
    /// Indica que si el archivo existe se reemplazara por uno nuevo.
    /// </summary>
    public bool SobreEscribirSiExiste { get; set; }

    #endregion


}
