using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Respuestas;
using ALOG.Modelos.Modelos.DTO.Solicitudes;
using ALOG.Modelos.Modelos.Orden;
using ALOG.Modelos.Modelos.Vacios;
using AlogisticsWASM.Layout.Utilerias;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Text.Json;

namespace AlogisticsWASM.Pages.Vacios.Contenedores
{
    public partial class GridContenedoresCmp
    {
        #region Parametros

        [Parameter]
        public IEnumerable<PeticionesContenedores> paramContenedores { get; set; }
        [Parameter]
        public bool paramMostraServicios { get; set; }
        [Parameter]
        public int paramIdOrden { get; set; }
        [Parameter]
        public int paramIdReferencia { get; set; }
        [Parameter]
        public bool paramMostarControles { get; set; }

        [Parameter]
        public bool ParamCRUDControles { get; set; }
        [Parameter]
        public UsuarioTokenDTO ParamUsuarioTokenDTO { get; set; }
        [Parameter]
        public Dictionary<string, bool> ParamdicAcceso { get; set; }

        [Parameter]
        public EventCallback<List<PeticionesContenedores>> OnValueChanged { get; set; }
        #endregion Parametros

        #region Variables
        private UtileriasPage objUtileriasPage = new UtileriasPage();
        [Inject] IContenedorService iContenedorService { get; set; }
        [Inject] SweetAlertService sweetAlertService { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] IOrdenService iOrdenService { get; set; }
        [Inject] IReferenciaService ireferenciaService { get; set; }
        [Inject] ILoginService loginService { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }

        private Dictionary<string, bool> dicAcceso = new Dictionary<string, bool>();
        private bool dicAccesoActualizar = false;

        private SolActualizarFolioManiobraDTO objSolActualizarFolioManiobraDTO = new SolActualizarFolioManiobraDTO();
        private SolCambioEstadoDTO objSolCambioEstadoDTO = new SolCambioEstadoDTO();
        List<CatReferenciaEstado> lstobjTipoEstados = new List<CatReferenciaEstado>();
        CatReferenciaEstado objTipoEstados = new CatReferenciaEstado();
        RadzenDataGrid<PeticionesContenedores> grid;
        string columnEditing;
        IEnumerable<PeticionesContenedores> IEcontenedoresOrig;
        List<KeyValuePair<int, string>> editedFields = new List<KeyValuePair<int, string>>();
        List<PeticionesContenedores> ordersToUpdate = new List<PeticionesContenedores>();
        private Ordenes _objOrden = new Ordenes();
        private PeticionesReferencias _objReferencia = new PeticionesReferencias();
        ODataEnumerable<PeticionesReferencias> _ODReferencias;
        ODataEnumerable<PeticionesContenedores> _ODContenedores;
        IList<PeticionesReferencias> selectedReferencia = new List<PeticionesReferencias>();

        IEnumerable<PeticionesContenedores> _IEContenedores = new List<PeticionesContenedores>();
        IEnumerable<PeticionesServicios> _IEServicio = new List<PeticionesServicios>();
        IEnumerable<PeticionesDocumentos> _IEDocumentos = new List<PeticionesDocumentos>();
        RadzenDataGrid<PeticionesReferencias> grid2 = new RadzenDataGrid<PeticionesReferencias>();
        private ICollection<PeticionesReferencias> _lstReferencias = new List<PeticionesReferencias>();
        private List<PeticionesReferencias> _lstReferenciasPage = new List<PeticionesReferencias>();
        private IEnumerable<PeticionesReferencias> _IEReferencias = new List<PeticionesReferencias>();

        private bool blpUsuarioInterno = false;
        #endregion Variables

        #region Operaciones

        public async Task AgregarContenedor()
        {
            await SendValue();
        }
        private async Task SendValue()
        {
            await OnValueChanged.InvokeAsync(_IEContenedores.ToList());
        }
        public async Task CerrarTodosContenedores()
        {
            // Promise/Task based
            //await sweetAlertService.FireAsync("Hello world!");

            SweetAlertResult swalresult = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = $"¿Desea cerrar todos los contenedores de la referencia {_objOrden.ReferenciaALO}?",
                Text = "En el proceso todos los contenedores serán marcados como TERMINADOS!",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí, cierra todos!",
                CancelButtonText = "No"
            });

