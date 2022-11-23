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
    public class RadicadoManager : IRadicadoRepository<RadicadoModel>
    {
        private readonly DBCACContext dBCACContext;
        private readonly ILogger<RadicadoModel> _logger;

        public RadicadoManager(DBCACContext dBCACContext, ILogger<RadicadoModel> logger)
        {
            this.dBCACContext = dBCACContext;
            _logger = logger;
        }

        public async Task<List<RadicadoModel>> GetcacRadicado(InputsRadicadoDto inputsRadicadoDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetcacRadicado] @PageNumber, @MaxRows, @idRadicado, @idArchivoRecibido, @fechaRadicadoIni, @fechaRadicadoFin, @reemplazado, @fechaReemplazadoIni, @fechaReemplazadoFin, @observaciones, @idRadicadoReemplazado";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsRadicadoDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsRadicadoDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@idRadicado", Value = inputsRadicadoDto.idRadicado},
                    new SqlParameter { ParameterName = "@idArchivoRecibido", Value = inputsRadicadoDto.idArchivoRecibido},
                    new SqlParameter { ParameterName = "@fechaRadicadoIni", Value = inputsRadicadoDto.fechaRadicadoIni},
                    new SqlParameter { ParameterName = "@fechaRadicadoFin", Value = inputsRadicadoDto.fechaRadicadoFin},
                    new SqlParameter { ParameterName = "@reemplazado", Value = inputsRadicadoDto.reemplazado},
                    new SqlParameter { ParameterName = "@fechaReemplazadoIni", Value = inputsRadicadoDto.fechaReemplazadoIni},
                    new SqlParameter { ParameterName = "@fechaReemplazadoFin", Value = inputsRadicadoDto.fechaReemplazadoFin},
                    new SqlParameter { ParameterName = "@observaciones", Value = inputsRadicadoDto.observaciones},
                    new SqlParameter { ParameterName = "@idRadicadoReemplazado", Value = inputsRadicadoDto.idRadicadoReemplazado},

                };

                var Data = await dBCACContext.RadicadoModel.FromSqlRaw<RadicadoModel>(sql, parms.ToArray()).ToListAsync();
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
