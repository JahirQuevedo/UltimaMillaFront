namespace ALOG.Modelos.Modelos {
    public class HttpResponseWraper<T> {

        public HttpResponseWraper(T response, bool error, HttpResponseMessage httpResponseMessage) {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        // Variable para controlar si hubo un error
        public bool Error { get; set; }
        // Almacenará la respuesta
        public T? Response { get; set; }
        // Almacena todo la información posible de la respuesta
        public HttpResponseMessage HttpResponseMessage { get; set; }

    }
}
