using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AuditCAC.Domain
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(LoggerManager));
        public void LogInformation()
        {
            try
            {
                XmlDocument log4netConfig = new();

                using var fs = File.OpenRead("log4net.config");
                log4netConfig.Load(fs);

                var repo = LogManager.CreateRepository(
                        Assembly.GetEntryAssembly(),
                        typeof(log4net.Repository.Hierarchy.Hierarchy));

                XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

                // The first log to be written 
                _logger.Info("Log System Initialized");
            }
            catch (Exception ex)
            {
                _logger.Error("Error", ex);
            }
        }

        public void LogInformation(string message)
        {
            _logger.Info(message);
        }
    }
}
