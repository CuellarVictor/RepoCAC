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
    public class ErrorManager : IErrorRepository<ErrorModel>
    {
        private readonly DBCACContext dBCACContext;
        private readonly ILogger<ErrorModel> _logger;

        public ErrorManager(DBCACContext dBCACContext, ILogger<ErrorModel> logger)
        {
            this.dBCACContext = dBCACContext;
            _logger = logger;
        }

        public async Task<List<ErrorModel>> GetcacError(InputsErrorDto errorDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetcacError] @PageNumber, @MaxRows, @idError, @descripcion, @idTipoError";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = errorDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = errorDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@idError", Value = errorDto.idError},
                    new SqlParameter { ParameterName = "@descripcion", Value = errorDto.descripcion},
                    new SqlParameter { ParameterName = "@idTipoError", Value = errorDto.idTipoError}

                };

                var Data = await dBCACContext.ErrorModel.FromSqlRaw<ErrorModel>(sql, parms.ToArray()).ToListAsync();
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
