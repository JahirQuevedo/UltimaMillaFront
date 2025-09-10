using ALOGRespositorios.Modelos.Dtos.Control;

namespace ALOGRepositorios.Helpers
{
    public class ExcelService
    {

        /*
          5 MB =  5 * 1024 * 1024
         10 MB = 10 * 1024 * 1024
         20 MG = 20 * 1024 * 1024
         */
        public static readonly long TAMANIO_PERMITIDO = (10 * 1024 * 1024);

        public static readonly int PLANTILLA_MANIOBRAS_VACIOS = 1;
        public static readonly int PLANTILLA_GESTION_EIR = 2;

        public static readonly string NUMERO_BL = "número de bl";
        //public static readonly string RFC_NAVIERA = "rfc naviera";
        public static readonly string NAVIERA_RAZON_SOCIAL = "razón social naviera";
        public static readonly string NOMBRE_BUQUE = "nombre del buque";
        public static readonly string NUMERO_BUQUE = "número del buque";
        public static readonly string NUMERO_CONTENEDOR = "número del contenedor";
        public static readonly string TIPO_CONTENEDOR = "tipo de contenedor";
        public static readonly string TRANSPORTISTA_CORREO = "correo línea transportista";
        public static readonly string TRANSPORTISTA_RFC = "rfc línea transportista";
        public static readonly string TRANSPORTISTA_RAZON_SOCIAL = "razón social línea transportista";
        public static readonly string TRANSPORTISTA_NOMBRE_CONTACTO = "nombre de contacto transportista";
        public static readonly string RFC_CLIENTE_SOLICITANTE = "rfc cliente servicio";
        public static readonly string RFC_CLIENTE_FACTURAR = "rfc cliente facturar";

        public static readonly string REFERENCIA_CLIENTE = "referencia del cliente";
        public static readonly string REFERENCIA_CLIENTE_FACTURAR = "referencia del cliente a facturar";
        public static readonly string ID_PATIO = "id patio";
        public static readonly string PATIO_RAZON_SOCIAL = "patio";
        public static readonly string REFERENCIA_PAGO_PATIO = "referencia pago patio";
        public static readonly string TICKET = "ticket";
        public static readonly string NOMBRE_EJECUTIVO_SOLICITANTE = "ejecutivo solicitante";

        public static readonly string RFC_PATIO = "rfc patio";
        public static readonly string FOLIO_MANIOBRA = "folio maniobra";

        //public static Dictionary<string, int> GetIndices(int idCatServicio, UsuarioTokenDTO usuarioToken) {
        //    Dictionary<string, int> indices = new();

        //    if (idCatServicio == 1) {
        //        indices[NUMERO_CONTENEDOR] = 1;
        //        indices[NUMERO_BL] = 2;
        //        indices[RFC_NAVIERA] = 3;
        //        indices[NOMBRE_BUQUE] = 4;
        //        indices[NUMERO_BUQUE] = 5;
        //        indices[TIPO_CONTENEDOR] = 6;

        //        indices[TRANSPORTISTA_CORREO] = 7;
        //        indices[TRANSPORTISTA_RFC] = 8;
        //        indices[TRANSPORTISTA_RAZON_SOCIAL] = 9;
        //        indices[TRANSPORTISTA_NOMBRE_CONTACTO] = 10;

        //        indices[RFC_CLIENTE_SOLICITANTE] = 11;
        //        indices[RFC_CLIENTE_FACTURAR] = 12;

        //        if ((usuarioToken.Rol is "ADMIN" or "ADMINUSER") || usuarioToken.IdCatUsuario == 26) {
        //            indices[REFERENCIA_CLIENTE] = 13;
        //            indices[REFERENCIA_CLIENTE_FACTURAR] = 14;
        //            indices[ID_PATIO] = 15;
        //            indices[PATIO_RAZON_SOCIAL] = 16;
        //            indices[REFERENCIA_PAGO_PATIO] = 17;
        //            indices[TICKET] = 18;
        //        }
        //    } else if (idCatServicio == 2) {
        //        indices[NUMERO_CONTENEDOR] = 1;
        //        indices[TIPO_CONTENEDOR] = 2;
        //        indices[RFC_PATIO] = 3;
        //        indices[FOLIO_MANIOBRA] = 4;
        //        indices[RFC_CLIENTE_SOLICITANTE] = 5;
        //        indices[RFC_CLIENTE_FACTURAR] = 6;

        //        if ((usuarioToken.Rol is "ADMIN" or "ADMINUSER") || usuarioToken.IdCatUsuario == 26) {
        //            indices[REFERENCIA_CLIENTE] = 7;
        //            indices[REFERENCIA_CLIENTE_FACTURAR] = 8;
        //            indices[TICKET] = 9;
        //        }
        //    }

        //    return indices;
        //}

        /// <summary>
        /// Genera un diccionario [cabecera → número de columna] a partir de la primera fila leída.
        /// </summary>
        public static Dictionary<string, int> MapearIndicesPorCabecera(IEnumerable<string> cabecerasPlantilla)
        {
            return cabecerasPlantilla
                .Select((texto, idx) => new { texto = texto.Trim().ToLower(), columna = idx + 1 })
                .ToDictionary(x => x.texto, x => x.columna);
        }

