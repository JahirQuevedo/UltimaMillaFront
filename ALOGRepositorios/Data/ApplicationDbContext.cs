using ALOGModelos.Modelos.Catalogos;
using ALOGModelos.Modelos.Modelos.Catalogos;
using ALOGModelos.Modelos.Vacios.Peticiones;
using ALOGModelos.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ALOGModelos.Modelos.DTLogistico;
using ALOGModelos.Modelos.Orden;
using Microsoft.Extensions.Configuration;

namespace ALOGRepositorios.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_configuration.GetConnectionString("ConexionSqlIIS"))
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }*/


        //Aquí pasar todas las entidades (Modelos)


        public DbSet<CatAduana> catAduana { get; set; }

        public DbSet<CatClientes> catClientes { get; set; }
        //public DbSet<CatClientesExterno> catClienteExterno { get; set; }



        public DbSet<CatClientesLNegocio> catClientesLNegocio { get; set; }
        public DbSet<CatClientesProyectos> catClientesProyectos { get; set; }
        public DbSet<CatClientesExterno> catClientesExternos { get; set; }
        public DbSet<CatClientesConfig> catClientesConfig { get; set; }
        public DbSet<CatClientesContactos> catClientesContactos { get; set; }
        public DbSet<CatClientesClasificacion> catClientesClasificacion { get; set; }
        public DbSet<CatClientesServicioAduana> catClientesServicioAduana { get; set; }
        public DbSet<CatClienteTarifa> catClienteTarifa { get; set; }
        public DbSet<CatContenedor> catContenedor { get; set; }
        public DbSet<CatDocumentos> catDocumento { get; set; }
        public DbSet<CatEmpresas> catEmpresas { get; set; }
        public DbSet<CatFormatoReferencias> catFormatoReferencias { get; set; }
        public DbSet<CatFormatoRefDet> catFormatoRefDets { get; set; }
        public DbSet<CatLineaNegocio> catLineaNegocio { get; set; }
        public DbSet<CatLineaNegocioTarifa> catLineaNegocioTarifas { get; set; }
        public DbSet<CatLineaNegocioTariPrecio> catLineaNegocioTariPrecios { get; set; }
        public DbSet<CatNavieras> catNavieras { get; set; }
        public DbSet<CatPaises> catPaises { get; set; }
        public DbSet<CatPaisCP> catPaisCP { get; set; }
        public DbSet<CatPaisEstados> catPaisEstados { get; set; }
        public DbSet<CatPaisMunicipios> catPaisMunicipios { get; set; }
        public DbSet<CatPatios> catPatios { get; set; }
        public DbSet<CatPatiosConfig> catPatiosConfig { get; set; }
        public DbSet<CatPatiosNavieras> catPatiosNavieras { get; set; }
        public DbSet<CatPermisos> catPermisos { get; set; }
        public DbSet<CatProveedores> catProveedores { get; set; }
        public DbSet<CatProveedoresConfig> catProveedoresConfigs { get; set; }
        public DbSet<CatProveedoresClasif> catProveedoresClasif { get; set; }
        public DbSet<CatProveedoresContactos> catProveedoresContactos { get; set; }
        public DbSet<CatProveedoresTarifaPatio> catProveedoresTarifaPatios { get; set; }
        public DbSet<CatProveedoresPatios> catProveedoresPatios { get; set; }
        public DbSet<CatProveedoresTarifas> catProveedoresTarifas { get; set; }
        //public DbSet<CatProveedorTarifaAduana> catProveedorTarifaAduana { get; set; }
        public DbSet<CatProyectos> catProyectos { get; set; }
        public DbSet<CatReferenciaEstado> catReferenciaEstado { get; set; }
        public DbSet<CatRoles> catRoles { get; set; }
        public DbSet<CatRolesPermisos> catRolesPermisos { get; set; }
        public DbSet<CatRecintos> catRecintos { get; set; }
        public DbSet<CatServicios> catServicios { get; set; }

        public DbSet<CatSistemas> catSistemas { get; set; }

        public DbSet<CatSucursales> catSucursales { get; set; }
        //public DbSet<CatTarifas> catTarifas { get; set; }
        public DbSet<CatTipoEstados> catTipoEstados { get; set; }
        public DbSet<CatTipoClasificacion> catTipoClasificacion { get; set; }
        public DbSet<CatTipoContacto> catTipoContactos { get; set; }
        public DbSet<CatTiposConfig> catTiposConfig { get; set; }
        public DbSet<CatTipoPuesto> catTipoPuesto { get; set; }
        public DbSet<CatTransportistas> catTransportistas { get; set; }
        public DbSet<CatUsuarioRoles> catUsuarioRoles { get; set; }
        public DbSet<CatUsuarios> catUsuarios { get; set; }
        public DbSet<CatUsuariosEmpresa> catUsuariosEmpresa { get; set; }
        public DbSet<CatUsuariosPermisos> catUsuariosPermisos { get; set; }
        public DbSet<CatUsuariosAduanas> catUsuariosAduanas { get; set; }
        public DbSet<CatSeriesFacturas> catSeriesFacturas { get; set; }


        public DbSet<Ordenes> ordenes { get; set; }

        //public DbSet<Servicios> servicios { get; set; }


        public DbSet<PeticionesContenedor> peticionesContenedores { get; set; }
        public DbSet<PeticionesDocumentos> peticionesDocumentos { get; set; }
        public DbSet<PeticionesReferencias> peticionesReferencias { get; set; }
        public DbSet<PeticionesServicios> peticionesServicios { get; set; }

        //public DbSet<AnticiposEnc> anticiposEnc { get; set; }
        //public DbSet<AnticiposDet> anticiposDet { get; set; }
        //public DbSet<PrefacturaEnc> prefacturaEnc { get; set; }
        //public DbSet<PrefacturaDet> prefacturaDet { get; set; }

        //INTEGRACION 1G

        //public DbSet<IntegraAnticipoSol> integracionAnticipoSol { get; set; }
        //public DbSet<IntegraConceptosFactura> integracionConceptosFactura { get; set; }
        //public DbSet<IntegraFacturaDet> integracionFacturaDet { get; set; }
        //public DbSet<IntegraFacturaEnc> integracionFacturaEnc { get; set; }
        //public DbSet<IntegraFacturaEst> integracionFacturaEst { get; set; }
        //public DbSet<IntegraReferencia> integracionReferencia { get; set; }

        //DTLogisticos
        public DbSet<DtAcarreos> dtAcarreos { get; set; }
        public DbSet<DtUltimaMillaEnc> dtUltimaMillaEnc { get; set; }
        public DbSet<DtUltimaMillaDet> dtUltimaMillaDet { get; set; }

        // Provisiones
        //public DbSet<ProvisionEnc> provisionesEnc { get; set; }
        //public DbSet<ProvisionDet> provisionesDet { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<PeticionesReferencias>()
                .Property(u => u.Name)
                .IsRequired(); // Esto configura la propiedad como NOT NULL

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();*/

            modelBuilder.Entity<PeticionesReferencias>()
           .HasIndex(u => u.Ticket)
           .IsUnique();

            /* modelBuilder.Entity<PeticionesContenedores>()
            .HasIndex(u => new { u.Contenedor, u.RefenciaCliente, u.IdReferencia })
            .IsUnique();*/

            modelBuilder.Entity<PeticionesServicios>()
           .HasIndex(u => new { u.IdServicio, u.IdContenedor, u.IdTipoServicio })
           .IsUnique();

            modelBuilder.Entity<CatClientesServicioAduana>()
                .HasIndex(u => new { u.IdCatClientes, u.IdCatServicio, u.IdCatAduana })
                .IsUnique();

            modelBuilder.Entity<CatClientesLNegocio>()
                .HasIndex(u => new { u.IdCliente, u.IdLineaNegocio })
                .IsUnique();

            modelBuilder.Entity<CatClientes>()
               .HasIndex(u => new { u.RFC })
               .IsUnique();

            modelBuilder.Entity<CatEmpresas>()
              .HasIndex(u => new { u.RFC })
              .IsUnique();

            modelBuilder.Entity<CatNavieras>()
              .HasIndex(u => new { u.RFC })
              .IsUnique();

            modelBuilder.Entity<CatProveedores>()
              .HasIndex(u => new { u.RFC })
              .IsUnique();

            //Inicio Clientes
            // modelBuilder.Entity<CatClientesExterno>()
            //.HasIndex(u => new { u.IdCatCliente, u.IdCatClientesAsociado })
            //.IsUnique();

            modelBuilder.Entity<CatClientesConfig>()
          .HasIndex(u => new { u.IdCatClientes, u.IdCatTipoConfig })
          .IsUnique();

            modelBuilder.Entity<CatClientesLNegocio>()
        .HasIndex(u => new { u.IdCliente, u.IdLineaNegocio })
        .IsUnique();

            modelBuilder.Entity<CatClientesProyectos>()
       .HasIndex(u => new { u.IdCatCliente, u.IdCatProyecto })
       .IsUnique();

            modelBuilder.Entity<CatClientesServicioAduana>()
       .HasIndex(u => new { u.IdCatClientes, u.IdCatAduana, u.IdCatServicio })
       .IsUnique();

            modelBuilder.Entity<CatClientesLNegocio>()
       .HasIndex(u => new { u.IdCliente, u.IdLineaNegocio })
       .IsUnique();

            modelBuilder.Entity<CatClienteTarifa>()
      .HasIndex(u => new { u.IdCatCteServAduana, u.IdCatContenedor })
      .IsUnique();


            modelBuilder.Entity<CatFormatoReferencias>()
      .HasIndex(u => new { u.IdCatLineaNegocio })
      .IsUnique();

            modelBuilder.Entity<CatLineaNegocio>()
   .HasIndex(u => new { u.Acronimo })
   .IsUnique();
            modelBuilder.Entity<CatLineaNegocio>()
  .HasIndex(u => new { u.Nombre })
  .IsUnique();

            modelBuilder.Entity<CatLineaNegocioTarifa>()
  .HasIndex(u => new { u.IdCatServicio, u.IdCatLineaNegocio })
  .IsUnique();
            modelBuilder.Entity<CatLineaNegocioTariPrecio>()
