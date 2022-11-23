using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Dal.Data
{
    public class DBCACContext: IdentityDbContext //DbContext
    {
        public DBCACContext()
        {
        }
        public DBCACContext(DbContextOptions<DBCACContext> options) : base(options)
        {
        }

        public DbSet<CoberturaModel> CoberturaModel { get; set; }
        public DbSet<CoberturasErrorModel> CoberturasErrorModel { get; set; }
        public DbSet<ErrorModel> ErrorModel { get; set; }
        public DbSet<PeriodoModel> PeriodoModel { get; set; }
        public DbSet<RadicadoModel> RadicadoModel { get; set; }
        public DbSet<ReglaModel> ReglaModel { get; set; }
        public DbSet<VariableModel> VariableModel { get; set; }
        public DbSet<VariablesPeriodoModel> VariablesPeriodoModel { get; set; }
        public DbSet<NemonicoPascientesConsultaModel> NemonicoPascientesConsultaModel { get; set; }
        public DbSet<MedicionAllModel> MedicionAllModel { get; set; }
        

        //DTOs para respuestas custom de metodos.
        public DbSet<ResponseGetTablasReferencialDto> ResponseGetTablasReferencialDto { get; set; }
        public DbSet<ResponseConsultarListadoTablasCamposReferencialesDto> ResponseConsultarListadoTablasCamposReferencialesDto { get; set; }
        public DbSet<ResponseConsultarDatosTablasCamposReferencialesDto> ResponseConsultarDatosTablasCamposReferencialesDto { get; set; }
        public DbSet<ResponseSpGetDataMigEnfermedad> ResponseSpGetDataMigEnfermedadDTO { get; set; }
        public DbSet<ResponseSPGetDataMigError> ResponseSPGetDataMigErrorDTO { get; set; }
        public DbSet<ResponseSPGetDataMigRestriccionesConsistencia> ResponseSPGetDataMigRestriccionesConsistenciaDto { get; set; }
        public DbSet<ResponseSPGetDataMigPeriodo> ResponseSPGetDataMigPeriodoDto { get; set; }
        public DbSet<ResponseSPGetDataMigRegla> ResponseSPGetDataMigReglaDto { get; set; }
        public DbSet<ResponseSPGetDataMigVariable> ResponseSPGetDataMigVariableDto { get; set; }
        

        

        //public DbSet<SoportesEntidadModel> SoportesEntidadModel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ResponseConsultarDatosTablasCamposReferencialesDto>(builder => { builder.HasNoKey(); });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(x => x.EnableRetryOnFailure());

                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("ConnectionCAC");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }

}
