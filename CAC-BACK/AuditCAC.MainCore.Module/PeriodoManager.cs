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
    public class PeriodoManager : IPeriodoRepository<PeriodoModel>
    {
        private readonly DBCACContext dBCACContext;
        private readonly ILogger<PeriodoModel> _logger;

        public PeriodoManager(DBCACContext dBCACContext, ILogger<PeriodoModel> logger)
        {
            this.dBCACContext = dBCACContext;
            _logger = logger;
        }

        public async Task<List<PeriodoModel>> Getcacperiodo(InputsPeriodoDto inputsPeriodoDto)
        {
            try
            {
                string sql = "EXEC [dbo].[Getcacperiodo] @PageNumber, @MaxRows, @idPeriodo, @nombre, @fechaCorteIni, @fechaCorteFin, @fechaFinalReporteIni, @fechaFinalReporteFin, @fechaMaximaCorreccionesIni, @fechaMaximaCorreccionesFin, @fechaMaximaSoportesIni, @fechaMaximaSoportesFin, @fechaMaximaConciliacionesIni, @fechaMaximaConciliacionesFin, @idCobertura, @idPeriodoAnt, @FechaMinConsultaAuditIni, @FechaMinConsultaAuditFin, @FechaMaxConsultaAuditIni, @FechaMaxConsultaAuditFin";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsPeriodoDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsPeriodoDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@idPeriodo", Value = inputsPeriodoDto.idPeriodo},
                    new SqlParameter { ParameterName = "@nombre", Value = inputsPeriodoDto.nombre},
                    new SqlParameter { ParameterName = "@fechaCorteIni", Value = inputsPeriodoDto.fechaCorteIni},
                    new SqlParameter { ParameterName = "@fechaCorteFin", Value = inputsPeriodoDto.fechaCorteFin},
                    new SqlParameter { ParameterName = "@fechaFinalReporteIni", Value = inputsPeriodoDto.fechaFinalReporteIni},
                    new SqlParameter { ParameterName = "@fechaFinalReporteFin", Value = inputsPeriodoDto.fechaFinalReporteFin},
                    new SqlParameter { ParameterName = "@fechaMaximaCorreccionesIni", Value = inputsPeriodoDto.fechaMaximaCorreccionesIni},
                    new SqlParameter { ParameterName = "@fechaMaximaCorreccionesFin", Value = inputsPeriodoDto.fechaMaximaCorreccionesFin},
                    new SqlParameter { ParameterName = "@fechaMaximaSoportesIni", Value = inputsPeriodoDto.fechaMaximaSoportesIni},
                    new SqlParameter { ParameterName = "@fechaMaximaSoportesFin", Value = inputsPeriodoDto.fechaMaximaSoportesFin},
                    new SqlParameter { ParameterName = "@fechaMaximaConciliacionesIni", Value = inputsPeriodoDto.fechaMaximaConciliacionesIni},
                    new SqlParameter { ParameterName = "@fechaMaximaConciliacionesFin", Value = inputsPeriodoDto.fechaMaximaConciliacionesFin},
                    new SqlParameter { ParameterName = "@idCobertura", Value = inputsPeriodoDto.idCobertura},
                    new SqlParameter { ParameterName = "@idPeriodoAnt", Value = inputsPeriodoDto.idPeriodoAnt},
                    new SqlParameter { ParameterName = "@FechaMinConsultaAuditIni", Value = inputsPeriodoDto.FechaMinConsultaAuditIni},
                    new SqlParameter { ParameterName = "@FechaMinConsultaAuditFin", Value = inputsPeriodoDto.FechaMinConsultaAuditFin},
                    new SqlParameter { ParameterName = "@FechaMaxConsultaAuditIni", Value = inputsPeriodoDto.FechaMaxConsultaAuditIni},
                    new SqlParameter { ParameterName = "@FechaMaxConsultaAuditFin", Value = inputsPeriodoDto.FechaMaxConsultaAuditFin},

                };

                var Data = await dBCACContext.PeriodoModel.FromSqlRaw<PeriodoModel>(sql, parms.ToArray()).ToListAsync();
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