.HasIndex(u => new { u.IdCatLineaNegocioTarifa, u.IdCatAduana })
.IsUnique();

            modelBuilder.Entity<CatNavieras>()
.HasIndex(u => new { u.Acronimo })
.IsUnique();
            modelBuilder.Entity<CatNavieras>()
.HasIndex(u => new { u.RazonSocial })
.IsUnique();
            modelBuilder.Entity<CatNavieras>()
.HasIndex(u => new { u.RFC })
.IsUnique();

            modelBuilder.Entity<CatPaises>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatPaisEstados>()
.HasIndex(u => new { u.IdCatPais, u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatPatios>()
.HasIndex(u => new { u.RazonSocial })
.IsUnique();

            modelBuilder.Entity<CatPatiosConfig>()
.HasIndex(u => new { u.IdCatPatios, u.IdCatPatiosConfig })
.IsUnique();

            modelBuilder.Entity<CatPermisos>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatProveedores>()
.HasIndex(u => new { u.RazonSocial })
.IsUnique();
            modelBuilder.Entity<CatProveedores>()
.HasIndex(u => new { u.RFC })
.IsUnique();

            modelBuilder.Entity<CatProveedores>()
.HasIndex(u => new { u.Clave1G })
.IsUnique();
            modelBuilder.Entity<CatProveedores>()
.HasIndex(u => new { u.Acronimo })
.IsUnique();

            modelBuilder.Entity<CatProveedoresConfig>()
.HasIndex(u => new { u.IdCatProveedor, u.IdCatTipoConfig })
.IsUnique();


            modelBuilder.Entity<CatProveedoresTarifaPatio>()
.HasIndex(u => new { u.IdCatProvTarifas, u.IdCatPatio })
.IsUnique();

            modelBuilder.Entity<CatProveedoresTarifas>()
.HasIndex(u => new { u.IdCatProvTarifas, u.IdCatServicio, u.IdCatAduana })
.IsUnique();

            modelBuilder.Entity<CatProyectos>()
.HasIndex(u => new { u.Nombre })
.IsUnique();
            modelBuilder.Entity<CatProyectos>()
.HasIndex(u => new { u.Acronimo })
.IsUnique();

            modelBuilder.Entity<CatReferenciaEstado>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatRoles>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatRolesPermisos>()
.HasIndex(u => new { u.IdCatRoles, u.IdCatPermisos })
.IsUnique();

            modelBuilder.Entity<CatServicios>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatSistemas>()
.HasIndex(u => new { u.userSistema })
.IsUnique();

            modelBuilder.Entity<CatSistemas>()
.HasIndex(u => new { u.nombre })
.IsUnique();

            modelBuilder.Entity<CatSucursales>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatSucursales>()
.HasIndex(u => new { u.RFC, u.IdCatEmpresas })
.IsUnique();

            modelBuilder.Entity<CatTipoEstados>()
.HasIndex(u => new { u.Nombre })
.IsUnique();
            modelBuilder.Entity<CatTipoEstados>()
.HasIndex(u => new { u.TipoEstado })
.IsUnique();

            modelBuilder.Entity<CatTiposConfig>()
.HasIndex(u => new { u.Nombre })
.IsUnique();
            modelBuilder.Entity<CatTiposConfig>()
.HasIndex(u => new { u.Clave })
.IsUnique();

            modelBuilder.Entity<CatTransportistas>()
.HasIndex(u => new { u.RazonSocial, u.IdCatEmpresas })
.IsUnique();
            modelBuilder.Entity<CatTransportistas>()
.HasIndex(u => new { u.RFC, u.IdCatEmpresas })
.IsUnique();
            modelBuilder.Entity<CatUsuarioRoles>()
                .HasIndex(u => new { u.IdCatUsuarios, u.IdCatRoles })
                .IsUnique();

            modelBuilder.Entity<CatUsuarios>()
             .HasIndex(u => new { u.Usuario })
             .IsUnique();

            modelBuilder.Entity<CatUsuarios>()
             .HasIndex(u => new { u.Nombre, u.ApellidoPaterno, u.ApellidoMaterno })
             .IsUnique();

            modelBuilder.Entity<CatUsuarios>()
            .HasIndex(u => new { u.RFC })
            .IsUnique();

            modelBuilder.Entity<CatUsuariosEmpresa>()
         .HasIndex(u => new { u.IdCatCliente, u.IdCatUsuarios })
         .IsUnique();

            modelBuilder.Entity<CatUsuariosPermisos>()
       .HasIndex(u => new { u.IdCatPermisos, u.IdCatUsuarios })
       .IsUnique(); modelBuilder.Entity<PeticionesReferencias>()
           .HasIndex(u => u.Ticket)
            .IsUnique();

            modelBuilder.Entity<PeticionesContenedor>()
           .HasIndex(u => new { u.Contenedor, u.RefenciaCliente, u.IdReferencia })
           .IsUnique();

            modelBuilder.Entity<PeticionesServicios>()
           .HasIndex(u => new { u.IdContenedor, u.IdTipoServicio })
           .IsUnique();

            modelBuilder.Entity<CatClientesServicioAduana>()
                .HasIndex(u => new { u.IdCatClientes, u.IdCatServicio, u.IdCatAduana })
                .IsUnique();

            modelBuilder.Entity<CatClientesLNegocio>()
                .HasIndex(u => new { u.IdCliente, u.IdLineaNegocio })
                .IsUnique();

            modelBuilder.Entity<CatClientes>()
               .HasIndex(u => new { u.RFC })
               .IsUnique();

            modelBuilder.Entity<CatEmpresas>()
              .HasIndex(u => new { u.RFC })
              .IsUnique();

            modelBuilder.Entity<CatNavieras>()
              .HasIndex(u => new { u.RFC })
              .IsUnique();

            modelBuilder.Entity<CatProveedores>()
              .HasIndex(u => new { u.RFC })
              .IsUnique();

            //Inicio Clientes


            modelBuilder.Entity<CatClientesConfig>()
          .HasIndex(u => new { u.IdCatClientes, u.IdCatTipoConfig })
          .IsUnique();

            modelBuilder.Entity<CatClientesLNegocio>()
        .HasIndex(u => new { u.IdCliente, u.IdLineaNegocio })
        .IsUnique();

            modelBuilder.Entity<CatClientesProyectos>()
       .HasIndex(u => new { u.IdCatCliente, u.IdCatProyecto })
       .IsUnique();

            modelBuilder.Entity<CatClientesServicioAduana>()
       .HasIndex(u => new { u.IdCatClientes, u.IdCatAduana, u.IdCatServicio })
       .IsUnique();

            modelBuilder.Entity<CatClientesLNegocio>()
       .HasIndex(u => new { u.IdCliente, u.IdLineaNegocio })
       .IsUnique();

            modelBuilder.Entity<CatClienteTarifa>()
      .HasIndex(u => new { u.IdCatCteServAduana, u.IdCatContenedor })
      .IsUnique();


            modelBuilder.Entity<CatFormatoReferencias>()
      .HasIndex(u => new { u.IdCatLineaNegocio })
      .IsUnique();

            modelBuilder.Entity<CatLineaNegocio>()
   .HasIndex(u => new { u.Acronimo })
   .IsUnique();
            modelBuilder.Entity<CatLineaNegocio>()
  .HasIndex(u => new { u.Nombre })
  .IsUnique();

            modelBuilder.Entity<CatLineaNegocioTarifa>()
  .HasIndex(u => new { u.IdCatServicio, u.IdCatLineaNegocio })
  .IsUnique();


            modelBuilder.Entity<CatNavieras>()
.HasIndex(u => new { u.Acronimo })
.IsUnique();
            modelBuilder.Entity<CatNavieras>()
.HasIndex(u => new { u.RazonSocial })
.IsUnique();
            modelBuilder.Entity<CatNavieras>()
.HasIndex(u => new { u.RFC })
.IsUnique();

            modelBuilder.Entity<CatPaises>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatPaisEstados>()
.HasIndex(u => new { u.IdCatPais, u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatPatios>()
.HasIndex(u => new { u.RazonSocial })
.IsUnique();

            modelBuilder.Entity<CatPatiosConfig>()
.HasIndex(u => new { u.IdCatPatios, u.IdCatPatiosConfig })
.IsUnique();

            modelBuilder.Entity<CatPermisos>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatProveedores>()
.HasIndex(u => new { u.RazonSocial })
.IsUnique();
            modelBuilder.Entity<CatProveedores>()
.HasIndex(u => new { u.RFC })
.IsUnique();

            modelBuilder.Entity<CatProveedores>()
.HasIndex(u => new { u.Clave1G })
.IsUnique();
            modelBuilder.Entity<CatProveedores>()
.HasIndex(u => new { u.Acronimo })
.IsUnique();

            modelBuilder.Entity<CatProveedoresConfig>()
.HasIndex(u => new { u.IdCatProveedor, u.IdCatTipoConfig })
.IsUnique();


            modelBuilder.Entity<CatProveedoresTarifaPatio>()
.HasIndex(u => new { u.IdCatProvTarifas, u.IdCatPatio })
.IsUnique();

            modelBuilder.Entity<CatProveedoresTarifas>()
.HasIndex(u => new { u.IdCatProvTarifas, u.IdCatServicio, u.IdCatAduana })
.IsUnique();

            modelBuilder.Entity<CatProyectos>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatReferenciaEstado>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatRoles>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatRolesPermisos>()
.HasIndex(u => new { u.IdCatRoles, u.IdCatPermisos })
.IsUnique();

            modelBuilder.Entity<CatServicios>()
.HasIndex(u => new { u.Nombre })
.IsUnique();

            modelBuilder.Entity<CatSistemas>()
.HasIndex(u => new { u.userSistema })
.IsUnique();

            modelBuilder.Entity<CatSistemas>()
.HasIndex(u => new { u.nombre })
.IsUnique();

            modelBuilder.Entity<CatSucursales>()
            .HasIndex(u => new { u.Nombre })
            .IsUnique();

            modelBuilder.Entity<CatSucursales>()
            .HasIndex(u => new { u.RFC, u.IdCatEmpresas })
            .IsUnique();

            modelBuilder.Entity<CatTipoEstados>()
            .HasIndex(u => new { u.Nombre })
            .IsUnique();

            modelBuilder.Entity<CatTipoEstados>()
            .HasIndex(u => new { u.TipoEstado })
            .IsUnique();

            modelBuilder.Entity<CatTiposConfig>()
            .HasIndex(u => new { u.Nombre })
            .IsUnique();

            modelBuilder.Entity<CatTiposConfig>()
            .HasIndex(u => new { u.Clave })
            .IsUnique();

            modelBuilder.Entity<CatTransportistas>()
            .HasIndex(u => new { u.RazonSocial, u.IdCatEmpresas })
            .IsUnique();

            modelBuilder.Entity<CatTransportistas>()
            .HasIndex(u => new { u.RFC, u.IdCatEmpresas })
            .IsUnique();

            modelBuilder.Entity<CatUsuarioRoles>()
                .HasIndex(u => new { u.IdCatUsuarios, u.IdCatRoles })
                .IsUnique();

            modelBuilder.Entity<CatUsuarios>()
             .HasIndex(u => new { u.Usuario })
             .IsUnique();

            modelBuilder.Entity<CatUsuarios>()
             .HasIndex(u => new { u.Nombre, u.ApellidoPaterno, u.ApellidoMaterno })
             .IsUnique();

            modelBuilder.Entity<CatUsuarios>()
            .HasIndex(u => new { u.RFC })
            .IsUnique();

            modelBuilder.Entity<CatUsuariosEmpresa>()
         .HasIndex(u => new { u.IdCatCliente, u.IdCatUsuarios })
         .IsUnique();

            modelBuilder.Entity<CatUsuariosPermisos>()
       .HasIndex(u => new { u.IdCatPermisos, u.IdCatUsuarios })
       .IsUnique();

            modelBuilder.Entity<CatTipoClasificacion>()
      .HasIndex(u => new { u.Nombre })
      .IsUnique();
            modelBuilder.Entity<CatTipoContacto>()
      .HasIndex(u => new { u.Nombre })
      .IsUnique();

            modelBuilder.Entity<CatRecintos>()
      .HasIndex(u => new { u.Nombre })
      .IsUnique();
            modelBuilder.Entity<CatRecintos>()
    .HasIndex(u => new { u.ClaveRecinto })
    .IsUnique();
            modelBuilder.Entity<CatProveedoresClasif>()
    .HasIndex(u => new { u.IdCatProveedor, u.IdCatTipoClasificacion })
    .IsUnique();
            modelBuilder.Entity<CatProveedoresContactos>()
    .HasIndex(u => new { u.IdCatProveedor, u.IdCatTipoContacto })
    .IsUnique();

            modelBuilder.Entity<CatClientesClasificacion>()
 .HasIndex(u => new { u.IdCatclientes, u.IdCatClasificacion })
 .IsUnique();
            modelBuilder.Entity<CatProveedoresContactos>()
    .HasIndex(u => new { u.IdCatProveedor, u.IdCatTipoContacto })
    .IsUnique();


            //            //Integración
            //            modelBuilder.Entity<IntegraAnticipoSol>()
            //  .HasIndex(u => new { u.IdOrden, u.IdPeticionesReferencia, u.IdPeticionesContenedor })
            //  .IsUnique();

            //            modelBuilder.Entity<IntegraFacturaDet>()
            //.HasIndex(u => new { u.IdPeticionesReferencia, u.IdPeticionesContenedor })
            //.IsUnique();

            //            modelBuilder.Entity<IntegraReferencia>()
            // .HasIndex(u => new { u.IdOrden })
            // .IsUnique();

            //            modelBuilder.Entity<IntegraFacturaEnc>()
            //.HasIndex(u => new { u.IdOrden, u.IdPeticionesReferencia })
            //.IsUnique();

            modelBuilder.Entity<CatUsuariosAduanas>()
.HasIndex(u => new { u.IdCatUsuario, u.IdCatAduana })
.IsUnique();

            modelBuilder.Entity<CatClientesExterno>()
.HasIndex(u => new { u.IdCatCliente, u.IdCatClientesAsociado })
.IsUnique();















        }






    }
}
