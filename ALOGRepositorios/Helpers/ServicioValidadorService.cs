using ALOG.Modelos.Modelos.DTO.Vacios;

namespace ALOGRepositorios.Helpers
{
    public class ServicioValidadorService
    {

        public List<string> Errores { get; set; }
        public List<int> IdsServiciosContenedor { get; set; }
        public Dictionary<string, string> Servicio { get; set; }

        public void ValidarReglaNegocio(PeticionesServiciosClienteExternoDTO servicio)
        {

        }

        public void InformacionComplementarioEsValida(PeticionesServiciosClienteExternoDTO servicio)
        {
            #region Validaciones servicio de maniobras de vacíos
            if (servicio.IdCatServicio == 1)
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

            if (servicio.IdCatServicio == 2)
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
        }
    }
}
