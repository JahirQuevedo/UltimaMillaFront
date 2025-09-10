using AlogisticsWASM;
using ALOGRepositorios;
using ALOGRepositorios.Helpers;
using ALOGRepositorios.Services;
using ALOGRepositorios.Services.Autenticacion;
using ALOGRepositorios.Services.Catalogos;
using ALOGRepositorios.Services.Catalogos.ICatalogos;
using ALOGRepositorios.Services.Control;
using ALOGRepositorios.Services.Control.IControl;
using ALOGRepositorios.Services.IServices;
using ALOGRepositorios.Services.Login;
using ALOGRepositorios.Services.Login.ILogin;
using ALOGRepositorios.Services.Logisticos;
using ALOGRepositorios.Services.Logisticos.ILogisticos;
using ALOGRepositorios.Services.Operativo;
using ALOGRepositorios.Services.Peticiones;
using ALOGRepositorios.Services.Peticiones.IPeticiones;
using ALOGRepositorios.Services.Utilerias;
using ALOGRepositorios.Services.Utilerias.IUtilerias;
using ALOGRespositorios.Modelos.Dtos.Control;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
// Cargar configuración desde appsettings.json


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IOrdenService, OrdenService>();
builder.Services.AddScoped<ICatPaisesServices, CatPaisesServices>();
builder.Services.AddScoped<ICatClientesService, CatClientesService>();
builder.Services.AddScoped<IReferenciaService, ReferenciaService>();
builder.Services.AddScoped<IContenedorService, ContenedorService>();
builder.Services.AddScoped<IPatiosService, PatiosService>();
builder.Services.AddScoped<ICatNavieraService, CatNavieraService>();
builder.Services.AddScoped<ICatServicioService, CatServicioService>();
builder.Services.AddScoped<ICatTransportistaService, CatTransportistaService>();
builder.Services.AddScoped<ICatClientesService, CatClientesService>();
builder.Services.AddScoped<IContenedorService, ContenedorService>();
builder.Services.AddScoped<IOrdenService, OrdenService>();
builder.Services.AddScoped<ICatTipoContenedorService, CatTipoContenedorService>();
builder.Services.AddScoped<IAcarreosService, AcarreosService>();
builder.Services.AddScoped<ISLOReferenciasService, SLOReferenciasService>();
builder.Services.AddScoped<IUltimaMillaEncabezadoService, UltimaMillaEncabezadoService>();
builder.Services.AddScoped<IUltimaMillaDetalleService, UltimaMillaDetalleService>();
builder.Services.AddScoped<ICatTipoEstadoService, CatTipoEstadoService>();
builder.Services.AddScoped<ICatPatiosServices, CatPatiosService>();
builder.Services.AddScoped<ICatAduanaService, CatAduanaService>();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddScoped<IContenedoresCron, ContenedoresCron>();
builder.Services.AddScoped<IExportarExcelService, ExportarExcelService>();
builder.Services.AddScoped<ICatTipoTransporteService, CatTipoTransporteService>();
builder.Services.AddScoped<ICatMercanciasService, CatMercanciasService>();
builder.Services.AddScoped<ICatClientesUbicacionesService, CatClientesUbicacionesService>();
builder.Services.AddScoped<ICatTipoCargaService, CatTipoCargaService>();
builder.Services.AddScoped<ICatTipoOperacionesComercioService, CatTipoOperacionesComercioService>();
builder.Services.AddScoped<ICatTipoIMOService, CatTipoIMOService>();
builder.Services.AddScoped<ICatTransportistaService, CatTransportistaService>();
builder.Services.AddScoped<ISLOTransporteSolicitudService, SLOTransporteSolicitudService>();
builder.Services.AddScoped<ICatTipoEventosCronService, CatTipoEventosCronService>();
builder.Services.AddScoped<ISLOTransporteCronService, SLOTransporteCronService>();
builder.Services.AddScoped<ICatPaisesService, CatPaisesService>();
builder.Services.AddScoped<ICatPaisEstadosService, CatPaisEstadosService>();
builder.Services.AddScoped<ICatPaisMunicipiosService, CatPaisMunicipiosService>();
builder.Services.AddScoped<ISLODocumentosService, SLODocumentosService>();


