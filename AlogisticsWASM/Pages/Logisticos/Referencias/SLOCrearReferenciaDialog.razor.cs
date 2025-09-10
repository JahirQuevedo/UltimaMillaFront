using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Logistica;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AlogisticsWASM.Pages.Logisticos.Referencias
{
    public partial class SLOCrearReferenciaDialog
    {
        [Inject] private ISLOReferenciasService iSLOReferenciasService { get; set; }
        [Inject] private ICatClientesService icatClientesService { get; set; }
        [Inject] private ICatAduanaService icatAduanaService { get; set; }
        [Inject] private ILoginService iloginService { get; set; }
        [Inject] private SweetAlertService sweetAlertService { get; set; }

        private IEnumerable<RespListarCoincidenciasDTO> ListarClientes = new List<RespListarCoincidenciasDTO>();
        private IEnumerable<RespListarCoincidenciasDTO> ListarClientesAFact = new List<RespListarCoincidenciasDTO>();
        private ICollection<CatAduana> lstAduanas = new List<CatAduana>();
        private SLOReferenciasDTO ObjsloReferenciasDTO = new SLOReferenciasDTO();
        private RespListarCoincidenciasDTO selectedCliente;
        // private RespListarCoincidenciasDTO selectedClienteFact;

        private int intcliente = 0;
        //private int intclienteFact = 0;
        private string strAduana;
        private int intAduana = 0;

        private string strcliente;
        //private string strfactCliente;
        //private string strRefcliente;

        #region Cliente y clienteaFact autocompletable
        void OnClienteChange(object value)
        {
            selectedCliente = ListarClientes
                .FirstOrDefault(c => c.RazonSocial == value?.ToString());

            if (selectedCliente != null)
            {
                intcliente = selectedCliente.Id;
                strcliente = selectedCliente.RazonSocial;
            }
            else
            {
                intcliente = 0;
                strcliente = null;
            }
        }
        //void OnClienteFactChange(object value)
        //{
        //    selectedClienteFact = ListarClientesAFact
        //        .FirstOrDefault(c => c.RazonSocial == value?.ToString());

        //    if (selectedClienteFact != null)
        //    {
        //        intclienteFact = selectedClienteFact.Id;
        //        strfactCliente = selectedClienteFact.RazonSocial;
        //    }
        //    else
        //    {
        //        intclienteFact = 0;
        //        strfactCliente = null;
        //    }
        //}

        async Task AutoCompleteOnLoadCliente(LoadDataArgs args)
        {
            var strQuery = args.Filter;
            ListarClientes = await icatClientesService.GetClientesConincidencia(strQuery);
            //ListarClientesAFact = await icatClientesService.GetClientesConincidencia(strQuery);
        }

        #endregion Cliente y clienteaFact autocompletable

        #region AduanaDropDown
        void DropDownOnAduanaChange()
        {
            try
            {
                if (strAduana.Length > 0)
                {
                    intAduana = lstAduanas.Where(x => x.Nombre.Equals(strAduana)).FirstOrDefault().IdCatAduana;
                }
                else intAduana = 0;
            }
            catch (Exception ex)
            {
                intAduana = 0;
            }
        }
        #endregion AduanaDropDown

        private List<KeyValuePair<int, string>> tiposServicio = new()
        {
            new KeyValuePair<int, string>(1, "Servicio de Comercio Exterior"),
            new KeyValuePair<int, string>(2, "Servicio Logístico")
        };

        protected override async Task OnInitializedAsync()
        {
            lstAduanas = await icatAduanaService.GetAduanas();
            var usuario = await iloginService.ObtenerdatosToken();
            if (usuario != null)
            {
                ObjsloReferenciasDTO.IdCatUsuario = usuario.IdCatUsuario;
            }
            //_filtro = new FiltroOrdenesReferenciasDTO();
        }

        private async Task Guardar(SLOReferenciasDTO args)
        {
            ObjsloReferenciasDTO.IdCliente = intcliente;
            //ObjsloReferenciasDTO.IdClienteFacturar = intclienteFact;
            ObjsloReferenciasDTO.IdCatAduana = intAduana;
            //ObjsloReferenciasDTO.ReferenciaCliente = strRefcliente;

            var respuesta = await iSLOReferenciasService.CrearReferenciaALO(args);

            if (respuesta.IsSuccess)
            {
                await sweetAlertService.FireAsync("¡Referencia creada con éxito!", respuesta.strMensaje, SweetAlertIcon.Success);
                DialogService.Close(ObjsloReferenciasDTO);
            }
            else
            {
                await sweetAlertService.FireAsync("Error al crear la referencia", "El Cliente Solicitante es erroneo o no está disponible", SweetAlertIcon.Error);
            }
        }

    }
}