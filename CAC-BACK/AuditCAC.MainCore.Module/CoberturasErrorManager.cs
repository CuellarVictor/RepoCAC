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
    public class CoberturasErrorManager : ICoberturasErrorRepository<CoberturasErrorModel>
    {
        private readonly DBCACContext dBCACContext;
        private readonly ILogger<CoberturasErrorModel> _logger;

        public CoberturasErrorManager(DBCACContext dBCACContext, ILogger<CoberturasErrorModel> logger)
        {
            this.dBCACContext = dBCACContext;
            _logger = logger;
        }

        public async Task<List<CoberturasErrorModel>> GetcacCoberturasError(InputsCoberturasErrorDto inputsCoberturasErrorDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetcacCoberturasError] @PageNumber, @MaxRows, @idError, @idCobertura";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsCoberturasErrorDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsCoberturasErrorDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@idError", Value = inputsCoberturasErrorDto.idError},
                    new SqlParameter { ParameterName = "@idCobertura", Value = inputsCoberturasErrorDto.idCobertura}

                };

                var Data = await dBCACContext.CoberturasErrorModel.FromSqlRaw<CoberturasErrorModel>(sql, parms.ToArray()).ToListAsync();
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
