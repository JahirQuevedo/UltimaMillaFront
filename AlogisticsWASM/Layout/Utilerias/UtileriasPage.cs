using ALOG.Modelos.Modelos.Catalogos;
using Radzen;

namespace AlogisticsWASM.Layout.Utilerias
{
    public class UtileriasPage
    {
        private CatReferenciaEstado objEstadosReferencia;
        private CatTipoEstados objTipoEstados;

        private List<CatReferenciaEstado> lstobjEstadosReferencia = new List<CatReferenciaEstado>();
        private List<CatTipoEstados> lstobjTipoEstados = new List<CatTipoEstados>();

        public enum ReferenciaEstado
        {
            ABIERTO = 1,
            ENPROCESO = 4,
            TERMINADO = 5,
            CANCELADO = 6,
            PENDIENTE = 7

        }
        public enum TiposEstados
        {
            ABIERTO = 1,
            ENPROCESO = 2,
            TERMINADO = 3,
            CANCELADO = 4,
            PENDIENTE = 5
        }

        public List<CatReferenciaEstado> ObtenerEstadosReferencia()

        {
            lstobjEstadosReferencia = new List<CatReferenciaEstado>();
            objEstadosReferencia = new CatReferenciaEstado();
            objEstadosReferencia.IdCatReferenciaEstado = 1;
            objEstadosReferencia.Nombre = "ABIERTO";
            lstobjEstadosReferencia.Add(objEstadosReferencia);
            objEstadosReferencia = new CatReferenciaEstado();
            objEstadosReferencia.IdCatReferenciaEstado = 4;
            objEstadosReferencia.Nombre = "EN PROCESO";
            lstobjEstadosReferencia.Add(objEstadosReferencia);
            objEstadosReferencia = new CatReferenciaEstado();
            objEstadosReferencia.IdCatReferenciaEstado = 5;
            objEstadosReferencia.Nombre = "TERMINADO";
            lstobjEstadosReferencia.Add(objEstadosReferencia);
            objEstadosReferencia = new CatReferenciaEstado();
            objEstadosReferencia.IdCatReferenciaEstado = 6;
            objEstadosReferencia.Nombre = "CANCELADO";
            lstobjEstadosReferencia.Add(objEstadosReferencia);
            objEstadosReferencia = new CatReferenciaEstado();
            objEstadosReferencia.IdCatReferenciaEstado = 7;
            objEstadosReferencia.Nombre = "PENDIENTE";
            lstobjEstadosReferencia.Add(objEstadosReferencia);

            return lstobjEstadosReferencia;
        }

        public List<CatTipoEstados> ObtenerTipoEstados()
        {
            lstobjTipoEstados = new List<CatTipoEstados>();

            objTipoEstados = new CatTipoEstados();
            objTipoEstados.IdCatTipoEstados = 1;
            objTipoEstados.Nombre = "ABIERTO";
            lstobjTipoEstados.Add(objTipoEstados);
            objTipoEstados = new CatTipoEstados();
            objTipoEstados.IdCatTipoEstados = 2;
            objTipoEstados.Nombre = "EN PROCESO";
            lstobjTipoEstados.Add(objTipoEstados);
            objTipoEstados = new CatTipoEstados();
            objTipoEstados.IdCatTipoEstados = 3;
            objTipoEstados.Nombre = "TERMINADO";
            lstobjTipoEstados.Add(objTipoEstados);
            objTipoEstados = new CatTipoEstados();
            objTipoEstados.IdCatTipoEstados = 4;
            objTipoEstados.Nombre = "CANCELADO";
            lstobjTipoEstados.Add(objTipoEstados);
            objTipoEstados = new CatTipoEstados();
            objTipoEstados.IdCatTipoEstados = 5;
            objTipoEstados.Nombre = "PENDIENTE";
            lstobjTipoEstados.Add(objTipoEstados);

            return lstobjTipoEstados;
        }
        //public enum TipoEstados
        //{
        //    ABIERTO,
        //    PROCESO,
        //    TERMINADO,
        //    CANCELADO,
        //    PENDIENTE
        //}
        public BadgeStyle GetBadgeStyle(string tipoEstado)
        {
            tipoEstado = tipoEstado.ToUpper();

            return tipoEstado switch
            {

                "ABIERTO" => BadgeStyle.Dark,
                "EN PROCESO" => BadgeStyle.Warning,
                "TERMINADO" => BadgeStyle.Success,
                "CANCELADO" => BadgeStyle.Danger,
                "PENDIENTE" => BadgeStyle.Dark,
                _ => BadgeStyle.Danger
            };
        }

        public BadgeStyle GetBadgeStyle2(string tipoEstado)
        {
            if (!int.TryParse(tipoEstado, out int idTipoEstado))
            {
                //Console.WriteLine($"Advertencia: No se pudo convertir '{tipoEstado}' a int. Asignando -1 por defecto.");
                idTipoEstado = -1;
            }

            //Console.WriteLine($"idTipoEstado {idTipoEstado}");

            return idTipoEstado switch
            {
                1 => BadgeStyle.Dark,
                4 => BadgeStyle.Warning,
                5 => BadgeStyle.Success,
                6 => BadgeStyle.Danger,
                7 => BadgeStyle.Dark,
                0 => BadgeStyle.Dark,
                _ => BadgeStyle.Dark
            };
        }

        //public BadgeStyle GetBadgeStyle2(string tipoEstado)
        //{
        //    tipoEstado = tipoEstado.ToUpper();
        //    int idTipoEstado = int.Parse(tipoEstado);
        //   //Console.WriteLine($"idTipoEstado {idTipoEstado}");
        //    return idTipoEstado switch
        //    {

        //        1 => BadgeStyle.Dark,
        //        4 => BadgeStyle.Warning,
        //        5 => BadgeStyle.Success,
        //        6 => BadgeStyle.Danger,
        //        7 => BadgeStyle.Dark,
        //        0 => BadgeStyle.Dark,
        //        _ => BadgeStyle.Dark
        //    };
        //}
        public string GetEstado(int tipoEstado)
        {
            return tipoEstado switch
            {

                1 => "ABIERTO",
                4 => "EN PROCESO",
                5 => "TERMINADO",
                6 => "CANCELADO",
                7 => "PENDIENTE",
                0 => "ESTADO NO IDENTIFICADO"
            };
        }

        public string GetTipoEstado(int tipoEstado)
        {
            return tipoEstado switch
            {

                1 => "ABIERTO",
                2 => "EN PROCESO",
                3 => "TERMINADO",
                4 => "CANCELADO",
                5 => "PENDIENTE"
            };
        }
    }
}
