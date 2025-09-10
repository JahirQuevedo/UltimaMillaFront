using AlogisticsWASM.Layout.Formularios;
using Microsoft.AspNetCore.Components;

namespace AlogisticsWASM.Layout.Generics
{
    public partial class ModalCreacionEdicion<TModel> : ComponentBase
    {

        [Parameter] public EventCallback<(TModel Modelo, bool EstaEditando)> OnClose { get; set; }
        [Parameter][EditorRequired] public bool EstaEditando { get; set; } = false;
        [Parameter] public TModel Modelo { get; set; } = default!;

        private string _tituloFormulario = string.Empty;
        private FormularioContenedor _formularioContenedor;
        private FormularioAcarreo _formularioAcarreo;

        protected override void OnParametersSet()
        {
            _tituloFormulario = EstaEditando ? "Editando" : "Creando";
        }

        private void GuardarModelo()
        {

            if (_formularioContenedor != null)
            {
                ////Console.WriteLine($"Modal - GuardarModelo()");
                var esValido = _formularioContenedor.DatosValidos();
                ////Console.WriteLine($"esValido {esValido}");
            }
            //else if (_formularioAcarreo != null) {
            //    //Console.WriteLine($"Modal - _formularioAcarreo - GuardarModelo()");
            //    var esValido = false;
            //    //Console.WriteLine($"Modal - _formularioAcarreo - esValido {esValido}");
            //}

            //TModel? modelo = default;

            //if (typeof(TModel) == typeof(Contenedor) && _formularioContenedor != null) {
            //    modelo = (TModel)(object)_formularioContenedor.ObtenerModelo();
            //} else if (typeof(TModel) == typeof(Referencia) && _formularioReferencia != null) {
            //    modelo = (TModel)(object)_formularioReferencia.ObtenerModelo();
            //}

            //if (modelo != null) {
            //    await OnClose.InvokeAsync((modelo, EstaEditando));
            //}
        }

    }
}
