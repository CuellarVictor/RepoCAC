using AuditCAC.Core.Core;
using AuditCAC.Dal.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConsolaMigracionData
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

                String pathLog = config["PathList:LogConsola"];

                DBAuditCACContext dBAuditCACContext = new();
                DBCACContext dBCACContext = new ();
                ProcesoCore procesoCore = new( pathLog, dBCACContext, dBAuditCACContext);

                procesoCore.MigCACVariables();
                procesoCore.MigCACRegla();
                procesoCore.MigCACPeriodo();
                procesoCore.MigCACRestriccionesConsistencia();
                procesoCore.MigCACError();
                procesoCore.MigCACCobertura();

                var MigrarTablas = await procesoCore.MigrarDataTablasReferenciales();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
