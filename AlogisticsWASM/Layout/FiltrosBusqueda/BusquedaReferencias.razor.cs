namespace AlogisticsWASM.Layout.FiltrosBusqueda
{
    public partial class BusquedaReferencias
    {

        private int Ticket { get; set; } = 0;
        private string TransporteRazonSocial { get; set; } = string.Empty;

        // Método público que el componente padre puede llamar
        public Dictionary<string, object> GetFiltros()
        {
            return new Dictionary<string, object>{
                    { "Ticket", Ticket },
                    { "TransporteRazonSocial", TransporteRazonSocial }
             };
        }

        public void SetFiltros(Dictionary<string, object> filtros)
        {
            if (filtros.Count() == 0)
            {
                Ticket = 0;
                TransporteRazonSocial = string.Empty;
                InvokeAsync(StateHasChanged);
            }
        }
    }
}
