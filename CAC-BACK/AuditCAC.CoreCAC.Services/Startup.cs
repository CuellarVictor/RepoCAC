using AuditCAC.Dal.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuditCAC.MainCore.Module.Interface;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.IO;
using AuditCAC.Domain.MongoEntities;
using AuditCAC.Domain.Dto;

namespace AuditCAC.CoreCAC.Services
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DBAuditCACContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<DBAuditCACContext>();

            //DB Context para Procedures (Base de datos CAC)
            services.AddDbContext<DBCACContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionCAC")));
            services.AddScoped<DBCACContext>();

            // Dependency Injection           
            services.AddScoped<ICoberturasRepository<CoberturaModel>, CoberturaManager>();
            services.AddScoped<ICoberturasErrorRepository<CoberturasErrorModel>, CoberturasErrorManager>();
            services.AddScoped<IErrorRepository<ErrorModel>, ErrorManager>();
            services.AddScoped<IPeriodoRepository<PeriodoModel>, PeriodoManager>();
            services.AddScoped<IRadicadoRepository<RadicadoModel>, RadicadoManager>();
            services.AddScoped<IReglaRepository<ReglaModel>, ReglaManager>();
            services.AddScoped<IVariableRepository<VariableModel>, VariableManager>();
            services.AddScoped<IVariablesPeriodoRepository<VariablesPeriodoModel>, VariablesPeriodoManager>();
            services.AddScoped<ITransferenciaRegistrosRepository<NemonicoPascientesConsultaModel>, transferenciaRegistrosManager>();
            services.AddScoped<ITablasReferencialRepository<ResponseGetTablasReferencial>, TablasReferencialManager>();
            //services.AddScoped<ITestMongoRepository<TestMongoModel>, TestMongoManager>();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("*")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuditCAC.CoreCAC.Services", Version = "v1" });

                //Agregamos configuración que permite ingresar Token en cada Solucitud a las APIs.
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                //Agregamos configuración que permite ingresar Token en cada Solucitud a las APIs.
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //Agregamos servicios de autenticacion.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                opciones => opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                    ClockSkew = TimeSpan.Zero
                });
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DBAuditCACContext>().AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuditCAC.CoreCAC.Services v1"));

            loggerFactory.AddLog4Net();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);    
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapControllers();
            });           
        }
    }
}