        public static List<string> GetCabecerasExcelSolicitudManiobras(UsuarioTokenDTO usuarioToken)
        {
            var columnas = new List<string>
            {
                 NUMERO_CONTENEDOR,
                 NUMERO_BL,
                 //RFC_NAVIERA,
                 NOMBRE_BUQUE,
                 NUMERO_BUQUE,
                 TIPO_CONTENEDOR,
                 TRANSPORTISTA_CORREO,
                 TRANSPORTISTA_RFC,
                 TRANSPORTISTA_NOMBRE_CONTACTO,
                 RFC_CLIENTE_SOLICITANTE,
                 RFC_CLIENTE_FACTURAR,
                 NOMBRE_EJECUTIVO_SOLICITANTE
            };

            if ((usuarioToken.Rol == "ADMIN" || usuarioToken.Rol == "ADMINUSER"))
            {
                columnas.AddRange(new[]
                {
                    TRANSPORTISTA_RAZON_SOCIAL,
                    NAVIERA_RAZON_SOCIAL,
                    REFERENCIA_CLIENTE,
                    REFERENCIA_CLIENTE_FACTURAR,
                    ID_PATIO,
                    PATIO_RAZON_SOCIAL,
                    REFERENCIA_PAGO_PATIO,
                    TICKET
                });
            }
            else if (usuarioToken.Rol.Equals("CLIENTE") && usuarioToken.Email.Contains("nad", StringComparison.InvariantCultureIgnoreCase))
            {
                columnas.AddRange(new[]
                {
                    TRANSPORTISTA_RAZON_SOCIAL,
                    NAVIERA_RAZON_SOCIAL,
                    REFERENCIA_CLIENTE,
                    REFERENCIA_CLIENTE_FACTURAR,
                    TICKET
                });
            }

            return columnas;
        }

        public static List<string> GetCabecerasExcelSolicitudEir(UsuarioTokenDTO usuarioToken)
        {

            List<string> columnas = new List<string>();

            columnas = new List<string> {
                        NUMERO_CONTENEDOR,
                        TIPO_CONTENEDOR,
                        RFC_PATIO,
                        FOLIO_MANIOBRA,
                        RFC_CLIENTE_SOLICITANTE,
                        RFC_CLIENTE_FACTURAR,
                        NOMBRE_EJECUTIVO_SOLICITANTE

            };

            if ((usuarioToken.Rol is "ADMIN" or "ADMINUSER") || (usuarioToken.Rol is "CLIENTE" && usuarioToken.IdCatUsuario == 26))
            {
                columnas.AddRange(new[]
                {
                    REFERENCIA_CLIENTE,
                    REFERENCIA_CLIENTE_FACTURAR,
                    TICKET
                });
            }

            return columnas;
        }

        public static List<string> ValidarEncabezadosExcel(int tipoPlantilla, UsuarioTokenDTO usuarioToken, List<string> cabecerasPlantilla)
        {

            List<string> errores = new List<string>();
            bool encabezadosValidos = true;

            try
            {
                errores ??= new List<string>();
                errores.Clear();
                List<string> cabecerasEsperadas = new List<string>();

                cabecerasEsperadas = tipoPlantilla switch
                {
                    1 => GetCabecerasExcelSolicitudManiobras(usuarioToken),
                    2 => GetCabecerasExcelSolicitudEir(usuarioToken),
                    _ => new List<string>()
                };

                string solicitudServicio = tipoPlantilla == 1 ? "Maniobras de vacios" : "Gestión de E.I.R.";

                if (cabecerasPlantilla.Count() < cabecerasEsperadas.Count())
                {
                    errores.Add($"La plantilla utilizada para generar una solicitud de servicio de {solicitudServicio} es incorrecta.");
                    return errores;
                }

                //for (int col = 1; col <= cabecerasEsperadas.Count(); col++)
                //{
                //    //string encabezado = _sheet.Cells[1, col].Text?.ToLower()?.Trim() ?? "";
                //    string encabezado = cabecerasPlantilla[col - 1].ToLower()?.Trim() ?? "";
                //    string esperado = cabecerasEsperadas[col - 1];

                //    if (!string.Equals(encabezado, esperado, StringComparison.OrdinalIgnoreCase))
                //    {
                //        encabezadosValidos = false;
                //        break;
                //    }
                //}

                //foreach (var encabezado in cabecerasPlantilla)
                //{
                //    if (!cabecerasEsperadas.Any(e => string.Equals(e, encabezado, StringComparison.OrdinalIgnoreCase)))
                //    {
                //        encabezadosValidos = false;
                //        break;
                //    }
                //}

                encabezadosValidos = cabecerasPlantilla.All(enc => cabecerasEsperadas.Any(exp => string.Equals(exp, enc, StringComparison.OrdinalIgnoreCase)));

                if (!encabezadosValidos)
                {
                    errores.Add($"La plantilla utilizada para generar una solicitud de servicio de {solicitudServicio} es incorrecta.");
                }
            }
            catch (Exception ex)
            {
                errores.Add($"Error inesperado al procesar el archivo {ex.Message}");
            }

            return errores;
        }
    }
}
