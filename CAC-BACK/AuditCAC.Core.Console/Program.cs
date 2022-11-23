using AuditCAC.Dal.Data;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.Core.Console
{
    using AuditCAC.Core;
    using AuditCAC.Core.Core;

    class Program 
    {
        
        static void Main(string[] args)
        {
            try
            {
                ConsoleCore core = new ConsoleCore();
                core.StartBashProcess();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public async Task<IEnumerable<ProcesosModel>> GetPercentageProcess()
        //{
        //    try
        //    {
        //        return await _dBAuditCACContext.ProcesosModel.ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
