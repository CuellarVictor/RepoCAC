using AuditCAC.Domain;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace AuditCAC.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadersController : ControllerBase
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ReadersController(ILoggerManager logger)
        {           

            try
            {
                //cualqueir cosa
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw new Exception("Error", ex);
            }
        }
    }
}
