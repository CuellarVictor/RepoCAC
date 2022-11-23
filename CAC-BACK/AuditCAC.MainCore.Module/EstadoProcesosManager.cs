using AuditCAC.Dal.Data;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class EstadoProcesosManager : IEstadoProceso<ProcessModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<ProcessModel> _logger;
        public EstadoProcesosManager(DBAuditCACContext dBAuditCACContext, ILogger<ProcessModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        public async Task<IEnumerable<ProcessModel>> GetPercentageProcess()
        {
            try
            {
                string sql = "EXEC [dbo].[GetPercentageProcess] ";
                    //"@PageNumber" +
                    //"@MaxRows"+
                    //"@ProccesId"+
                    //"@Status"+
                    //"@Name"+
                    //"@Class"+
                    //"@Method"+
                    //"@LifeTime";

        List<SqlParameter> parms = new List<SqlParameter>
                {
                    //new SqlParameter { ParameterName = "@PageNumber", Value = InputsProcesosDto.PageNumber},
                    //new SqlParameter { ParameterName = "@MaxRows", Value = InputsProcesosDto.MaxRows},  
                    //new SqlParameter { ParameterName = "@ProccesId", Value = InputsProcesosDto.ProccesId},  
                    //new SqlParameter { ParameterName = "@Status", Value = InputsProcesosDto.Status},  
                    //new SqlParameter { ParameterName = "@Name", Value = InputsProcesosDto.Name},  
                    //new SqlParameter { ParameterName = "@Class", Value = InputsProcesosDto.Class},  
                    //new SqlParameter { ParameterName = "@Method", Value = InputsProcesosDto.Method},  
                    //new SqlParameter { ParameterName = "@LifeTime", Value = InputsProcesosDto.LifeTime},
                };
                var Data = await _dBAuditCACContext.ProcesosModel.FromSqlRaw(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw ex;
            }
        }
    }
}
