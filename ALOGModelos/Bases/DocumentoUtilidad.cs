using System.Security;
using ALOG.Enums;

namespace ALOG.Modelos;

public class DocumentoUtilidad
{

    public (ResultBase resultBase, string rutaCompleta, string rutaSinRutaBase) CrearArchivo(
      string nombreArchivo,
      byte[] content,
      string rutaBase,
      string rutaRelativa,
      TipoSubDirectorioGeneral tipoSubDirectorioGeneral = TipoSubDirectorioGeneral.IncluirAnioMesDia,
      bool sobreEscribirSiExiste = true)
    {
        ResultBase resultBase = new ResultBase();
        string str1 = (string) null;
        string str2 = (string) null;
        try
        {
            string empty = string.Empty;
            if (content == null)
            resultBase.MensajeRespuesta = "No se envia el contenido del archivo";
            if (string.IsNullOrWhiteSpace(rutaBase))
            resultBase.MensajeRespuesta = "Falta la información de la ruta del directorio root.";
            if (string.IsNullOrWhiteSpace(nombreArchivo))
            resultBase.MensajeRespuesta = "Falta nombre de archivo";
            if (resultBase.Success)
            {
            foreach (char invalidPathChar in Path.GetInvalidPathChars())
                rutaBase = rutaBase.Replace(invalidPathChar.ToString(), "");
            // if (!rutaBase.EndsWith("\\"))
            //   rutaBase += "\\";
            foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
                nombreArchivo = nombreArchivo.Replace(invalidFileNameChar.ToString(), "");
            DirectoryInfo directoryInfo = new DirectoryInfo(rutaBase);
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            if (!string.IsNullOrWhiteSpace(rutaRelativa))
            {
                if (rutaRelativa.EndsWith("\\"))
                rutaRelativa = rutaRelativa.Substring(1, rutaRelativa.Length - 1);
                int index;
                switch (tipoSubDirectorioGeneral)
                {
                case TipoSubDirectorioGeneral.IncluirAnioMesDia:
                    string[] strArray1 = new string[8]
                    {
                    rutaRelativa,
                    "/",
                    DateTime.Now.Year.ToString(),
                    "/",
                    null,
                    null,
                    null,
                    null
                    };
                    index = DateTime.Now.Month;
                    strArray1[4] = index.ToString();
                    strArray1[5] = "/";
                    index = DateTime.Now.Day;
                    strArray1[6] = index.ToString();
                    strArray1[7] = "/";
                    rutaRelativa = string.Concat(strArray1);
                    break;
                case TipoSubDirectorioGeneral.IncluirAnioMes:
                    string[] strArray2 = new string[6]
                    {
                    rutaRelativa,
                    "/",
                    DateTime.Now.Year.ToString(),
                    "/",
                    null,
                    null
                    };
                    index = DateTime.Now.Month;
                    strArray2[4] = index.ToString();
                    strArray2[5] = "/";
                    rutaRelativa = string.Concat(strArray2);
                    break;
                case TipoSubDirectorioGeneral.IncluirAnio:
                    string str3 = rutaRelativa;
                    index = DateTime.Now.Year;
                    string str4 = index.ToString();
                    rutaRelativa = str3 + "/" + str4 + "/";
                    break;
                default:

                    break;
                }
                char[] invalidPathChars = Path.GetInvalidPathChars();
                for (index = 0; index < invalidPathChars.Length; ++index)
                {
                char ch = invalidPathChars[index];
                rutaRelativa = rutaRelativa.Replace(ch.ToString(), "");
                }
                string fileFullPath = directoryInfo.FullName + rutaRelativa;
                directoryInfo = new DirectoryInfo(fileFullPath);
            }

            if (!directoryInfo.Exists)
                directoryInfo.Create();
            
            string filePath = directoryInfo.FullName + nombreArchivo;

            FileInfo fileInfo = new FileInfo(filePath);
            bool flag = true;

            if (System.IO.File.Exists(fileInfo.FullName))
            {
                flag = false;
                if (sobreEscribirSiExiste)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                using (FileStream fileStream = System.IO.File.Create(fileInfo.FullName))
                fileStream.Write(content, 0, content.Length);
            }
            str1 = fileInfo.FullName;
            str2 = fileInfo.FullName.Replace(rutaBase, string.Empty);
            }
        }
        catch (DirectoryNotFoundException ex)
        {
            resultBase = new ResultBase()
            {
                MensajeRespuesta = "Ruta de directorio no existe."
            };
        }
        catch (PathTooLongException ex)
        {
            resultBase = new ResultBase()
            {
            MensajeRespuesta = "Ruta configurada demasiado larga."
            };
        }
        catch (UnauthorizedAccessException ex)
        {
            resultBase = new ResultBase()
            {
            MensajeRespuesta = "No tiene acceso a la ruta."
            };
        }
        catch (IOException ex)
        {
            resultBase = new ResultBase()
            {
            MensajeRespuesta = "Se produjo un error de E/S al crear el archivo."
            };
        }
        catch (Exception ex)
        {
            new ResultBase<string>().MensajeRespuesta = ex.Message;
            throw;
        }
        return (resultBase, str1, str2);
    }

