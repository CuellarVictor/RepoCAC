using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class VariablesPeriodoManager : IVariablesPeriodoRepository<VariablesPeriodoModel>
    {
        private readonly DBCACContext dBCACContext;
        private readonly ILogger<VariablesPeriodoModel> _logger;

        public VariablesPeriodoManager(DBCACContext dBCACContext, ILogger<VariablesPeriodoModel> logger)
        {
            this.dBCACContext = dBCACContext;
            _logger = logger;
        }

        public async Task<List<VariablesPeriodoModel>> GetcacVariablesPeriodo(InputsVariablesPeriodoDto variablesPeriodoDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetcacVariablesPeriodo] @PageNumber, @MaxRows, @idPeriodo, @idVariable, @orden, @variable_num_segun_resoucion";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = variablesPeriodoDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = variablesPeriodoDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@idPeriodo", Value = variablesPeriodoDto.idPeriodo},
                    new SqlParameter { ParameterName = "@idVariable", Value = variablesPeriodoDto.idVariable},
                    new SqlParameter { ParameterName = "@orden", Value = variablesPeriodoDto.orden},
                    new SqlParameter { ParameterName = "@variable_num_segun_resoucion", Value = variablesPeriodoDto.variable_num_segun_resoucion}

                };

                var Data = await dBCACContext.VariablesPeriodoModel.FromSqlRaw<VariablesPeriodoModel>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
