using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using AuditCACConsole;
using System;
using ApiAuditCAC.Core.MongoCollection;
using ApiAuditCAC.Data.DataAccess;
using ApiAuditCAC.Data.MongoDataAccess;
using ApiAuditCAC.Domain.MongoModels;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();


using IHost host = Host.CreateDefaultBuilder(args)    
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((_, services) =>
        services.AddTransient<ITransientOperation, DefaultOperation>()
            .AddScoped<IScopedOperation, DefaultOperation>()
            .AddSingleton<ISingletonOperation, DefaultOperation>()
            .AddSingleton<IMongoCollection, MongoCollection>()
            .AddSingleton<IProcedures, Procedures>()
            .Configure<MongoDatabaseSettings>(config.GetSection(nameof(MongoDatabaseSettings)))
            .AddSingleton<IMongoCollections, MongoCollections>()
            .AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value)            
            .AddTransient<OperationLogger>()       
            
            )
    
    .Build();


ExemplifyScoping(host.Services, "Scope 1", host);

await host.RunAsync();

static void ExemplifyScoping(IServiceProvider services, string scope, IHost host)
{

    MongoCollection test = (MongoCollection)host.Services.GetService(typeof(IMongoCollection));

    var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

    var pathLog = config["Path:PahtLog"];
    test.CreateCollection(pathLog);

    /*using IServiceScope serviceScope = services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    OperationLogger logger = provider.GetRequiredService<OperationLogger>();
    logger.LogOperations($"{scope}-Call 1 .GetRequiredService<OperationLogger>()");

    Console.WriteLine("...");

    logger = provider.GetRequiredService<OperationLogger>();
    logger.LogOperations($"{scope}-Call 2 .GetRequiredService<OperationLogger>()");*/

    Console.WriteLine();
}