    public ResultBase EliminarArchivo(string ruta)
    {
      ResultBase resultBase = new ResultBase();
      try
      {
        if (File.Exists(ruta))
          File.Delete(ruta);
        else
          resultBase.MensajeRespuesta = "Ruta no existe. No se puede eliminar archivo";
      }
      catch (DirectoryNotFoundException ex)
      {
        
        resultBase = new ResultBase()
        {
          MensajeRespuesta = "No se puede eliminar: La ruta de acceso especificada no es válida."
        };
      }
      catch (PathTooLongException ex)
      {
        
        resultBase = new ResultBase()
        {
          MensajeRespuesta = "No se puede eliminar: La ruta de acceso especificada, el nombre de archivo o ambos superan la longitud máxima definida por el sistema."
        };
      }
      catch (UnauthorizedAccessException ex)
      {
        
        resultBase = new ResultBase()
        {
          MensajeRespuesta = "No se puede eliminar: No tiene acceso a la ruta."
        };
      }
      catch (IOException ex)
      {
        
        resultBase = new ResultBase()
        {
          MensajeRespuesta = "No se puede eliminar: El archivo especificado está en uso."
        };
      }
      catch (NotSupportedException ex)
      {
        
        resultBase = new ResultBase()
        {
          MensajeRespuesta = "No se puede eliminar: Path está en un formato no válido."
        };
      }
      catch (Exception ex)
      {
        
        new ResultBase().MensajeRespuesta = ex.Message;
        throw;
      }

      return resultBase;

    }

    public ResultBase<byte[]> LeerArchivo(string ruta)
    {
      ResultBase<byte[]> resultBase1 = new ResultBase<byte[]>();
      try
      {
        if (string.IsNullOrWhiteSpace(ruta))
          throw new ArgumentException("Ruta sin datos", nameof (ruta));
        byte[] numArray = (byte[]) null;
        if (new FileInfo(ruta).Exists)
        {
          using (StreamReader streamReader = new StreamReader(ruta))
          {
            using (BinaryReader binaryReader = new BinaryReader(streamReader.BaseStream))
            {
              numArray = binaryReader.ReadBytes(Convert.ToInt32(binaryReader.BaseStream.Length));
              binaryReader.Dispose();
            }
            streamReader.Dispose();
          }
        }
        else
        {
          resultBase1.MensajeRespuesta = "Archivo no existe.";
        }
        resultBase1.Data = numArray;
      }
      catch (DirectoryNotFoundException ex)
      {
        ResultBase<byte[]> resultBase2 = new ResultBase<byte[]>();
        resultBase2.MensajeRespuesta = "Ruta de directorio no existe.";
        resultBase1 = resultBase2;
      }
      catch (PathTooLongException ex)
      {
        ResultBase<byte[]> resultBase3 = new ResultBase<byte[]>();
        resultBase3.MensajeRespuesta = "Ruta configurada demasiado larga.";
        resultBase1 = resultBase3;
      }
      catch (UnauthorizedAccessException ex)
      {
        ResultBase<byte[]> resultBase4 = new ResultBase<byte[]>();
        resultBase4.MensajeRespuesta = "No tiene acceso a la ruta.";
        resultBase1 = resultBase4;
      }
      catch (SecurityException ex)
      {
        ResultBase<byte[]> resultBase5 = new ResultBase<byte[]>();
        resultBase5.MensajeRespuesta = "No dispone del permiso requerido..";
        resultBase1 = resultBase5;
      }
      catch (IOException ex)
      {;
        ResultBase<byte[]> resultBase6 = new ResultBase<byte[]>();
        resultBase6.MensajeRespuesta = "Se produjo un error de E/S al crear el archivo.";
        resultBase1 = resultBase6;
      }
      catch (NotSupportedException ex)
      {
        ResultBase<byte[]> resultBase7 = new ResultBase<byte[]>();
        resultBase7.MensajeRespuesta = "Nombre de archivo contiene un carácter de dos puntos (:) dentro de la cadena.";
        resultBase1 = resultBase7;
      }
      catch (Exception ex)
      {
        new ResultBase<byte[]>().MensajeRespuesta = ex.Message;
        throw;
      }
      return resultBase1;
    }

    /// <summary>Leer un archivo y convertirlo a base64</summary>
    /// <param name="ruta">ruta del archivo</param>
    /// <returns>ResultBase con información en Data con archivo en Base64. O Mensaje de respuesta con error.</returns>
    public ResultBase<string> LeerArchivoToBase64(string ruta)
    {
      try
      {
        ResultBase<string> base64 = new ResultBase<string>();
        ResultBase<byte[]> resultBase = this.LeerArchivo(ruta);
        if (resultBase?.Data != null)
          base64.Data = Convert.ToBase64String(resultBase?.Data);
        else
          base64.MensajeRespuesta = resultBase?.MensajeRespuesta ?? "Error al leer archivo";
        return base64;
      }
      catch
      {
        throw;
      }
    }

}
