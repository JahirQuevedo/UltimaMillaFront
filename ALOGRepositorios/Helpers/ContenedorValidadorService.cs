
using ALOG.Modelos.Modelos.DTO.Vacios;
using System.Text.RegularExpressions;

namespace ALOGRepositorios.Helpers
{
    public class ContenedorValidadorService
    {

        public List<PeticionesContenedoresClienteExternoDTO> Contenedores { get; set; }
        public List<string> Errores { get; set; }

        public List<string> ContenedorEsValido(PeticionesContenedoresClienteExternoDTO contenedor, int tipoServicio)
        {

            PeticionesServiciosClienteExternoDTO servicio = contenedor.Servicios.FirstOrDefault();

            if (Errores == null)
            {
                Errores = new List<string>();
            }

            //Errores.Clear();

            //bool comparacion = Contenedores.Any(c => c.Contenedor.Equals(contenedor.Contenedor));
            var regex = new Regex(@"^[A-Z]{4}\d{7}$");

            #region Validaciones generales
            if (string.IsNullOrEmpty(contenedor.Contenedor))
            {
                Errores.Add("El número de contenedor no puede estar vacío");
            }
            else
            {
                if (!regex.IsMatch(contenedor.Contenedor))
                {
                    Errores.Add($"El contenedor {contenedor.Contenedor}  no es un contenedor valido.");
                }

                //string valorOriginal = _contenedorOriginal.TryGetValue(contenedor, out var original) ? original : null;
                //bool fueModificado = !string.Equals(valorOriginal, contenedor.Contenedor, StringComparison.OrdinalIgnoreCase);

                //if (fueModificado) {
                //    foreach (var c in Contenedores) {
                //        if (c != contenedor && c.Contenedor.Equals(contenedor.Contenedor.ToUpper().Trim())) {
                //            Errores.Add($"El contenedor {contenedor.Contenedor} ya se encuentra en la solicitud");
                //            break;
                //        }
                //    }
                //}
            }

            if (contenedor.IdCatTipoContenedor <= 0)
            {
                Errores.Add("El tipo de contenedor es requerido");
            }
            else
            {
                //if (!TipoContenedorEsValido(contenedor.IdCatTipoContenedor)) {
                //    Errores.Add($"El tipo de contenedor {contenedor.ClaveTipoContenedor} no tiene un formato válido");
                //}
            }

            if (string.IsNullOrEmpty(contenedor.ClienteRFC) && string.IsNullOrEmpty(servicio.RFCFacturar))
            {
                Errores.Add($"Debes indicar un RFC de cliente servico o RFC de cliente a facturar para el contenedor {contenedor.Contenedor}");
            }
            // Aquí se valida el RFC del cliente solicitante
            if (!string.IsNullOrEmpty(contenedor.ClienteRFC))
            {

                if (!ValidateRfc.RfcIsValid(contenedor.ClienteRFC).EsValido)
                {
                    Errores.Add("El RFC del cliente servicio no es válido");
                }
                else
                {
                    if (string.IsNullOrEmpty(servicio.RFCFacturar))
                    {
                        servicio.RFCFacturar = contenedor.ClienteRFC;
                    }
                }
            }

            if (!string.IsNullOrEmpty(servicio.RFCFacturar))
            {
                if (!ValidateRfc.RfcIsValid(servicio.RFCFacturar).EsValido)
                {
                    Errores.Add("El RFC del cliente a facturar no es válido");
                }
                else
                {
                    if (string.IsNullOrEmpty(contenedor.ClienteRFC))
                    {
                        contenedor.ClienteRFC = servicio.RFCFacturar;
                    }
                }
            }
            #endregion Validaciones generales

            #region Validaciones servicio de maniobras de vacíos
            if (tipoServicio == 0)
            {
                if (string.IsNullOrEmpty(servicio.NumeroBL))
                {
                    Errores.Add("El número de BL es requerido");
                }

                //if (string.IsNullOrEmpty(servicio.NavieraRFC)) {
                //    Errores.Add("El RFC de la naviera es requerido");
                //} else {
                //    if (!ValidateRfc.RfcIsValid(servicio.NavieraRFC).EsValido) {
                //        Errores.Add("El RFC de la naviera no es válido");
                //    }
                //}

                if (string.IsNullOrEmpty(servicio.NumeroViaje))
                {
                    Errores.Add("El número del viaje es requerido");
                }

                if (string.IsNullOrEmpty(servicio.NombreBuque))
                {
                    Errores.Add("El nombre del buque es requerido");
                }

                if (string.IsNullOrEmpty(servicio.TransporteRFC))
                {
                    Errores.Add("El RFC del transportista es requerido");
                }
                else
                {
                    if (!ValidateRfc.RfcIsValid(servicio.TransporteRFC).EsValido)
                    {
                        Errores.Add("El RFC del transportista no es válido");
                    }
                }

                if (string.IsNullOrEmpty(servicio.TransporteEmailContacto))
                {
                    Errores.Add("El email del transportista es requerido");
                }

                if (string.IsNullOrEmpty(servicio.TransporteRazonSocial))
                {
                    Errores.Add("La razón social del transportista es requerida");
                }

            }
            #endregion Validaciones servicio de maniobras de vacíos

            #region Validaciones servicio de eir

            if (tipoServicio == 1)
            {
                if (string.IsNullOrEmpty(servicio.PatioRFC))
                {
                    Errores.Add("El RFC del patio es requerido");
                }
                else
                {
                    if (!ValidateRfc.RfcIsValid(servicio.PatioRFC).EsValido)
                    {
                        Errores.Add("El RFC del patio no es válido");
                    }
                }
            }
            #endregion Validaciones servicio de eir
            return Errores;
        }

        private bool TipoContenedorEsValido(string tipoContenedor)
        {

            if (string.IsNullOrWhiteSpace(tipoContenedor)) return false;

            tipoContenedor = tipoContenedor.ToUpper().Trim();

            // Verifica que coincida con el patrón general
            if (!Regex.IsMatch(tipoContenedor, @"^[A-Z]{2}\d{2}$"))
                return false;

            // Lista de prefijos no permitidos
            var prefijosInvalidos = new HashSet<string> { "XX", "ZZ", "AA" };

            string prefijo = tipoContenedor.Substring(0, 2);
            if (prefijosInvalidos.Contains(prefijo))
                return false;

            return true;
        }
    }
}
