using AuditCAC.Domain;
using AuditCAC.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuditCAC.Bash
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ILoggerManager  loggerManager;

        public static IConfiguration configuration;


        private static void ConfigureServices(IServiceCollection serviceCollection)
        {

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                BashCore core = new BashCore();
                // Create service collection 
                ServiceCollection serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                var pathLog = configuration["PathList:LogBash"];

                try
                {
                   

                    S3Model _s3 = new S3Model()
                    {
                        S3AccessKeyId = configuration["S3:S3AccessKeyId"],
                        S3SecretAccessKey = configuration["S3:S3SecretAccessKey"],
                        S3RegionEndpoint = configuration["S3:S3RegionEndpoint"],
                        S3BucketName = configuration["S3:S3BucketName"],
                        S3StorageClass = configuration["S3:S3StorageClass"],
                        UrlFileS3Uploaded = configuration["S3:UrlFileS3Uploaded"]
                    };
                    core.EscribirArchivoPlano(pathLog, "\n");

                    core.EscribirArchivoPlano(pathLog, "INICIA PROCESO BASH V.1.0.22 " + DateTime.Now.ToString());
                    core.StartBashProcess(pathLog, _s3);
                    core.EscribirArchivoPlano(pathLog, "FINALIZA PROCESO BASH " + DateTime.Now.ToString());

                }
                catch (Exception ex)
                {
                    core.EscribirArchivoPlano(pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    throw;
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
