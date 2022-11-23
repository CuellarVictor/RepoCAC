using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class ProcedureManager : IProceduresRepository<CoberturaModel>
    {
        
        private readonly DBCACContext dBCACContext;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProcedureManager(DBCACContext dBCACContext) 
        {            
            this.dBCACContext = dBCACContext;
        }
      
        //Obtenemos GetcacCoberturasError.
        public async Task<List<CoberturaModel>> GetcacCoberturas(InputsCoberturaDto inputsCoberturaDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetcacCobertura] @PageNumber, @MaxRows, @idCobertura, @nombre, @nemonico, @legislacion, @definicion, @tiempoEstimado, @ExcluirNovedades, @Novedades, @idResolutionSiame, @NovedadesCompartidosUnicos, @CantidadVariables, @idCoberturaPadre, @idCoberturaAdicionales";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsCoberturaDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsCoberturaDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@idCobertura", Value = inputsCoberturaDto.idCobertura},
                    new SqlParameter { ParameterName = "@nombre", Value = inputsCoberturaDto.nombre},
                    new SqlParameter { ParameterName = "@nemonico", Value = inputsCoberturaDto.nemonico},
                    new SqlParameter { ParameterName = "@legislacion", Value = inputsCoberturaDto.legislacion},
                    new SqlParameter { ParameterName = "@definicion", Value = inputsCoberturaDto.definicion},
                    new SqlParameter { ParameterName = "@tiempoEstimado", Value = inputsCoberturaDto.tiempoEstimado},
                    new SqlParameter { ParameterName = "@ExcluirNovedades", Value = inputsCoberturaDto.ExcluirNovedades},
                    new SqlParameter { ParameterName = "@Novedades", Value = inputsCoberturaDto.Novedades},
                    new SqlParameter { ParameterName = "@idResolutionSiame", Value = inputsCoberturaDto.idResolutionSiame},
                    new SqlParameter { ParameterName = "@NovedadesCompartidosUnicos", Value = inputsCoberturaDto.NovedadesCompartidosUnicos},
                    new SqlParameter { ParameterName = "@CantidadVariables", Value = inputsCoberturaDto.CantidadVariables},
                    new SqlParameter { ParameterName = "@idCoberturaPadre", Value = inputsCoberturaDto.idCoberturaPadre},
                    new SqlParameter { ParameterName = "@idCoberturaAdicionales", Value = inputsCoberturaDto.idCoberturaAdicionales}

                };

                var Data = await dBCACContext.CoberturaModel.FromSqlRaw<CoberturaModel>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                //var attempsCheckState = 1;
                //attempsCheckState++;
                _log.Info(ex);
                _log.Error(ex);
                throw;
            }
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
                _log.Info(ex);
                _log.Error(ex);
                throw;
            }
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
                _log.Info(ex);
                _log.Error(ex);
                throw;
            }
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
                _log.Info(ex);
                _log.Error(ex);
                throw;
            }
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
                _log.Info(ex);
                _log.Error(ex);
                throw;
            }
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
                _log.Info(ex);
                _log.Error(ex);
                throw;
            }
        }

        public async Task<List<VariableModel>> GetCacvariable(InputsVariableDto inputsVariableDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetCacvariable] @PageNumber, @MaxRows, @idVariable, @idCobertura, @nombre, @nemonico, @descripcion, @idTipoVariable, @longitud, @decimales, @formato, @idErrorTipo, @tablaReferencial, @campoReferencial, @idErrorReferencial, @idTipoVariableAlterno, @formatoAlterno, @permiteVacio, @idErrorPermiteVacio, @identificadorRegistro, @clavePrimaria, @idTipoAnalisisEpidemiologico, @sistema, @exportable, @enmascarado";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsVariableDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsVariableDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@idVariable", Value = inputsVariableDto.idVariable},
                    new SqlParameter { ParameterName = "@idCobertura", Value = inputsVariableDto.idCobertura},
                    new SqlParameter { ParameterName = "@nombre", Value = inputsVariableDto.nombre},
                    new SqlParameter { ParameterName = "@nemonico", Value = inputsVariableDto.nemonico},
                    new SqlParameter { ParameterName = "@descripcion", Value = inputsVariableDto.descripcion},
                    new SqlParameter { ParameterName = "@idTipoVariable", Value = inputsVariableDto.idTipoVariable},
                    new SqlParameter { ParameterName = "@longitud", Value = inputsVariableDto.longitud},
                    new SqlParameter { ParameterName = "@decimales", Value = inputsVariableDto.decimales},
                    new SqlParameter { ParameterName = "@formato", Value = inputsVariableDto.formato},
                    new SqlParameter { ParameterName = "@idErrorTipo", Value = inputsVariableDto.idErrorTipo},
                    new SqlParameter { ParameterName = "@tablaReferencial", Value = inputsVariableDto.tablaReferencial},
                    new SqlParameter { ParameterName = "@campoReferencial", Value = inputsVariableDto.campoReferencial},
                    new SqlParameter { ParameterName = "@idErrorReferencial", Value = inputsVariableDto.idErrorReferencial},
                    new SqlParameter { ParameterName = "@idTipoVariableAlterno", Value = inputsVariableDto.idTipoVariableAlterno},
                    new SqlParameter { ParameterName = "@formatoAlterno", Value = inputsVariableDto.formatoAlterno},
                    new SqlParameter { ParameterName = "@permiteVacio", Value = inputsVariableDto.permiteVacio},
                    new SqlParameter { ParameterName = "@idErrorPermiteVacio", Value = inputsVariableDto.idErrorPermiteVacio},
                    new SqlParameter { ParameterName = "@identificadorRegistro", Value = inputsVariableDto.identificadorRegistro},
                    new SqlParameter { ParameterName = "@clavePrimaria", Value = inputsVariableDto.clavePrimaria},
                    new SqlParameter { ParameterName = "@idTipoAnalisisEpidemiologico", Value = inputsVariableDto.idTipoAnalisisEpidemiologico},
                    new SqlParameter { ParameterName = "@sistema", Value = inputsVariableDto.sistema},
                    new SqlParameter { ParameterName = "@exportable", Value = inputsVariableDto.exportable},
                    new SqlParameter { ParameterName = "@enmascarado", Value = inputsVariableDto.enmascarado},
                };

                var Data = await dBCACContext.VariableModel.FromSqlRaw<VariableModel>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _log.Info(ex);
                _log.Error(ex);
                throw;
            }
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
                _log.Info(ex);
                _log.Error(ex);
                throw;
            }
        }
             
    }
}
