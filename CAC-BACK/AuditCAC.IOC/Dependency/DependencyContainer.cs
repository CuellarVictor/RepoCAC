using AuditCAC.Dal.Entities;
using AuditCAC.Domain;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Entities.Cobertura;
using AuditCAC.MainCore.Module;
using AuditCAC.MainCore.Module.BolsasDeMedicion;
using AuditCAC.MainCore.Module.BolsasDeMedicion.Interface;
using AuditCAC.MainCore.Module.Catalogo;
using AuditCAC.MainCore.Module.Catalogo.Interfaces;
using AuditCAC.MainCore.Module.Helpers;
using AuditCAC.MainCore.Module.Interface;
using AuditCAC.MainCore.Module.ItemCobertura;
using AuditCAC.MainCore.Module.ItemCobertura.Interface;
using AuditCAC.MainCore.Module.Lider;
using AuditCAC.MainCore.Module.Lider.Interfaces;
using AuditCAC.MainCore.Module.Trazabilidad;
using AuditCAC.MainCore.Module.Trazabilidad.Interfaces;
using AuditCAC.MainCore.Module.UserManagement;
using AuditCAC.MainCore.Module.Users.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AuditCAC.IOC.Dependency
{
    public  class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services) 
        {
            #region Services
            // Dependency Injection
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IDataRepository<Test>, TestManager>();
            //services.AddScoped<IProceduresRepository<CoberturaModel>, ProcedureManager>();
            services.AddScoped<IMedicionRepository<MedicionModel>, MedicionManager>();
            services.AddScoped<INaturalezaVariableRepository<NaturalezaVariableModel>, NaturalezaVariableManager>();
            services.AddScoped<IEstadoVariableRepository<EstadoVariableModel>, EstadoVariableManager>();
            services.AddScoped<IRegistrosAuditoriaRepository<RegistrosAuditoriaModel>, RegistrosAuditoriaManager>();
            services.AddScoped<IRegistrosAuditoriaDetalleRepository<RegistrosAuditoriaDetalleModel>, RegistrosAuditoriaDetalleManager>();
            services.AddScoped<IRegistrosAuditoriaDetalleSeguimientoRepository<RegistrosAuditoriaDetalleSeguimientoModel>, RegistrosAuditoriaDetalleSeguimientoManager>();
            services.AddScoped<IEstadosRegistroAuditoriaRepository<EstadosRegistroAuditoriaModel>, EstadosRegistroAuditoriaManager>();
            services.AddScoped<IVariablesRepository<VariablesModel>, VariablesManager>();
            services.AddScoped<IItemRepository<ItemModel>, ItemManager>();
            services.AddScoped<IGenerateExcel, GenerateExcel>();
            services.AddScoped<IBolsasMedicionManager, BolsasMedicionManager>();
            services.AddScoped<IProcesosRepository<ProcessModel>, ProcesosManager>();
            services.AddScoped<IBancoInformacionRepository<BancoInformacionModel>, BancoInformacionManager>();
            services.AddScoped<IRegistroAuditoriaCalificacionesRepository<RegistroAuditoriaCalificacionesModel>, RegistroAuditoriaCalificacionesManager>();
            services.AddScoped<IVariableXMedicionRepository<VariableXMedicionModel>, VariableXMedicionManager>();
            services.AddScoped<ICoberturaRepository<CoberturaXUsuarioModel>, MainCore.Module.Cobertura.CoberturaManager>();
            //services.AddScoped<IItemCoberturaRepository<ItemCoberturaModel>, ItemCoberturaManager>();
            services.AddScoped<IItemCoberturaRepository<CatalogoItemCobertura>, ItemCoberturaManager>();
            services.AddScoped<IHallazgosRepository<HallazgosModel>, HallazgosManager>();
            //services.AddScoped<IRolesRepository<RolesModel>, RolesManager>();
            //
            services.AddScoped<ICatalogoCoberturaManager, CatalogoCoberturaManager>();
            services.AddScoped<ILiderManager, LiderManager>();
            services.AddScoped<IRegistroAuditoriaLogManager, RegistroAuditoriaLogManager>();
            services.AddScoped<IUserManagement, UserManagement>();
            services.AddScoped<ICatalogoManager, CatalogoManager>();
            services.AddScoped<IItemCatalogoManager, ItemCatalogoManager>();
            services.AddScoped<IActaManager<ParametroTemplateDto>, ActaManager>();
            #endregion

            #region Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            #endregion
        }
    }
}
