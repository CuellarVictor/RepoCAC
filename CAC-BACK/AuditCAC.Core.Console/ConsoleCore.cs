using AuditCAC.Core.Core;
using AuditCAC.Dal.Data;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Core.Console
{
    public class ConsoleCore 
    {
        public static IConfiguration configuration;
        public string pathLog;
        public ConsoleCore()
        {
            // Create service collection 
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
       }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging();

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
        public void StartBashProcess()
        {
            DBAuditCACContext _dBAuditCACContext = new DBAuditCACContext();
            DBCACContext _dBCACContext = new DBCACContext();

            S3Model s3 = new S3Model()
            {
                S3AccessKeyId = configuration["S3:S3AccessKeyId"],
                S3SecretAccessKey = configuration["S3:S3SecretAccessKey"],
                S3RegionEndpoint = configuration["S3:S3RegionEndpoint"],
                S3BucketName = configuration["S3:S3BucketName"],
                S3StorageClass = configuration["S3:S3StorageClass"],
                UrlFileS3Uploaded = configuration["S3:UrlFileS3Uploaded"]
            };

            string pathLog = configuration["PathList:LogBash"];


            //string currentdate = (DateTime.Now.ToString("yyyy:MM:dd").Replace(":", "") + ".txt" );

            //pathLog = pathLog + currentdate;


            //if (!File.Exists(pathLog))
            //{
            //    using (var stream = File.Create(pathLog))
            //    {

            //    }
            //}

            ProcesoCore core = new ProcesoCore(_dBAuditCACContext, s3, pathLog);

            core.ExecuteCurrentProcess();

        }
    }
}
