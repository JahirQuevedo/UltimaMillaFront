using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Utilerias;
using ALOG.Modelos.Modelos.DTO.Vacios;
using AlogisticsWASM.Layout.Vacios.Patios;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Control.IControl;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using System.Text.RegularExpressions;

namespace AlogisticsWASM.Pages.Vacios.Solicitudes
{
    public partial class GridPeticionContenedorCmp
    {

        protected RadzenDataGrid<PeticionesContenedoresClienteExternoDTO> _gridContenedores;
        protected List<PeticionesContenedoresClienteExternoDTO> _contenedorToInsert;
        protected List<PeticionesContenedoresClienteExternoDTO> _contenedorToUpdate;
        protected PeticionesContenedoresClienteExternoDTO _contenedorUpdate;
        private List<string> _errores;
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }
        [Parameter] public List<PeticionesContenedoresClienteExternoDTO> Contenedores { get; set; }
        [Parameter] public EventCallback<List<PeticionesContenedoresClienteExternoDTO>> ContenedoresActualizados { get; set; }
        [Parameter] public EventCallback<PeticionesContenedoresClienteExternoDTO> ContenedorActualizado { get; set; }
        [Parameter] public EventCallback<List<PeticionesContenedoresClienteExternoDTO>> OnSeleccionContenedores { get; set; }

        [Parameter] public bool DeshabilitarAcciones { get; set; }
        [Parameter] public string TabType { get; set; }
        [Parameter] public RenderFragment ColumnTemplate { get; set; }
        [Parameter] public bool MostrarColumnaAcciones { get; set; }
        [Parameter] public int TipoServicio { get; set; }

