using System.Text.RegularExpressions;

namespace ALOGRepositorios.Helpers
{

    public class ResultadoValidacionRfc
    {
        public string Rfc { get; set; }
        public bool EsValido { get; set; }
        public bool EsPersonaFisica { get; set; }
        public bool EsPersonaMoral { get; set; }
        public bool EsExtranjero { get; set; }
        public List<string> MensajesError { get; set; }
    }

    public static class ValidateRfc
    {

        private static readonly Regex _regexRfc = new(@"^([A-ZÑ&]{3,4})(\d{6})([A-Z0-9]{3})$", RegexOptions.Compiled);

        private static readonly HashSet<string> _rfcInvalidos = new() {
            "XAXX010101000", // RFC genérico nacional
            "XEXX010101000"  // RFC genérico extranjero
         };

        private static readonly HashSet<string> _prefijosInvalidos = new() { "AAA", "AAB", "ABB", "ABA", "BBB", "ABC" };

        public static ResultadoValidacionRfc RfcIsValid(string rfc, bool validarHomoclave = false)
        {
            var resultado = new ResultadoValidacionRfc
            {
                Rfc = rfc,
                EsValido = false,
                MensajesError = new List<string>()
            };

            if (string.IsNullOrWhiteSpace(rfc))
            {
                resultado.MensajesError.Add("El RFC está vacío.");
                return resultado;
            }

            rfc = rfc.ToUpper();

            if (_rfcInvalidos.Contains(rfc))
            {
                resultado.MensajesError.Add("El RFC está en la lista negra del SAT.");
                return resultado;
            }

            var match = _regexRfc.Match(rfc);
            if (!match.Success)
            {
                resultado.MensajesError.Add("El formato del RFC no es válido.");
                return resultado;
            }

            string letras = match.Groups[1].Value;
            string fecha = match.Groups[2].Value;

            resultado.EsPersonaFisica = letras.Length == 4;
            resultado.EsPersonaMoral = letras.Length == 3;
            resultado.EsExtranjero = rfc.StartsWith("XEXX");

            if (resultado.EsPersonaFisica && _prefijosInvalidos.Contains(letras))
            {
                resultado.MensajesError.Add($"El prefijo '{letras}' es inválido para un RFC de persona física.");
                return resultado;
            }

            if (resultado.EsPersonaMoral && _prefijosInvalidos.Contains(letras))
            {
                resultado.MensajesError.Add($"El prefijo '{letras}' es inválido para un RFC de persona moral.");
                return resultado;
            }

            if (!FechaEsValida(fecha))
            {
                resultado.MensajesError.Add($"La fecha '{fecha}' dentro del RFC no es válida.");
                return resultado;
            }

            if (validarHomoclave && !HomoclaveEsValida(match.Groups[3].Value))
            {
                resultado.MensajesError.Add("La homoclave (últimos 3 caracteres) contiene caracteres inválidos.");
                return resultado;
            }

            resultado.EsValido = true;
            return resultado;
        }

        private static bool FechaEsValida(string fecha)
        {
            if (fecha.Length != 6) return false;

            var año = int.Parse(fecha.Substring(0, 2));
            var mes = int.Parse(fecha.Substring(2, 2));
            var dia = int.Parse(fecha.Substring(4, 2));

            // Lo intentamos con ambas centurias (1900 y 2000) para ser más flexibles
            foreach (int siglo in new[] { 1900, 2000 })
            {
                try
                {
                    var _ = new DateTime(siglo + año, mes, dia);
                    return true;
                }
                catch { continue; }
            }

            return false;
        }

        private static bool HomoclaveEsValida(string homoclave)
        {
            // Homoclave debe ser alfanumérica y tener 3 caracteres
            return homoclave.Length == 3 && homoclave.All(char.IsLetterOrDigit);
        }
    }
}
