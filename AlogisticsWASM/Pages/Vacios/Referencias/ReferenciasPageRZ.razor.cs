using ALOGModelos.Modelos.Logisticos;
using Radzen.Blazor;
using Radzen;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using Microsoft.AspNetCore.Components;
using ALOGModelos.Modelos.FiltrosBusqueda;
using ALOGModelos.Modelos.Autenticacion.TokenUsuario;
using ALOGModelos.Modelos.Vacios.Peticiones;
using System.Text;
using Newtonsoft.Json;
using ALOGModelos.Modelos.Dtos;
using System;
using ALOGRepositorios.Services.Logisticos;
using AlogisticsWASM.Layout.Generics;
using ALOGRepositorios.Services.Peticiones.IPeticiones;

namespace AlogisticsWASM.Pages.Vacios.Referencias
{

    public partial class ReferenciasPageRZ : ComponentBase
    {

        #region Variables
        [Inject] public IReferenciaService iReferenciasService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        bool isLoading;
        int count;
        private FiltroReferencia _filtro;
        ODataEnumerable<Referencia> _ODReferencias;
        IList<Referencia> selectedReferencia;
        RadzenDataGrid<Referencia> grid;
        private ICollection<Referencia> _lstReferencias;
        private List<Referencia> _lstReferenciasPage;

        private IEnumerable<Referencia> _IEReferencias;
        LogicalFilterOperator logicalFilterOperator = LogicalFilterOperator.And;
        FilterCaseSensitivity filterCaseSensitivity = FilterCaseSensitivity.CaseInsensitive;
        private DateTime dFSolicitudIni;
        private DateTime dFSolicitudFin;
        private DateTime dFSolicitudCierre;
        private string strPcontenedor;
        private string strPcliente;

        private int intPcliente;
        private string strProveedor;
        private int intProveedor;
        IEnumerable<string> selectedCustomers;




        int position = 1;

        #endregion Variables

        #region Init
        protected override async Task OnInitializedAsync()
        {

            isLoading = true;
            _filtro = new FiltroReferencia();
            //_filtro.NumeroPagina = 1;
            //_filtro.NumeroRegistros = 20;
            //_filtro.IdCatTipoEstados = 2;
            dFSolicitudIni = DateTime.Now.AddDays(-7);
            dFSolicitudFin = DateTime.Now;

            _filtro.Activo = true;
            await ObtenerDatos();
            isLoading = false;


        }
        #endregion Init

        #region OperacionesRadzen
        BadgeStyle GetBadgeStyle(string tipoEstado)
        {
            return tipoEstado switch
            {
                "Abierto" => BadgeStyle.Info,
                "En Proceso" => BadgeStyle.Warning,
                "Terminado" => BadgeStyle.Success,
                "Cancelado" => BadgeStyle.Danger,
                _ => BadgeStyle.Secondary
            };
        }

        void Change(string text)
        {
            //Console.WriteLine.Log($"{text}");
        }

        private async Task EnviarDatos()
        {
            _filtro = new FiltroReferencia();
            _filtro.Contenedor = strPcontenedor;
            _filtro.FechaSolicitudIni = dFSolicitudIni;
            _filtro.FechaSolicitudFin = dFSolicitudFin;
            await ObtenerDatos();
        }
        #endregion OperacionesRadzen

        #region Operaciones
        private async Task ObtenerDatos()
        {

            _lstReferencias = new List<Referencia>();

            _lstReferencias = await iReferenciasService!.GetReferencias(_filtro);
            Console.WriteLine(_lstReferencias);
            Console.WriteLine("Cuantos registros: " + _lstReferencias.Count());


            if (_lstReferencias == null)
            {
                _lstReferencias = new List<Referencia>();
            }
            else
            {
                //_lstAcarreosWork = _lstAcarreos;                
                var result = _lstReferencias.AsEnumerable();
                // Update the Data property
                _ODReferencias = result.AsODataEnumerable();
                Console.WriteLine(_ODReferencias);

            }
        }

        private void CrearReferencia()
        {
            _navigationManager.NavigateTo("ordenes-crear");
        }

        private void VerDetalles(Referencia referencia)
        {
            //ObtenerReferenciaSeleccionada(referencia);
            _navigationManager.NavigateTo($"/contenedores/{referencia.IdReferencia}");
        }
        #endregion Operaciones

    }
}
