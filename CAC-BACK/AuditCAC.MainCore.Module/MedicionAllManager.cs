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
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class MedicionAllManager : IMedicionAllRepository<MedicionModel>
    {
        private readonly DBAuditCACContext _DBAuditCACContext;
        private readonly ILogger<MedicionModel> _logger;

        public MedicionAllManager(DBAuditCACContext _DBAuditCACContext, ILogger<MedicionModel> logger)
        {
            this._DBAuditCACContext = _DBAuditCACContext;
            _logger = logger;
        }

        public async Task<List<MedicionModel>> GetMedicionAll(InputsMedicionAllDto MedicionAll)
        {
            try
            {
                string sql = "EXEC [GetMedicionCriterio]" +
                    "@PageNumber, " +
                    "@MaxRows, " +
                    "@Id, " +
                    "@IdCobertura, " +
                    "@IdPeriodo, " +
                    "@Descripcion, " +
                    "@Activo, " +
                    "@CreatedBy,	" +
                    "@CreatedDate, " +
                    "@ModifyBy," +
                    "@ModifyDate";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = MedicionAll.IdPeriodo},
                    new SqlParameter { ParameterName = "@MaxRows", Value = MedicionAll.Descripcion},
                    //
                    new SqlParameter { ParameterName = "@Id", Value = MedicionAll.Id},
                    new SqlParameter { ParameterName = "@IdCobertura", Value = MedicionAll.IdCobertura},
                    new SqlParameter { ParameterName = "@IdPeriodo", Value = MedicionAll.IdPeriodo},
                    new SqlParameter { ParameterName = "@Descripcion", Value = MedicionAll.Descripcion},
                    new SqlParameter { ParameterName = "@Activo", Value = MedicionAll.Activo},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = MedicionAll.CreatedBy},
                    new SqlParameter { ParameterName = "@CreatedDate", Value = MedicionAll.CreatedDate},
                    new SqlParameter { ParameterName = "@ModifyBy", Value = MedicionAll.ModifyBy},
                    new SqlParameter { ParameterName = "@ModifyDate", Value = MedicionAll.ModifyDate},
                };

                var Data = await _DBAuditCACContext.MedicionModel.FromSqlRaw(sql, parms.ToArray()).ToListAsync();
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
