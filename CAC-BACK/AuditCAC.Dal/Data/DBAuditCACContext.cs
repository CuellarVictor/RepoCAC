using AuditCAC.Dal.Entities;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Actas;
using AuditCAC.Domain.Dto.BolsasMedicion;
using AuditCAC.Domain.Dto.CalificacionMasiva;
using AuditCAC.Domain.Dto.CarguePoblacion;
using AuditCAC.Domain.Dto.RegistroAuditoria;
using AuditCAC.Domain.Dto.Variable;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Entities.Cobertura;
using AuditCAC.Domain.Entities.ReglaVariable;
using AuditCAC.Domain.Entities.Usuarios;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AuditCAC.Dal.Data
{
    public class DBAuditCACContext : IdentityDbContext //DbContext
    {
        public DBAuditCACContext()
        {        
        }
        public DBAuditCACContext(DbContextOptions<DBAuditCACContext> options) : base(options)
        {
        }

        public DbSet<Test> Test { get; set; }
        public DbSet<ReglasVariableModel> ReglaVariable { get; set; }
        public DbSet<ItemModel> ItemModel { get; set; }
        public DbSet<CatalogoCoberturaModel> CatalogoCobertura { get; set; }
        public DbSet<CatalogoItemCobertura> CatalogoItemCobertura { get; set; }
        public DbSet<CatalogModel> CatalogModel { get; set; }
        public DbSet<MedicionModel> MedicionModel { get; set; }
        public DbSet<NaturalezaVariableModel> NaturalezaVariableModel { get; set; }
        public DbSet<EstadoVariableModel> EstadoVariableModel { get; set; }
        public DbSet<RegistrosAuditoriaModel> RegistrosAuditoriaModel { get; set; }
        public DbSet<RegistrosAuditoriaLogModel> RegistrosAuditoriaLogModel { get; set; }
        public DbSet<RegistrosAuditoriaDetalleModel> RegistrosAuditoriaDetalleModel { get; set; }
        public DbSet<VariablesModel> VariablesModel { get; set; }
        public DbSet<ProcessModel> ProcesosModel { get; set; }
        public DbSet<BolsasMedicionDto> BolsasMedicion { get; set; }
        public DbSet<CurrentProcessModel> CurrentProcessModel { get; set; }
        public DbSet<CurrentProcessParamModel> CurrentProcessParamModel { get; set; }
        public DbSet<ParametroGeneralModel> ParametroGeneralModel { get; set; }
        public DbSet<CurrentProcessModel> ProccesModel { get; set; }
        public DbSet<ParametroGeneralModel> ParametrosGenerales { get; set; }
        public DbSet<EstadosRegistroAuditoriaModel> EstadosRegistroAuditoria { get; set; }
        public DbSet<InputsRegistroAuditoriaDetalle> InputsRegistroAuditoriaDetalle { get; set; }
        public DbSet<RegistrosAuditoriaDetalleSeguimientoModel> RegistrosAuditoriaDetalleSeguimientoModel { get; set; }
        public DbSet<RegistroAuditoriaDetalleErrorModel> RegistroAuditoriaDetalleErrorModel { get; set; }
        public DbSet<RegistroAuditoriaSoporteModel> RegistroAuditoriaSoporteModel { get; set; }
        public DbSet<BancoInformacionModel> BancoInformacionModel { get; set; }
        public DbSet<RegistroAuditoriaCalificacionesModel> RegistroAuditoriaCalificacionesModel { get; set; }
        public DbSet<UsuarioXEnfermedadModel> GetFiltrosBolsaMedicion { get; set; }
        public DbSet<RolesUsuarioModel> RolesUsuarios { get; set; }
        public DbSet<CantidadRegistrosModel> CantidadRegistros { get; set; }
        public DbSet<VariableXMedicionModel> VariableXMedicionModel { get; set; }
        public DbSet<VariablesXItemsModel> VariablesXItemsModel { get; set; }
        //public DbSet<ReglasVariableModel> ReglasVariableModel { get; set; }
        public DbSet<CoberturaXUsuarioModel> CoberturaXUsuarioModel { get; set; }
        public DbSet<AspNetUsersDetellesModel> AspNetUsersDetellesModel { get; set; }
        public DbSet<HallazgosModel> HallazgosModel { get; set; }


        //DTOs para respuestas custom de metodos.
        public DbSet<ResponseRegistrosAuditoriaFiltrosDto> ResponseRegistrosAuditoriaFiltrosDto { get; set; }
        public DbSet<ResponseRegistrosAuditoriaDetallesAsignacionDto> ResponseRegistrosAuditoriaDetallesAsignacionDto { get; set; }
        public DbSet<ResponseRegistrosAuditoriaProgresoDiarioDto> ResponseRegistrosAuditoriaProgresoDiarioDto { get; set; }
        public DbSet<ResponseRegistrosAuditoriaModelDto> ResponseRegistrosAuditoriaModelDto { get; set; }
        public DbSet<RegistroAuditoriaInfo> RegistroAuditoriaInfo { get; set; }
        public DbSet<LiderIssuesResponseModel> LiderIssuesResponse { get; set; }
        public DbSet<GestionUsuarios> GestionUsuarios { get; set; }
        public DbSet<PanelEnfermedadesMadre> PanelEnfermedadesMadre { get; set; }
        public DbSet<RespuestaString> GetString { get; set; }
        public DbSet<ResponseDetalleConsultarOrderVariablesDto> ResponseDetalleConsultarOrderVariablesDto { get; set; }
        public DbSet<ResponseGetConsultaPerfilAccionDto> ResponseGetConsultaPerfilAccionDto { get; set; }

        public DbSet<ResponseRegistroAuditoriaDetallesDto> ResponseRegistroAuditoriaDetallesDto { get; set; }
        public DbSet<ResponseRegistrosAuditoriaDetalleSeguimientoDto> ResponseRegistrosAuditoriaDetalleSeguimientoDto { get; set; }
        public DbSet<ResponseGetUsuariosByRoleIdDto> ResponseGetUsuariosByRoleIdDto { get; set; }
        public DbSet<ResponseBancoInformacionDto> ResponseBancoInformacionDto { get; set; }
        public DbSet<ResponseRegistroAuditoriaCalificacionesDto> ResponseRegistroAuditoriaCalificacionesDto { get; set; }
        public DbSet<ResponseAlertasRegistrosAuditoriaDto> ResponseAlertasRegistrosAuditoriaDto { get; set; }
        public DbSet<ResponseQueryPaginatorDto> ResponseQueryPaginatorDto { get; set; }
        public DbSet<ResponseGetCalificacionEsCompletasDto> ResponseGetCalificacionEsCompletasDto { get; set; }
        public DbSet<ResponseVariablesDto> ResponseVariablesDto { get; set; }
        public DbSet<ResponseRegistrosAuditoriaAsignadoAuditorEstadoDto> ResponseRegistrosAuditoriaAsignadoAuditorEstadoDto { get; set; }
        public DbSet<ResponseMedicionCrudoDto> ResponseMedicionCrudoDto { get; set; }
        public DbSet<ResponseGetUsuariosConcatByRoleIdDto> ResponseGetUsuariosConcatByRoleIdDto { get; set; }
        public DbSet<ResponseValidacionEstado> ResponseValidacionEstado { get; set; }
        public DbSet<ResponseValidacionEstadoBolsasMedicionDto> ResponseValidacionEstadoBolsasMedicionDto { get; set; }
        public DbSet<ResponseUsuariosBolsaMedicionFiltroDto> ResponseUsuariosBolsaMedicionFiltroDto { get; set; }
        public DbSet<ResponseUsuariosBolsaMedicionDto> ResponseUsuariosBolsaMedicionDto { get; set; }
        public DbSet<ResponseUsuariosLider> ResponseUsuariosLider { get; set; }
        public DbSet<ResponseRegistrosAuditoriaXBolsaMedicionFiltrosDto> ResponseRegistrosAuditoriaXBolsaMedicionFiltrosDto { get; set; }
        public DbSet<ResponseRegistrosAuditoriaXBolsaMedicionDto> ResponseRegistrosAuditoriaXBolsaMedicionDto { get; set; }
        public DbSet<ResponseReasignacionesBolsaDetalladaDto> ResponseReasignacionesBolsaDetalladaDto { get; set; }
        public DbSet<ResponseBolsasMedicionXEnfermedadMadreDto> ResponseBolsasMedicionXEnfermedadMadreDto { get; set; }
        public DbSet<ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto> ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto { get; set; }
        public DbSet<SPResponseDto> SPResponseDto { get; set; }
        //public DbSet<ItemCoberturaModel> ItemCoberturaModel { get; set; }
        public DbSet<ResponseAutocompleteCatalogoCoberturaDto> ResponseAutocompleteCatalogoCoberturaDto { get; set; }
        public DbSet<ResponseCambiarEstadoEntidadDto> ResponseCambiarEstadoEntidadDto { get; set; }
        public DbSet<ResponseConsultarEstadosEntidadDto> ResponseConsultarEstadosEntidadDto { get; set; }
        public DbSet<ResponseCatalogoCoberturaDto> ResponseCatalogoCoberturaDto { get; set; }
        public DbSet<ResponseCatalogoItemCoberturaDto> ResponseCatalogoItemCoberturaDto { get; set; }
        public DbSet<ResponseBancoInformacionFiltradoDto> ResponseBancoInformacionFiltradoDto { get; set; }
        public DbSet<ResponseCargarBancoInformacionPlantillaDto> ResponseCargarBancoInformacionPlantillaDto { get; set; }
        public DbSet<ResponseCargarBancoInformacionDto> ResponseCargarBancoInformacionDto { get; set; }
        public DbSet<InputErroresRegistrosAuditoriaDto> InputErroresRegistrosAuditoriaDto { get; set; }
        public DbSet<ResponseConsultarHallazgosDto> ResponseConsultarHallazgosDto { get; set; }
        public DbSet<ResponseProcessLogFiltradoDto> ResponseProcessLogFiltradoDto { get; set; }
        public DbSet<ResponseConsultaHallazgosGeneradosDto> ResponseConsultarVariablesHallazgosDto { get; set; }
        //public DbSet<RolesModel> RolesModel { get; set; }
        public DbSet<RolesModelDto> RolesModelDto { get; set; }
        public DbSet<ResponseRolesDeleteDto> ResponseRolesDeleteDto { get; set; }
        public DbSet<ResponseConsultaEstructurasCargePoblacionDto> ResponseConsultaEstructurasCargePoblacionDto { get; set; }
        public DbSet<ResponseEliminarRegistrosAuditoriaPlantillaDto> ResponseEliminarRegistrosAuditoriaPlantillaDto { get; set; }
        public DbSet<ResponseConsultaAsignacionLiderEntidadDto> ResponseConsultaAsignacionLiderEntidadDto { get; set; }
        public DbSet<ResponseConsultaEPSCoberturaPeriodoDto> ResponseConsultaEPSCoberturaPeriodoDto { get; set; }        
        //

        //Cargue Poblacion
        public DbSet<ResponseCarguePoblacionDto> ResponseCarguePoblacion { get; set; }
        public DbSet<ControlArchivos_CarguePoblacionModel> ControlArchivos_CarguePoblacion { get; set; }
        public DbSet<ResponseLlaveValor> LLaveValor { get; set; }
        public DbSet<EnfermedadModel> Enfermedad { get; set; }

        public DbSet<ResponseValidacionError> ResponseValidacionError { get; set; }
        public DbSet<VariableCondicionalDto> VariableCondicionalDto { get; set; }

        public DbSet<ResponseItemsCalificacion> ResponseItemsCalificacion { get; set; }

        public DbSet<PermisoRol> PermisoRol { get; set; }
        public DbSet<ResponseRoles> ResponseRoles { get; set; }

        public DbSet<ResponseAutenticacionDto> ResponseAutenticacionDto { get; set; }

        public DbSet<UsuarioDto> UsuarioDto { get; set; }
        public DbSet<UsuarioxEnfermedadDto> UsuarioxEnfermedadDto { get; set; }
        public DbSet<TemplateCalificacionMasivaDto> TemplateCalificacionMasivaDto { get; set; }

        public DbSet<ResultadoCalificacionMasiva> ResultadoCalificacionMasiva { get; set; }
        public DbSet<ParametroTemplateDto> ParametroTemplateDto { get; set; }
        public DbSet<ParametroDto> ParametroDto { get; set; }
        public DbSet<ParametroMedicionDto> ParametroMedicionDto { get; set; }
        public DbSet<ParametroMedicionDetalleDto> ParametroMedicionDetalleDto { get; set; }
        public DbSet<ParametroMedicionInconsistenciaDto> ParametroMedicionInconsistenciaDto { get; set; }
        public DbSet<ConsultaEPSItemDto> ConsultaEPSItemDto { get; set; }

        public DbSet<DataTablaReferencial_RegistroAuditoriaModel> DataTablaReferencial_RegistroAuditoriaModel { get; set; }
        public DbSet<AuditorCoberturaEpsDto> AuditorCoberturaEpsDto { get; set; }


        



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Test>().HasData(new Test
            {
                ID = 1,
                Name = "Test",
            });

            modelBuilder.Entity<GestionUsuarios>(builder => { builder.HasNoKey(); }); 
            modelBuilder.Entity<LiderIssuesResponseModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<EnfermedadModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<RegistrosAuditoriaLogModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ReglasVariableModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<BolsasMedicionDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseGetUsuariosConcatByRoleIdDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<RespuestaString>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<PanelEnfermedadesMadre>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<BolsasMedisionAsignadasModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseValidacionEstadoBolsasMedicionDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseGetConsultaPerfilAccionDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseUsuariosBolsaMedicionFiltroDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseUsuariosBolsaMedicionDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseRegistrosAuditoriaXBolsaMedicionDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseReasignacionesBolsaDetalladaDto>(builder => { builder.HasNoKey(); });            
            modelBuilder.Entity<ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<SPResponseDto>(builder => { builder.HasNoKey(); });
            //modelBuilder.Entity<CustomUsersModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseAutocompleteCatalogoCoberturaDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseCambiarEstadoEntidadDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseConsultarEstadosEntidadDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<HallazgosModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseBancoInformacionFiltradoDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseCargarBancoInformacionPlantillaDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseCargarBancoInformacionDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<InputErroresRegistrosAuditoriaDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseConsultarHallazgosDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseProcessLogFiltradoDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseConsultaHallazgosGeneradosDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseConsultaEstructurasCargePoblacionDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseEliminarRegistrosAuditoriaPlantillaDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseConsultaAsignacionLiderEntidadDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResponseConsultaEPSCoberturaPeriodoDto>(builder => { builder.HasNoKey(); });
            //modelBuilder.Entity<ParametroDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ParametroMedicionDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ParametroMedicionDetalleDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ParametroMedicionInconsistenciaDto>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ConsultaEPSItemDto>(builder => { builder.HasNoKey(); });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(x => x.EnableRetryOnFailure());
            //}
            //
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
    
}
