namespace ALOG.Modelos.Modelos.Autenticacion.TokenUsuario
{
    public class LoginResponse
    {
        public string Id { get; set; }
        public LoginResult Result { get; set; }
        public int Status { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCompletedSuccessfully { get; set; }
        public int CreationOptions { get; set; }
        public bool IsFaulted { get; set; }
    }
}
