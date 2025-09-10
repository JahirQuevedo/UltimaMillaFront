using Microsoft.JSInterop;

namespace ALOGRepositorios.Helpers
{
    public static class IJSRuntimeExtensionMethods
    {

        public static ValueTask<object> GuardarEnLocalStorage(this IJSRuntime js, string key, string value)
        {
            return js.InvokeAsync<object>("localStorage.setItem", key, value);
        }

        public static ValueTask<object> ObtenerDeLocalStorage(this IJSRuntime js, string key)
        {
            return js.InvokeAsync<object>("localStorage.getItem", key);
        }

        public static ValueTask<object> RemoverDelLocalStorage(this IJSRuntime js, string key)
        {
            return js.InvokeAsync<object>("localStorage.removeItem", key);
        }

    }
}
