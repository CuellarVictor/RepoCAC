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
    public class ReglaManager : IReglaRepository<ReglaModel>
    {
        private readonly DBCACContext dBCACContext;
        private readonly ILogger<ReglaModel> _logger;

        public ReglaManager(DBCACContext dBCACContext, ILogger<ReglaModel> logger)
        {
            this.dBCACContext = dBCACContext;
            _logger = logger;
        }

        public async Task<List<ReglaModel>> GetcacRegla(InputsReglaDto inputsReglaDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetcacRegla] @PageNumber, @MaxRows, @idRegla, @idCobertura, @nombre, @idTipoRegla, @idTiempoAplicacion, @habilitado, @idError, @idVariable, @idTipoEnvioLimbo";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsReglaDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsReglaDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@idRegla", Value = inputsReglaDto.idRegla},
                    new SqlParameter { ParameterName = "@idCobertura", Value = inputsReglaDto.idCobertura},
                    new SqlParameter { ParameterName = "@nombre", Value = inputsReglaDto.nombre},
                    new SqlParameter { ParameterName = "@idTipoRegla", Value = inputsReglaDto.idTipoRegla},
                    new SqlParameter { ParameterName = "@idTiempoAplicacion", Value = inputsReglaDto.idTiempoAplicacion},
                    new SqlParameter { ParameterName = "@habilitado", Value = inputsReglaDto.habilitado},
                    new SqlParameter { ParameterName = "@idError", Value = inputsReglaDto.idError},
                    new SqlParameter { ParameterName = "@idVariable", Value = inputsReglaDto.idVariable},
                    new SqlParameter { ParameterName = "@idTipoEnvioLimbo", Value = inputsReglaDto.idTipoEnvioLimbo},

                };

                var Data = await dBCACContext.ReglaModel.FromSqlRaw<ReglaModel>(sql, parms.ToArray()).ToListAsync();
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