        [Inject] private UsuarioTokenDTO UsuarioTokenDTO { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Inject] private ILoginService LoginService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] public IControlService ControlService { get; set; }
        [Inject] public ICatPatiosServices PatioService { get; set; }

        private Dictionary<string, bool> _dicAcceso;
        private bool _tieneAcceso;
        private Dictionary<PeticionesContenedoresClienteExternoDTO, string> _contenedorOriginal = new();
        private Dictionary<PeticionesContenedoresClienteExternoDTO, PeticionesContenedoresClienteExternoDTO> _copiasOriginales = new();

        private HashSet<string> _contenedoresSeleccionados = new();

        public void LimpiarSeleccion() => _contenedoresSeleccionados.Clear();
        private bool _seleccionarTodos = false;

        protected override void OnInitialized()
        {
            _contenedorToInsert = new List<PeticionesContenedoresClienteExternoDTO>();
            _contenedorToUpdate = new List<PeticionesContenedoresClienteExternoDTO>();
            _errores = new List<string>();
            MostrarColumnaAcciones = true;
        }

        public async void SeleccionarContenedor(string contenedor, bool isSelected)
        {
            if (isSelected)
                _contenedoresSeleccionados.Add(contenedor);
            else
                _contenedoresSeleccionados.Remove(contenedor);

            ActualizarEstadoSeleccionarTodos();

            if (OnSeleccionContenedores.HasDelegate)
                await OnSeleccionContenedores.InvokeAsync(ObtenerSeleccionados());
        }

        public List<PeticionesContenedoresClienteExternoDTO> ObtenerSeleccionados()
        {
            return Contenedores?.Where(c => _contenedoresSeleccionados.Contains(c.Contenedor)).ToList() ?? new();
        }

        private async Task SeleccionarTodosAsync(bool isChecked)
        {
            _seleccionarTodos = isChecked;

            if (isChecked)
                _contenedoresSeleccionados = Contenedores
                    .Select(c => c.Contenedor)
                    .ToHashSet();
            else
                _contenedoresSeleccionados.Clear();

            StateHasChanged();

            if (OnSeleccionContenedores.HasDelegate)
                await OnSeleccionContenedores.InvokeAsync(ObtenerSeleccionados());
        }

        private void ActualizarEstadoSeleccionarTodos()
        {
            _seleccionarTodos = Contenedores != null &&
                                Contenedores.Count > 0 &&
                                _contenedoresSeleccionados.Count == Contenedores.Count;
        }

        private async Task OnSeleccionarTodosChanged(bool value)
        {
            await SeleccionarTodosAsync(value);
        }

        private async Task ObtenerDatosUsuarioAsync()
        {

            try
            {
                var token = await JsRuntime.InvokeAsync<string>("localStorage.getItem", "JWT Token");
                if (!string.IsNullOrEmpty(token))
                {
                    if (UsuarioTokenDTO.IdCatUsuario == 0)
                    {
                        UsuarioTokenDTO = await LoginService.ObtenerdatosToken();

                        //if (!await LoginService.validarAccesoPagina(8, UsuarioTokenDTO)) {
                        //    NavigationManager.NavigateTo("/");
                        //} else {
                        //    _dicAcceso = await LoginService.validarControlesPagina(8, UsuarioTokenDTO);
                        //    _tieneAcceso = _dicAcceso["CREAR"];
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("APP.RAZOR:" + ex.Message);
            }
        }
        public void ShowNotification(string titulo, List<string> mensages, NotificationSeverity notificationSeverity)
        {

            string mensajeHtml = "<ul>";
            foreach (var mensaje in mensages)
            {
                mensajeHtml += $"<li>{mensaje}</li>";
            }
            mensajeHtml += "</ul>";

            NotificationService.Notify(new NotificationMessage { Severity = notificationSeverity, Summary = titulo, Detail = mensajeHtml, Duration = 8000 });
        }

        private RenderFragment RenderizarColumnaAcciones() => builder =>
        {
            int seq = 0;

            builder.OpenComponent<RadzenDataGridColumn<PeticionesContenedoresClienteExternoDTO>>(seq++);
            builder.AddAttribute(seq++, "Width", "150px");
            builder.AddAttribute(seq++, "Filterable", false);
            builder.AddAttribute(seq++, "Sortable", false);
            builder.AddAttribute(seq++, "TextAlign", TextAlign.Right);
            builder.AddAttribute(seq++, "Frozen", true);
            builder.AddAttribute(seq++, "FrozenPosition", FrozenColumnPosition.Right);

            builder.AddAttribute(seq++, "Template", (RenderFragment<PeticionesContenedoresClienteExternoDTO>)((contenedor) => (__builder) =>
            {
                int innerSeq = 0;

                __builder.OpenComponent<RadzenButton>(innerSeq++);
                __builder.AddAttribute(innerSeq++, "Icon", "edit");
                __builder.AddAttribute(innerSeq++, "ButtonStyle", ButtonStyle.Light);
                __builder.AddAttribute(innerSeq++, "Variant", Variant.Flat);
                __builder.AddAttribute(innerSeq++, "Size", ButtonSize.Medium);
                __builder.AddAttribute(innerSeq++, "Class", "rz-my-1 rz-ms-1");
                __builder.AddAttribute(innerSeq++, "Disabled", DeshabilitarAcciones);
                __builder.AddAttribute(innerSeq++, "Click", EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs e) => EditRow(contenedor)));
                __builder.CloseComponent();

                __builder.OpenComponent<RadzenButton>(innerSeq++);
                __builder.AddAttribute(innerSeq++, "Icon", "delete");
                __builder.AddAttribute(innerSeq++, "ButtonStyle", ButtonStyle.Danger);
                __builder.AddAttribute(innerSeq++, "Variant", Variant.Flat);
                __builder.AddAttribute(innerSeq++, "Size", ButtonSize.Medium);
                __builder.AddAttribute(innerSeq++, "Class", "rz-my-1 rz-ms-1");
                __builder.AddAttribute(innerSeq++, "Disabled", DeshabilitarAcciones);
                __builder.AddAttribute(innerSeq++, "Click", EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs e) => DeleteRow(contenedor)));
                __builder.CloseComponent();
            }));

            builder.AddAttribute(seq++, "EditTemplate", (RenderFragment<PeticionesContenedoresClienteExternoDTO>)((contenedor) => (__builder) =>
            {
                int innerSeq = 0;

                __builder.OpenComponent<RadzenButton>(innerSeq++);
                __builder.AddAttribute(innerSeq++, "Icon", "check");
                __builder.AddAttribute(innerSeq++, "ButtonStyle", ButtonStyle.Success);
                __builder.AddAttribute(innerSeq++, "Variant", Variant.Flat);
                __builder.AddAttribute(innerSeq++, "Size", ButtonSize.Medium);
                __builder.AddAttribute(innerSeq++, "Disabled", DeshabilitarAcciones);
                __builder.AddAttribute(innerSeq++, "Click", EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs e) => SaveRow(contenedor)));
                __builder.CloseComponent();

                __builder.OpenComponent<RadzenButton>(innerSeq++);
                __builder.AddAttribute(innerSeq++, "Icon", "close");
                __builder.AddAttribute(innerSeq++, "ButtonStyle", ButtonStyle.Light);
                __builder.AddAttribute(innerSeq++, "Variant", Variant.Flat);
                __builder.AddAttribute(innerSeq++, "Size", ButtonSize.Medium);
                __builder.AddAttribute(innerSeq++, "Class", "rz-my-1 rz-ms-1");
                __builder.AddAttribute(innerSeq++, "Disabled", DeshabilitarAcciones);
                __builder.AddAttribute(innerSeq++, "Click", EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs e) => CancelEdit(contenedor)));
                __builder.CloseComponent();

                __builder.OpenComponent<RadzenButton>(innerSeq++);
                __builder.AddAttribute(innerSeq++, "Icon", "delete");
                __builder.AddAttribute(innerSeq++, "ButtonStyle", ButtonStyle.Danger);
                __builder.AddAttribute(innerSeq++, "Variant", Variant.Flat);
                __builder.AddAttribute(innerSeq++, "Size", ButtonSize.Medium);
                __builder.AddAttribute(innerSeq++, "Class", "rz-my-1 rz-ms-1");
                __builder.AddAttribute(innerSeq++, "Disabled", DeshabilitarAcciones);
                __builder.AddAttribute(innerSeq++, "Click", EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs e) => DeleteRow(contenedor)));
                __builder.CloseComponent();
            }));

            builder.CloseComponent();
        };

        private PeticionesContenedoresClienteExternoDTO InitObject()
        {

            PeticionesContenedoresClienteExternoDTO cont = new PeticionesContenedoresClienteExternoDTO();
            PeticionesServiciosClienteExternoDTO serv = new PeticionesServiciosClienteExternoDTO();
            cont.Servicios = new List<PeticionesServiciosClienteExternoDTO>();

            serv.IdCatServicio = TabType switch
            {
                "MV" => 1,
                "EIR" => 2,
                _ => 0
            };

            cont.Contenedor = "";
            //cont.ClaveTipoContenedor = "";
            cont.ClienteRFC = "";
            cont.ClienteRazonSocial = "";
            cont.NombreEjecutivoSolicitante = "";


            serv.TransporteRFC = "";
            serv.TransporteRazonSocial = "";
            serv.TransporteNombreContacto = "";
            serv.TransporteEmailContacto = "";
            serv.NumeroBL = "";
            serv.NumeroViaje = "";
            serv.IdCatPatio = 0;
            serv.PatioRFC = "";
            serv.PatioRazonSocial = "";
            serv.FolioManiobra = "";
            serv.ConsignatarioRFC = "";
            serv.ConsignatarioRazonSocial = "";
            serv.NombreInstitucionBancaria = "";
            serv.NumeroCuentaEgresoPago = 0;
            serv.MontoSoliciud = 0;
            serv.MontoTotal = 0;
            serv.FechaPago = DateTime.Now;
            serv.FechaTocaPiso = DateTime.Now;
            serv.RFCFacturar = "";
            serv.RazonSocialFacturar = "";
            //if(serv.IdCatServicio == 1)
            //    serv.BL = new PeticionesDocumentosClienteExternoDTO() { IdTipoDocumento = 9, Nombre = "", MimeType = "", Base64 = "" };
            //if (serv.IdCatServicio == 2)
            //    serv.EirDeLleno = new PeticionesDocumentosClienteExternoDTO() { IdTipoDocumento = 8, Nombre = "", MimeType = "", Base64 = "" };

            cont.Servicios.Add(serv);


            return cont;

        }
        public void Reset()
        {
            _contenedorToInsert.Clear();
            _contenedorToUpdate.Clear();
            //Contenedores.Clear();
        }

        public void Reset(PeticionesContenedoresClienteExternoDTO contenedor)
        {
            _contenedorToInsert.Remove(contenedor);
            _contenedorToUpdate.Remove(contenedor);
        }

        public async Task EditRow(PeticionesContenedoresClienteExternoDTO contenedor)
        {
            Reset();

            if (!_copiasOriginales.ContainsKey(contenedor))
            {
                var copia = JsonConvert.DeserializeObject<PeticionesContenedoresClienteExternoDTO>(
                    JsonConvert.SerializeObject(contenedor));
                _copiasOriginales[contenedor] = copia;
            }

            if (!_contenedorOriginal.ContainsKey(contenedor))
                _contenedorOriginal[contenedor] = contenedor.Contenedor;

            _contenedorToUpdate.Add(contenedor);
            _contenedorUpdate = contenedor;
            await _gridContenedores.EditRow(contenedor);
        }

        public async Task SaveRow(PeticionesContenedoresClienteExternoDTO contenedor)
        {

            List<string> listaErroes = ContenedorEsValido(contenedor, TipoServicio);

            if (listaErroes.Count() > 0)
            {

                foreach (var e in listaErroes)
                {
                    if (_errores.Contains(e))
                    {
                        continue;
                    }
                    _errores.Add(e);
                }
                ShowNotification("Error al guardar contenedor", _errores, NotificationSeverity.Error);
                _errores.Clear();
                return;
            }

            if (!Contenedores.Contains(contenedor))
            {
                Contenedores.Add(contenedor);
            }

            if (contenedor.Servicios.Any(s => s.IdCatServicio == 2))
            {
                foreach (var servicio in contenedor.Servicios)
                {

                    var filtroPatio = new FiltroPatioDTO
                    {
                        ProveedorInfo = servicio.PatioRFC
                    };

                    string filtroCifrado = await ControlService.GetFiltroCifrado(filtroPatio);
                    var patios = await PatioService.ObtenerListaPatiosFiltrados(filtroCifrado) ?? new List<CatPatios>();

                    if (patios.Count() > 0)
                        servicio.IdCatPatio = patios.FirstOrDefault().IdCatPatios;
                }
            }

            _contenedorOriginal.Remove(contenedor);
            _copiasOriginales.Remove(contenedor);
            await ContenedorActualizado.InvokeAsync(contenedor);
            await ContenedoresActualizados.InvokeAsync(Contenedores);
            await _gridContenedores.UpdateRow(contenedor);
        }

        public async Task CancelEdit(PeticionesContenedoresClienteExternoDTO contenedor)
        {

            if (_copiasOriginales.TryGetValue(contenedor, out var copiaOriginal))
            {
                int index = Contenedores.IndexOf(contenedor);
                if (index != -1)
                {
                    Contenedores[index] = copiaOriginal;
                    await ContenedoresActualizados.InvokeAsync(Contenedores);
                }
                _copiasOriginales.Remove(contenedor);
            }
            Reset(_contenedorUpdate);
            Reset();
            _gridContenedores.CancelEditRow(_contenedorUpdate);
        }

        public async Task DeleteRow(PeticionesContenedoresClienteExternoDTO contenedor)
        {

            var result = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "¡Advertencia!",
                Text = $"¿Está seguro de eliminar el contenedor {contenedor.Contenedor}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí",
                CancelButtonText = "No"
            });

            if (string.IsNullOrEmpty(result.Value))
            {
                return;
            }
            Reset();
            Contenedores.Remove(contenedor);
            await ContenedoresActualizados.InvokeAsync(Contenedores);
            await _gridContenedores.Reload();
        }

        public async Task InsertRow()
        {
            Reset();
            var contenedor = InitObject();
            _contenedorToInsert.Add(contenedor);
            await _gridContenedores.InsertRow(contenedor);
        }

        public async Task InsertAfterRow(PeticionesContenedoresClienteExternoDTO after)
        {
            Reset();
            var contenedor = new PeticionesContenedoresClienteExternoDTO();
            _contenedorToInsert.Add(contenedor);
            await _gridContenedores.InsertAfterRow(contenedor, after);
        }

        protected async Task OnUpdateRow(PeticionesContenedoresClienteExternoDTO contenedor)
        {
            Reset(contenedor);
        }

        protected async Task OnCreateRow(PeticionesContenedoresClienteExternoDTO contenedor)
        {
            _contenedorToInsert.Remove(contenedor);
        }

        public async Task ActualizarTabla()
        {
            StateHasChanged();
            await _gridContenedores.Reload();
        }

        public List<string> ContenedorEsValido(PeticionesContenedoresClienteExternoDTO contenedor, int tipoServicio)
        {

            PeticionesServiciosClienteExternoDTO servicio = contenedor.Servicios.FirstOrDefault();

            if (_errores == null)
            {
                _errores = new List<string>();
            }

            //_errores.Clear();

            //bool comparacion = Contenedores.Any(c => c.Contenedor.Equals(contenedor.Contenedor));
            var regex = new Regex(@"^[A-Z]{4}\d{7}$");

            #region Validaciones generales
            if (string.IsNullOrEmpty(contenedor.Contenedor))
            {
                _errores.Add("El número de contenedor no puede estar vacío");
            }
            else
            {
                if (!regex.IsMatch(contenedor.Contenedor))
                {
                    _errores.Add($"El contenedor {contenedor.Contenedor}  no es un contenedor valido.");
                }

                string valorOriginal = _contenedorOriginal.TryGetValue(contenedor, out var original) ? original : null;
                bool fueModificado = !string.Equals(valorOriginal, contenedor.Contenedor, StringComparison.OrdinalIgnoreCase);

                if (fueModificado)
                {
                    foreach (var c in Contenedores)
                    {
                        if (c != contenedor && c.Contenedor.Equals(contenedor.Contenedor.ToUpper().Trim()))
                        {
                            _errores.Add($"El contenedor {contenedor.Contenedor} ya se encuentra en la solicitud");
                            break;
                        }
                    }
                    //bool esDuplicado = Contenedores.Any(c => c.Contenedor.Equals(contenedor.Contenedor, StringComparison.OrdinalIgnoreCase));

                    //if (esDuplicado) {
                    //    _errores.Add($"El contenedor {contenedor.Contenedor} ya se encuentra en la solicitud");
                    //}
                }
            }

            if (contenedor.IdCatTipoContenedor <= 0)
            {
                _errores.Add($"El tipo de contenedor del contenedor {contenedor.Contenedor} no es válido.");
            }

            //if (string.IsNullOrEmpty(contenedor.ClaveTipoContenedor)) {
            //    _errores.Add("El tipo de contenedor es requerido");
            //} else {
            //    if (!TipoContenedorEsValido(contenedor.ClaveTipoContenedor)) {
            //        _errores.Add($"El tipo de contenedor {contenedor.ClaveTipoContenedor} no tiene un formato válido");
            //    }
            //}

            if (string.IsNullOrEmpty(contenedor.ClienteRFC) && string.IsNullOrEmpty(servicio.RFCFacturar))
            {
                _errores.Add($"Debes indicar un RFC de cliente servico o RFC de cliente a facturar para el contenedor {contenedor.Contenedor}");
            }
            // Aquí se valida el RFC del cliente solicitante
            if (!string.IsNullOrEmpty(contenedor.ClienteRFC))
            {
                if (!RfcValido(contenedor.ClienteRFC))
                {
                    _errores.Add("El RFC del cliente servicio no es válido");
                }
                else
                {
                    if (string.IsNullOrEmpty(servicio.RFCFacturar))
                    {
                        servicio.RFCFacturar = contenedor.ClienteRFC;
                    }
                }
            }

            if (!string.IsNullOrEmpty(servicio.RFCFacturar))
            {
                if (!RfcValido(servicio.RFCFacturar))
                {
                    _errores.Add("El RFC del cliente a facturar no es válido");
                }
                else
                {
                    if (string.IsNullOrEmpty(contenedor.ClienteRFC))
                    {
                        contenedor.ClienteRFC = servicio.RFCFacturar;
                    }
                }
            }
            #endregion Validaciones generales

            #region Validaciones servicio de maniobras de vacíos
            if (tipoServicio == 0)
            {
                if (string.IsNullOrEmpty(servicio.NumeroBL))
                {
                    _errores.Add("El número de BL es requerido");
                }

                //if (string.IsNullOrEmpty(servicio.NavieraRFC)) {
                //    _errores.Add("El RFC de la naviera es requerido");
                //} else {
                //    if (!RfcValido(servicio.NavieraRFC)) {
                //        _errores.Add("El RFC de la naviera no es válido");
                //    }
                //}

                if (string.IsNullOrEmpty(servicio.NumeroViaje))
                {
                    _errores.Add("El número del viaje es requerido");
                }

                if (string.IsNullOrEmpty(servicio.NombreBuque))
                {
                    _errores.Add("El nombre del buque es requerido");
                }

                //if (string.IsNullOrEmpty(servicio.TransporteRFC)) {
                //    _errores.Add("El RFC del transportista es requerido");
                //} else {
                //    if (!RfcValido(servicio.TransporteRFC)) {
                //        _errores.Add("El RFC del transportista no es válido");
                //    }
                //}

                //if (string.IsNullOrEmpty(servicio.TransporteEmailContacto)) {
                //    _errores.Add("El email del transportista es requerido");
                //}

                //if (string.IsNullOrEmpty(servicio.TransporteRazonSocial)) {
                //    _errores.Add("La razón social del transportista es requerida");
                //}

            }
            #endregion Validaciones servicio de maniobras de vacíos

            #region Validaciones servicio de eir

            if (tipoServicio == 1)
            {
                if (string.IsNullOrEmpty(servicio.PatioRFC))
                {
                    _errores.Add("El RFC del patio es requerido");
                }
                else
                {
                    if (!RfcValido(servicio.PatioRFC))
                    {
                        _errores.Add("El RFC del patio no es válido");
                    }
                }
            }
            #endregion Validaciones servicio de eir
            return _errores;
        }

        private bool RfcValido(string rfc)
        {

            var resultado = ValidateRfc.RfcIsValid(rfc);
            return resultado.EsValido;
            //if (!resultado.EsValido) {
            //    resultado.MensajesError.ForEach(e => { _errores.Add(e); });
            //}
        }

        private bool TipoContenedorEsValido(string tipoContenedor)
        {

            if (string.IsNullOrWhiteSpace(tipoContenedor)) return false;

            tipoContenedor = tipoContenedor.ToUpper().Trim();

            // Verifica que coincida con el patrón general
            if (!Regex.IsMatch(tipoContenedor, @"^[A-Z]{2}\d{2}$"))
                return false;

            // Lista de prefijos no permitidos
            var prefijosInvalidos = new HashSet<string> { "XX", "ZZ", "AA" };

            string prefijo = tipoContenedor.Substring(0, 2);
            if (prefijosInvalidos.Contains(prefijo))
                return false;

            return true;
        }

        public async Task MostrarModalPatios()
        {

            var parametros = new Dictionary<string, object> { { "Contenedores", Contenedores } };

            var resultado = await DialogService.OpenAsync<AsignarPatioCmp>(
                "Asignar patio",
                parametros,
                new DialogOptions { Width = "900px", Height = "600px", CloseDialogOnOverlayClick = false });

            if (resultado is List<PeticionesContenedoresClienteExternoDTO> actualizados)
            {
                // actualizar tus listas locales o enviar al servidor
            }
        }


    }
}
