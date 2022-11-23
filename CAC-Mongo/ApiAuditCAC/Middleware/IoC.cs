using ApiAuditCAC.Core.MongoCollection;
using ApiAuditCAC.Data.DataAccess;
using ApiAuditCAC.Data.MongoDataAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAuditCAC.Services.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddScoped<IMongoCollection, MongoCollection>();
            services.AddScoped<IMongoCollections, MongoCollections>();
            services.AddScoped<IProcedures, Procedures>();
            return services;
        }
    }
}
