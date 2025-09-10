namespace ClienteBlazorWASM.Helpers
{
    public static class Inicializar
    {

        //LOCAL IIS

        public const string UrlApiControl = "https://localhost:7087/";
        public const string UrlApiCatalogos = "https://localhost:7211/";
        public const string UrlApiLogistico = "https://localhost:7185/";
        public const string UrlApiLogisticWMS = "https://localhost:7146/";

        //LOCAL IIS        

        //public const string UrlApiControl = "https://127.0.0.1/alogisticsapis/APIControl/";
        //public const string UrlApiCatalogos = "https://127.0.0.1/alogisticsapis/APICatalogos/";
        //public const string UrlApiLogistico = "https://127.0.0.1/alogisticsapis/APILogistico/";
        //public const string UrlApiLogisticWMS = "https://127.0.0.1/alogisticsapis/APIWms/";


        //SERVERQA

        //public const string UrlApiControl = "https://172.31.92.39/alogisticsapis/APIControl/";
        //public const string UrlApiCatalogos = "https://172.31.92.39/alogisticsapis/APICatalogos/";
        //public const string UrlApiLogistico = "https://172.31.92.39/alogisticsapis/APILogistico/";
        //public const string UrlApiLogisticWMS = "https://172.31.92.39/alogisticsapis/APIWms/";

        //SERVER PROD
        //public const string UrlApi = "https://www.alogistics.com.mx/alogisticsapis/";        

        //public const string UrlApiControl = "https://www.alogistics.com.mx/alogisticsapis/APIControl/";
        //public const string UrlApiCatalogos = "https://www.alogistics.com.mx/alogisticsapis/APICatalogos/";
        //public const string UrlApiLogistico = "https://www.alogistics.com.mx/alogisticsapis/APILogistico/";
        //public const string UrlApiLogisticWMS = "https://www.alogistics.com.mx/alogisticsapis/APIWms/";


        public const string ClaveSecreta = "GnQ0kgS2RvudC90fm6dwsb4bx0pltf5Z6ahgUKiE6tiKBjfQTX";



        public const string Token_Local = "JWT Token";
        public const string Datos_Usuario_Local = "Detalle Usuario";

        // URL api del proyecto DT
        // Encabezado
        public const string DtUltimaMillaActualizarUrlApi = "DTLogistica/DtUltimaMilla/UltMillaEncActualizar";
        public const string DtUltimaMillaCrearUrlApi = "DTLogistica/DtUltimaMilla/UltMillaEncCrear";
        public const string DtUltimaMillaCambiarTipoEstadoUrlApi = "DTLogistica/DtUltimaMilla/UltMillaEncCambiarEstado/";
        // Detalles
        public const string DtUltimaMillaDetalleActualizarUrlApi = "DTLogistica/DtUltimaMilla/UMillaDetActualizar";

    }
}
