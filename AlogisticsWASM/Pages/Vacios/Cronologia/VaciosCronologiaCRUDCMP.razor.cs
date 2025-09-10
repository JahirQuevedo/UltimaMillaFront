using ALOG.Modelos.Modelos.Catalogos;
using ALOG.Modelos.Modelos.Peticiones;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ALOGRespositorios.Modelos.Dtos.Control;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace AlogisticsWASM.Pages.Vacios.Cronologia
{
    public partial class VaciosCronologiaCRUDCMP
    {

        [Parameter] public int paramIdContenedor { get; set; }
        [Parameter] public int paramIdServicio { get; set; }
        [Parameter] public Dictionary<string, object> DiccionarioInfoContenedor { get; set; } = new();

        [Inject] IContenedoresCron iContenedoresCron { get; set; }
        [Inject] public ILoginService iLoginService { get; set; }
        [Inject] public SweetAlertService sweetAlertService { get; set; }

        #region VARIABLES
        private IEnumerable<CatTipoIncidenciaCron> listaTiposIncidencia = new List<CatTipoIncidenciaCron>();
        private List<CatTipoEventosCron> listaEventosPorTipo = new();
        private List<PeticionesContenedoresCron> listaCronologia = new();
        private PeticionesContenedoresCron objPeticionesContenedoresCron = new PeticionesContenedoresCron();
        private List<CatTipoIncidenciaEvento> listaTipoIncidenciaEvento = new();
        private UsuarioTokenDTO usuarioToken = new();

        private bool esInterno = false; // true si es ADMIN o ADMINUSER
        private int idIncidenciaSeleccionada;
        private int idEventoSeleccionado;
        private bool mostrarTabla = false;
        private int CaracteresRestantes => 500 - (objPeticionesContenedoresCron?.Comentarios?.Length ?? 0);
        #endregion VARIABLES

        protected override async Task OnInitializedAsync()
        {
            await ObtenerDatosUsuarioAsync();

            listaCronologia = await iContenedoresCron.ObtenerCronologiaPorContenedor(paramIdContenedor, paramIdServicio);
            var usuario = await iLoginService.ObtenerdatosToken();
            if (usuario != null)
            {
                objPeticionesContenedoresCron.IdRegistroUsuario = usuario.IdCatUsuario;
            }

            objPeticionesContenedoresCron.IdContenedor = paramIdContenedor;
            objPeticionesContenedoresCron.IdServicio = paramIdServicio;

            listaTipoIncidenciaEvento = await iContenedoresCron.ObtenerRelacionIncidenciaEvento();

            // De ahí obtienes las incidencias únicas
            listaTiposIncidencia = listaTipoIncidenciaEvento
                .Select(x => x.catTipoIncidenciaCron)
                .DistinctBy(x => x.IdCatTipoIncidenciaCron)
                .ToList();
        }

        private async Task ObtenerDatosUsuarioAsync()
        {
            try
            {
                var token = await JsRuntime.InvokeAsync<string>("localStorage.getItem", "JWT Token");
                if (!string.IsNullOrEmpty(token))
                {
                    usuarioToken = await iLoginService.ObtenerdatosToken();

                    esInterno = usuarioToken.Rol == "ADMIN" || usuarioToken.Rol == "ADMINUSER";
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error al obtener usuario: " + ex.Message);
            }
        }


        private void OnTipoIncidenciaChange(object value)
        {
            idIncidenciaSeleccionada = (int)value;

            listaEventosPorTipo = listaTipoIncidenciaEvento
                .Where(x => x.IdCatTipoIncidenciaCron == idIncidenciaSeleccionada)
                .Select(x => x.catTipoEventosCron)
                .ToList();

            idEventoSeleccionado = 0;
        }

        private void OnTipoEventoChange(object value)
        {
            idEventoSeleccionado = (int)value;

            var relacion = listaTipoIncidenciaEvento
                .FirstOrDefault(x => x.IdCatTipoIncidenciaCron == idIncidenciaSeleccionada && x.IdCatTipoEventoCron == idEventoSeleccionado);

            if (relacion != null)
            {
                objPeticionesContenedoresCron.IdCatTipoIncidenciaEvento = relacion.IdCatTipoIncidenciaEvento;
            }
        }

        private int currentPage = 0;
        private int pageSize = 4;

        private IEnumerable<List<PeticionesContenedoresCron>> Paginas =>
            listaCronologia
                .Select((item, index) => new { item, index })
                .GroupBy(x => x.index / pageSize)
                .Select(g => g.Select(x => x.item).ToList());

        private async Task EliminarCronologia(int idContenedorCron)
        {
            var confirmacion = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "¿Estás seguro?",
                Text = "Esta acción desactivará permanentemente la incidencia.",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Sí, eliminar",
                CancelButtonText = "Cancelar"
            });

            if (!confirmacion.IsConfirmed)
                return;

            var respuesta = await iContenedoresCron.BajaCronologiaporContenedor(idContenedorCron);

            if (respuesta.IsSuccess)
            {
                await sweetAlertService.FireAsync("¡Éxito!", respuesta.strMensaje, SweetAlertIcon.Success);

                // Vuelve a cargar las incidencias
                listaCronologia = await iContenedoresCron.ObtenerCronologiaPorContenedor(paramIdContenedor, paramIdServicio);
                await OnInitializedAsync();
            }
            else
            {
                await sweetAlertService.FireAsync("Error", respuesta.strMensaje, SweetAlertIcon.Error);
            }
        }

        private async Task EditarIncidencia(PeticionesContenedoresCron item)
        {
            var parametros = new Dictionary<string, object>
            {
                { "IncidenciaAEditar", item },
                { "RelacionIncidenciaEvento", listaTipoIncidenciaEvento }
            };

            var result = await DialogService.OpenAsync<VaciosCronologiaCRUDEdit>("Editar Incidencia", parametros);

            if (result is not null && result is bool actualizado && actualizado)
            {
                listaCronologia = await iContenedoresCron.ObtenerCronologiaPorContenedor(paramIdContenedor, paramIdServicio);
            }
        }

        private void CambiarVista()
        {
            mostrarTabla = !mostrarTabla;
        }

        private string TruncarComentario(string comentario)
        {
            if (string.IsNullOrEmpty(comentario))
                return string.Empty;

            return comentario.Length > 150
                ? comentario.Substring(0, 147) + "..."
                : comentario;
        }

        private async Task Exportar()
        {
            var datosExportar = listaCronologia.Select(c => new
            {
                FechaEvento = c.FechaEvento.ToString("dd/MM/yyyy HH:mm"),
                FechaRegistro = c.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                Incidencia = c.catTipoIncidenciaEvento?.catTipoIncidenciaCron?.Nombre,
                Evento = c.catTipoIncidenciaEvento?.catTipoEventosCron?.Nombre,
                Usuario = c.catUsuarios?.Nombre,
                Comentarios = c.Comentarios,
                Contenedor = c.peticionesContenedores?.Contenedor ?? "N/D",
                Servicio = c.peticionesServicios?.catServicios.Nombre ?? "N/D"
            }).ToList();

            var fechaActual = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var nombreArchivo = $"CronologiaContenedor_{fechaActual}.xlsx";

            await ExportarExcelService.ExportarExcelAsync(datosExportar, nombreArchivo);
        }

        private async Task CargarListaCronologia()
        {
            listaCronologia = await iContenedoresCron.ObtenerCronologiaPorContenedor(objPeticionesContenedoresCron.IdContenedor, objPeticionesContenedoresCron.IdServicio);
            StateHasChanged();
        }


        private async Task Submit(PeticionesContenedoresCron args)
        {
            var respuesta = await iContenedoresCron.RegistrarIncidenciaCron(objPeticionesContenedoresCron);

            if (args.FechaEvento > args.FechaRegistro)
            {
                await DialogService.Alert("La Fecha de Evento no puede ser mayor a la Fecha de Registro.", "Error");
                return;
            }

            var guardado = await iContenedoresCron.ActualizarCronologiaporContenedor(args);

            if (respuesta.IsSuccess)
            {
                await sweetAlertService.FireAsync("Incidencia registrada", respuesta.strMensaje, SweetAlertIcon.Success);
                await CargarListaCronologia();
            }
            else
            {

                await sweetAlertService.FireAsync("Error", respuesta.strMensaje, SweetAlertIcon.Error);
            }
        }

    }
}