            if (!string.IsNullOrEmpty(swalresult.Value))
            {
                objSolCambioEstadoDTO = new SolCambioEstadoDTO();
                objSolCambioEstadoDTO.IdCatReferenciaEstado = 5;
                objSolCambioEstadoDTO.IdReferencia = paramContenedores.First().IdReferencia;
                objSolCambioEstadoDTO.TipoCambio = "R";

                var resultado = await iContenedorService.ActualizaEstadoTodosContenedores(objSolCambioEstadoDTO);
                ////Console.WriteLine("resultado:" + resultado.Count());

                if (resultado != null)
                {
                    if (resultado.Count() == 0)
                    {
                        await sweetAlertService.FireAsync(
                       "Contenedores Cerrados",
                       "El proceso ha sido completado.",
                       SweetAlertIcon.Success
                       );
                    }

                    else
                    {
                        foreach (string elemento in resultado)
                        {
                            ////Console.WriteLine("lista:" + elemento);
                        }

                        await sweetAlertService.FireAsync(
                       "Contenedores NO Cerrados: " + string.Join(",", resultado),
                       "El proceso ha fallado.",
                       SweetAlertIcon.Error
                       );
                    }
                }

            }
            else if (swalresult.Dismiss == DismissReason.Cancel)
            {
                await sweetAlertService.FireAsync(
                    "Cancelled",
                    "Your imaginary file is safe :)",
                    SweetAlertIcon.Error
                    );
            }




        }

        public async Task ObtenerOrden()
        {

            _objOrden = new Ordenes();
            _objOrden = await iOrdenService!.GetOrden(paramIdOrden);

            if (_objOrden != null)
            {

            }
        }
        #endregion Operaciones


        protected override async Task OnInitializedAsync()
        {

            #region ValidarAcceso
            try
            {
                var _UsuarioTokenDTO = ParamUsuarioTokenDTO;
                if (_UsuarioTokenDTO.IdCatUsuario == 0)
                {
                    ////Console.WriteLine($"ENTRO APP VACIO");
                    _UsuarioTokenDTO = await loginService.ObtenerdatosToken();

                    ////Console.WriteLine($"SALIO APP {_UsuarioTokenDTO.Nombre}");
                }
                if (!await loginService.validarAccesoPagina(2, _UsuarioTokenDTO))
                {
                    _navigationManager.NavigateTo("/");
                }
                else
                {
                    blpUsuarioInterno = _UsuarioTokenDTO.catUsuarios.catUsuariosEmpresas.Any();
                    //dicAcceso = await loginService.validarControlesPagina(2, _UsuarioTokenDTO);
                    dicAccesoActualizar = ParamdicAcceso["ACTUALIZAR"];

                }

            }
            catch (Exception ex)
            {

                _navigationManager.NavigateTo("/");
            }
            #endregion ValidarAcceso

            await ObtenerOrden();

            lstobjTipoEstados = objUtileriasPage.ObtenerEstadosReferencia();


            IEcontenedoresOrig = paramContenedores;
            _IEContenedores = paramContenedores;
            //Console.WriteLine($"_IEContenedores {JsonConvert.SerializeObject(_IEContenedores, Formatting.Indented)}");

        }

        /// <summary>
        /// Determines if the specified column is in edit mode for the specified order.
        /// </summary>
        /// <param name="columnName">The RadzenDataGridColumn.Property currently being rendered by the RadzenDataGrid.</param>
        /// <param name="order">The Order currently being rendered by the RadzenDataGrid.</param>
        /// <returns>True if the column should render the EditTemplate for the specified Order, otherwise false.</returns>
        bool IsEditing(string columnName, PeticionesContenedores pCont)
        {
            ////Console.WriteLine("IsEditing");
            // Comparing strings is quicker than checking the contents of a List, so let the property check fail first.
            return columnEditing == columnName && ordersToUpdate.Contains(pCont);
        }

        /// <summary>
        /// Determines if the specified column needs a custom CSS class based on the <typeparamref name="TItem">TItem's</typeparamref> state.
        /// </summary>
        /// <param name="column">The RadzenDataGridColumn.Property currently being rendered by the RadzenDataGrid.</param>
        /// <param name="order">The Order currently being rendered by the RadzenDataGrid.</param>
        /// <returns>A string containing the CssClass to add, or <see cref="String.Empty">.</returns>
        string IsEdited(RadzenDataGridColumn<PeticionesContenedores> column, PeticionesContenedores order)
        {
            ////Console.WriteLine("IsEdited");
            // In a real scenario, you might use IRevertibleChangeTracking to check the current column
            //  against a list of the object's edited fields.
            return editedFields.Where(c => c.Key == order.IdContenedor && c.Value == column.Property).Any() ?
                "table-cell-edited" :
                string.Empty;
        }

        /// <summary>
        /// Handles the CellClick event of the RadzenDataGrid.
        /// </summary>
        /// <param name="args"></param>
        async Task OnCellClick(DataGridCellMouseEventArgs<PeticionesContenedores> args)
        {
            ////Console.WriteLine("OnCellClick");
            ////Console.WriteLine(args);
            ////Console.WriteLine(args.Data);
            ////Console.WriteLine(args.Column.Property);
            //////Console.WriteLine(!gridSolicitudesDetalle.IsValid);
            ////Console.WriteLine(ordersToUpdate.Contains(args.Data));
            if (!grid.IsValid ||
                (ordersToUpdate.Contains(args.Data) && columnEditing == args.Column.Property)) return;
            ////Console.WriteLine("OnCellClick2:" + args.Data);
            ////Console.WriteLine("OnCellClick2:" + columnEditing);
            // Record the previous edited field, if you're not using IRevertibleChangeTracking to track object changes

            if (ordersToUpdate.Any())
            {
                ////Console.WriteLine("OnCellClick3");
                editedFields.Add(new(ordersToUpdate.First().IdContenedor, columnEditing));



            }

            columnEditing = args.Column.Property;
            await EditRow(args.Data);
        }

        void Reset(PeticionesContenedores order = null)
        {
            ////Console.WriteLine("Reset");
            editorFocused = false;

            if (order != null)
            {
                ordersToUpdate.Remove(order);
            }
            else
            {
                ordersToUpdate.Clear();
            }
        }
        async Task UpdateFolioManiobra()
        {
            editorFocused = false;
            var order = ordersToUpdate.First();
            objSolActualizarFolioManiobraDTO = new SolActualizarFolioManiobraDTO();
            objSolActualizarFolioManiobraDTO.FolioManiobra = order.FolioManiobra;
            objSolActualizarFolioManiobraDTO.IdContenedor = order.IdContenedor;
            objSolActualizarFolioManiobraDTO.IdReferencia = order.IdReferencia;
            var result = await iContenedorService.ActualizaFolioContenedor(objSolActualizarFolioManiobraDTO);
        }
        async Task Update()
        {
            ////Console.WriteLine("update");
            editorFocused = false;
            ////Console.WriteLine(ordersToUpdate);
            //Actualizar solicitud.
            objSolCambioEstadoDTO = new SolCambioEstadoDTO();
            var contenedor = ordersToUpdate.First();
            var contenedorback = paramContenedores.FirstOrDefault(x => x.IdContenedor == contenedor.IdContenedor);
            objSolCambioEstadoDTO.IdContenedor = contenedor.IdContenedor;
            objSolCambioEstadoDTO.IdCatReferenciaEstado = contenedor.IdEstadoContenedor;
            objSolCambioEstadoDTO.IdReferencia = contenedor.IdReferencia;
            ////Console.WriteLine("IdReferencia:" + objSolCambioEstadoDTO.IdReferencia);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Para una salida JSON con formato
            };

            var resultado = await iContenedorService.ActualizaEstadoContenedor(objSolCambioEstadoDTO);
            if (resultado.IsSuccess)
            {
                ordersToUpdate.First().EstadoContenedor = lstobjTipoEstados.First(x => x.IdCatReferenciaEstado == ordersToUpdate.First().IdEstadoContenedor).Nombre;


                string jsonString = System.Text.Json.JsonSerializer.Serialize(ordersToUpdate, options);
                ////Console.WriteLine(jsonString);
                if (ordersToUpdate.Any())
                {
                    await grid.UpdateRow(ordersToUpdate.First());

                }
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success Summary", Detail = $"Actualización correcta. {contenedor.Contenedor}", Duration = 4000 });
            }
            else
            {
                string jsonString = System.Text.Json.JsonSerializer.Serialize(ordersToUpdate, options);
                //En caso de error actualizamos con el back.
                ordersToUpdate.First().IdEstadoContenedor = contenedorback.IdEstadoContenedor;
                ordersToUpdate.First().EstadoContenedor = lstobjTipoEstados.First(x => x.IdCatReferenciaEstado == contenedorback.IdEstadoContenedor).Nombre;
                await grid.UpdateRow(ordersToUpdate.First());
                await obtenerReferencia();
                await grid.RefreshDataAsync();
                grid.ResetLoadData();

                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error Summary", Detail = $"Error: {String.Join(Environment.NewLine, resultado.lstrErrorMessages)} ", Duration = 4000 });


            }
        }

        async Task EditRow(PeticionesContenedores order)
        {
            ////Console.WriteLine("EditRow");
            Reset();
            ////Console.WriteLine(order.IdEstadoContenedor);
            ////Console.WriteLine(order.EstadoContenedor);
            ordersToUpdate.Add(order);

            await grid.EditRow(order);
        }

        /// <summary>
        /// Saves the changes from the Order to the database.
        /// </summary>
        /// <param name="order">The <see cref="Order" /> to save.</param>
        /// <remarks>
        /// Currently, this is called every time the Cell is changed. In a real in-cell edit scenario, you would likely either update
        /// on RowDeselect, or batch the changes using a "Save Changes" button in the header.
        /// </remarks>
        void OnUpdateRow(PeticionesContenedores order)
        {
            ////Console.WriteLine("OnUpdateRow");
            ////Console.WriteLine("OnUpdateRow:" + order.Contenedor);
            ////Console.WriteLine("OnUpdateRow:" + order.IdContenedor);
            ////Console.WriteLine("OnUpdateRow:" + order.FolioManiobra);
            Reset(order);
            ////Console.WriteLine("OnUpdateRow:" + order.Contenedor);
            ////Console.WriteLine("OnUpdateRow:" + order.IdContenedor);
            ////Console.WriteLine("OnUpdateRow:" + order.FolioManiobra);



            // dbContext.Update(order);

            // dbContext.SaveChanges();

            // If you were doing row-level edits and handling RowDeselect, you could use the line below to
            // clear edits for the current record.

            //editedFields = editedFields.Where(c => c.Key != order.OrderID).ToList();

        }

        IRadzenFormComponent editor;
        bool editorFocused;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            ////Console.WriteLine("OnAfterRenderAsync");
            await base.OnAfterRenderAsync(firstRender);

            if (!editorFocused && editor != null)
            {
                editorFocused = true;

                try
                {
                    await editor.FocusAsync();
                }
                catch
                {
                    //
                }
            }
        }

        public class TipoEstado
        {


            public int Id { get; private set; }
            public string Nombre { get; private set; }

            public TipoEstado(int id, string nombre)
            {
                Id = id;
                Nombre = nombre;
            }

            public override string ToString()
            {
                return Nombre;
            }
        }


        public async Task obtenerReferencia()
        {
            _objReferencia = await ireferenciaService!.ObtenerReferencia(paramIdReferencia);

            if (_objReferencia == null)
            {
                _objReferencia = new PeticionesReferencias();
            }
            else
            {
                _lstReferencias = new List<PeticionesReferencias>();
                //_lstAcarreosWork = _lstAcarreos;
                _lstReferencias.Add(_objReferencia);
                var result = _lstReferencias.AsEnumerable();
                // Update the Data property
                _ODReferencias = result.AsODataEnumerable();
                _IEReferencias = _lstReferencias.AsEnumerable();

                //string json = JsonSerializer.Serialize(_objReferencia, new JsonSerializerOptions { WriteIndented = true });

                // Escribir el JSON en la consola
                //////Console.WriteLine("JSON:" + json);

                var contenedoresList = _IEReferencias
                   .Where(referencia => referencia.Contenedores != null)
                    .SelectMany(referencia => referencia.Contenedores)
                    .ToList();


                // Asignar la lista combinada a _ODContenedores
                _IEContenedores = contenedoresList.AsEnumerable();


                //Obtenemos Servicios.
                var serviciosList = _IEContenedores
                    .Where(contenedor => contenedor.Servicios != null)
                    .SelectMany(contenedor => contenedor.Servicios)
                    .ToList();
                _IEServicio = serviciosList.AsEnumerable();


                var DocumentosList = _IEServicio
                    .Where(serv => serv.Documentos != null)
                    .SelectMany(serv => serv.Documentos)
                    .ToList();
                _IEDocumentos = DocumentosList.AsEnumerable();

            }
        }
        public async Task OnContenedorActualizarGridHandler(List<String> pContenedoresActualizarGrid)
        {
            RespuestaGenericaDTO objrespuestaGenericaDTO = new RespuestaGenericaDTO();
            PeticionesContenedores objPeticionesContenedores = new PeticionesContenedores();

            if (pContenedoresActualizarGrid.Any())
            {

                ordersToUpdate.Clear();
                int pIdContenedor = int.Parse(pContenedoresActualizarGrid[1]);
                int pIdEstadoContenedor = int.Parse(pContenedoresActualizarGrid[1]);
                objPeticionesContenedores.IdContenedor = pIdContenedor;
                var objresp = await iContenedorService.ObtenerContenedor(objPeticionesContenedores);

                if (objresp != null)
                {
                    var objContenedor = objresp;
                    ordersToUpdate.Add(
                    objContenedor);

                    _IEContenedores.First(x => x.IdContenedor == pIdContenedor).IdEstadoContenedor = objContenedor.IdEstadoContenedor;
                    _IEContenedores.First(x => x.IdContenedor == pIdContenedor).EstadoContenedor = objContenedor.catReferenciaEstado.Nombre;
                    ordersToUpdate.First().IdEstadoContenedor = objContenedor.IdEstadoContenedor;
                    ordersToUpdate.First().EstadoContenedor = objContenedor.catReferenciaEstado.Nombre;
                    await grid.UpdateRow(ordersToUpdate.First());
                    await grid.RefreshDataAsync();
                    grid.ResetLoadData();
                }
            }
            //else ////Console.WriteLine("OnContenedorActualizarGrid NADA VACIO");

        }

    }
}
