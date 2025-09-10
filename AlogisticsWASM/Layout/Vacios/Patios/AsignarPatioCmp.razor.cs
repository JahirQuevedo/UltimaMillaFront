using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.DTO.Utilerias;
using ALOG.Modelos.Modelos.DTO.Vacios;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Control.IControl;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AlogisticsWASM.Layout.Vacios.Patios
{
    public partial class AsignarPatioCmp
    {

        private RadzenDataGrid<CatPatios> _gridPatios;

        [Parameter] public List<PeticionesContenedoresClienteExternoDTO> Contenedores { get; set; } = new();
        [Parameter] public string AduanaNombre { get; set; }

        // Banderas para mostrar formularios según validación
        private bool _esPatioHazesa;
        private bool _esNavieraHyundai;

        // Colecciones de patios
        private ICollection<CatPatios> _patiosOriginales;
        private ICollection<CatPatios> _patios;

        // Propiedad de selección única (sin usar RowSelectEventArgs)
        // Se utiliza dos vía (bind-Value) en el DataGrid.
        private CatPatios _patioSeleccionado;
        private IList<CatPatios> _patiosSeleccionados;
        private IList<CatPatios> PatioSeleccionado
        {
            get => _patiosSeleccionados;
            set
            {
                if (_patioSeleccionado != value)
                {
                    _patiosSeleccionados = value;
                    // Al actualizar la selección, se aplica la validación
                    HandlePatioSelection();
                }
            }
        }

        [Inject] public ICatProveedorService ProvedoreService { get; set; }
        [Inject] public ICatPatiosServices PatioService { get; set; }
        [Inject] public NotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public IControlService ControlService { get; set; }
        [Inject] public ICatAduanaService CatAduanaService { get; set; }

        // Campos de formulario
        private string _nombreChofer;
        private string _numeroLicenciaChofer;
        private string _numeroUnidad;
        private string _numeroPlacas;
        private string _comentarios;

        // Constantes para validación
        private const string _rfcPatioHazesa = "TSH081231SK5";
        private const string _rfcHyundai = "HYN140815591";

        private bool _isLoading;
        private string _busquedaPatio = string.Empty;
        private List<string> _errores = new();
        private int _sizeColumns;
        private FiltroPatioDTO _filtroPatio;
        private CatAduana _catAduana;
        private string BusquedaPatio
        {
            get => _busquedaPatio;
            set
            {
                _busquedaPatio = value;
                FiltrarPatios(value);
            }
        }

        // Estilo para animación en el contenedor de campos (puedes ajustarlo)
        private string DetallePatioAnimacion =>
            $"rz-text-align-center rz-p-2 column-transicion-suave {(_esPatioHazesa || _esNavieraHyundai ? "slide-in" : (_mostrandoAnimacion ? "slide-out" : ""))}";

        private bool _mostrandoAnimacion = false;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            _sizeColumns = 12;
            _filtroPatio = new FiltroPatioDTO();
            _catAduana = new CatAduana();
            // Obtenemos la lista de patios
            //_patiosOriginales = await ProvedoreService.ObtenerProveedores();
            _patiosOriginales = new List<CatPatios>();
            //_patios = _patiosOriginales.ToList();
            _patiosSeleccionados = new List<CatPatios>();
            InitContenedores();
            // Validación inicial basada en la lista Contenedores
            ValidarContenedoresInicial();
            _isLoading = false;
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(AduanaNombre))
            {
                var coincidencias = await CatAduanaService.GetAduanasConincidencia(AduanaNombre);
                var coincidencia = coincidencias.FirstOrDefault();
                _catAduana.IdCatAduana = coincidencia.Id;
                _catAduana.Nombre = coincidencia.RazonSocial;
                _catAduana.Acronimo = coincidencia.Acronimo;
                _filtroPatio.IdCatAduana = _catAduana.IdCatAduana;
                string filtroCifrado = await ControlService.GetFiltroCifrado(_filtroPatio);
                _patiosOriginales = await PatioService.ObtenerListaPatiosFiltrados(filtroCifrado);
                _patios = _patiosOriginales.ToList();
                await _gridPatios.Reload();
            }

        }

        private void InitContenedores()
        {
            PeticionesContenedoresClienteExternoDTO cont = new PeticionesContenedoresClienteExternoDTO();
            PeticionesServiciosClienteExternoDTO ser = new PeticionesServiciosClienteExternoDTO();

            cont.Servicios = new List<PeticionesServiciosClienteExternoDTO> {
                new PeticionesServiciosClienteExternoDTO{ NavieraRFC = "HYN1408155916" },
                new PeticionesServiciosClienteExternoDTO{  PatioRFC = "TSH081231SK6"},
            };

            Contenedores.Add(cont);

        }

        // Valida la lista de Contenedores al iniciar el componente
        private void ValidarContenedoresInicial()
        {
            if (Contenedores == null || !Contenedores.Any())
                return;

            foreach (var cont in Contenedores)
            {
                foreach (var servicio in cont.Servicios)
                {
                    if (servicio.PatioRFC?.Equals(_rfcPatioHazesa, StringComparison.OrdinalIgnoreCase) == true)
                        _esPatioHazesa = true;
                    if (servicio.NavieraRFC?.Equals(_rfcHyundai, StringComparison.OrdinalIgnoreCase) == true)
                        _esNavieraHyundai = true;
                }
            }

            if (_esPatioHazesa && _esNavieraHyundai == false)
                BusquedaPatio = "HAZESA";
        }

        // Se invoca cuando cambia la propiedad "PatioSeleccionado" (por el binding)
        private void HandlePatioSelection()
        {

            if (PatioSeleccionado != null)
            {
                var patio = PatioSeleccionado.FirstOrDefault();
                if (patio != null)
                {
                    //_esPatioHazesa = (patio.RFC?.Equals(_rfcPatioHazesa, StringComparison.OrdinalIgnoreCase) == true);

                    if (_esPatioHazesa)
                    {
                        _sizeColumns = 6;
                        BusquedaPatio = "HAZESA";
                    }
                }
            }
            else
            {
                _esPatioHazesa = false;
                _sizeColumns = 12;
            }
        }

        // Filtra la lista de patios según el texto ingresado
        private void FiltrarPatios(string texto)
        {
            _busquedaPatio = texto?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(_busquedaPatio))
            {
                _patios = _patiosOriginales.ToList();
                _sizeColumns = 12;
                _esPatioHazesa = false;
            }
            else
            {
                _patios = _patiosOriginales
                    .Where(p => p.RazonSocial != null &&
                                p.RazonSocial.Contains(_busquedaPatio, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            _gridPatios?.GoToPage(0);
        }

        // Al presionar Aceptar, se devuelve la lista de Contenedores (y aquí podrías asignar valores del patio seleccionado si fuera necesario)
        private void Aceptar()
        {
            if (_patiosSeleccionados.Count() > 0)
            {
                DialogService.Close(_patiosSeleccionados.FirstOrDefault());
            }
        }

        private void Cancelar()
        {
            DialogService.Close(null);
        }

        public void ShowNotification(string titulo, List<string> mensajes, NotificationSeverity notificationSeverity)
        {
            string mensajeHtml = "<ul>";
            foreach (var msg in mensajes)
            {
                mensajeHtml += $"<li>{msg}</li>";
            }
            mensajeHtml += "</ul>";

            NotificationService.Notify(new NotificationMessage
            {
                Severity = notificationSeverity,
                Summary = titulo,
                Detail = mensajeHtml
            });
        }
    }
}