// WMS Alogistic
builder.Services.AddScoped<IReporteInventarioService, ReporteInventarioService>();
builder.Services.AddScoped<IReporteLiberacionService, ReporteLiberacionService>();
builder.Services.AddScoped<ILiberacionService, LiberacionService>();
builder.Services.AddScoped<IReferenciaBookingBLService, ReferenciaBookingBLService>();
builder.Services.AddScoped<IInventarioAlmacenService, InventarioAlmacenService>();
builder.Services.AddScoped<IViajeService, ViajeService>();
builder.Services.AddScoped<IBarcoService, BarcoService>();
builder.Services.AddScoped<IPaqueteService, PaqueteService>();
builder.Services.AddScoped<IReferenciaWMSService, ReferenciaWMSService>();
builder.Services.AddScoped<ITarjaService, TarjaService>();
builder.Services.AddScoped<IPartidaService, PartidaService>();
builder.Services.AddScoped<IUbicacionAlmacenService, UbicacionAlmacenService>();
builder.Services.AddScoped<IZonaAlmacenService, ZonaAlmacenService>();
builder.Services.AddScoped<ITipoTransporteService, TipoTransporteServie>();
builder.Services.AddScoped<ILineaTransporteService, LineaTransporteService>();
builder.Services.AddScoped<ILineaOperadorService, LineaOperadorService>();
builder.Services.AddScoped<IManiobristaService, ManiobristaService>();
builder.Services.AddScoped<IControlTransporteService, ControlTransporteService>();
builder.Services.AddScoped<IFolioServicioService, FolioServicioService>();
builder.Services.AddScoped<ISolicitudTrasladoService, SolicitudTrasladoService>();
builder.Services.AddScoped<ISolicitudIngresoService, SolicitudIngresoService>();
builder.Services.AddScoped<IRecepcionService, RecepcionService>();
builder.Services.AddScoped<IServicioFotograficoService, ServicioFotograficoService>();
builder.Services.AddScoped<IReporteTarjaServicie, ReporteTarjaService>();
builder.Services.AddScoped<ICodigoDesperfectoService, CodigoDesperfectoService>();
builder.Services.AddScoped<ITipoDesperfectoService, TipoDesperfectoService>();
builder.Services.AddScoped<ITipoSeveridadService, TipoSeveridadService>();
builder.Services.AddScoped<IBitacoraAveriaService, BitacoraAveriaService>();
builder.Services.AddScoped<IReferenciaBookingBLService, ReferenciaBookingBLService>();
builder.Services.AddTransient<ExcelService>();
builder.Services.AddTransient<ContenedorValidadorService>();
builder.Services.AddTransient<ServicioValidadorService>();
builder.Services.AddScoped<IDocumentoService, DocumentoService>();
builder.Services.AddScoped<ICatDocumentoService, CatDocumentoService>();
builder.Services.AddScoped<ISLOSolicitudesService, SLOSolicitudesService>();
builder.Services.AddScoped<ICatTipoOperacionService, CatTipoOperacionService>();
builder.Services.AddScoped<ISLOTransporteAsignadoService, SLOTransporteAsignadoService>();
builder.Services.AddScoped<ISLOTControlTerrestreService, SLOTControlTerrestreService>();
builder.Services.AddScoped<ISLOTransporteDetalleService, SLOTransporteDetalleService>();


builder.Services.AddScoped<ICatProveedorService, CatProveedorService>();

builder.Services.AddScoped<ICatTipoMonedaService, CatTipoMonedaService>();
builder.Services.AddScoped<IControlService, ControlService>();


builder.Services.AddSweetAlert2();


//var host = builder.Build();

//// Inicializa el UserService antes de renderizar la app
//var userService = host.Services.GetRequiredService<UserService>();
//await userService.InitializeAsync();

// Registrar userData como un servicio singleton

builder.Services.AddSingleton<IUsuarioTokenService, UsuarioTokenService>();
builder.Services.AddOptions();
builder.Services.AddSingleton<UsuarioTokenDTO>();
builder.Services.AddSingleton(async provider =>
{
    var getdatos = provider.GetRequiredService<IUsuarioTokenService>().getUsuarioTokenService();

    return getdatos;

});


builder.Services.AddAuthorizationCore();


//Para usar el Local Storage del navegador
builder.Services.AddBlazoredLocalStorage();


//Agregar para la autenticación y autorización

builder.Services.AddAuthorizationCore();



builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    s => s.GetRequiredService<AuthStateProvider>());

builder.Services.AddRadzenComponents();

builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "humanistic"; // The name of the cookie
    options.Duration = TimeSpan.FromDays(365); // The duration of the cookie
});



await builder.Build().RunAsync();
