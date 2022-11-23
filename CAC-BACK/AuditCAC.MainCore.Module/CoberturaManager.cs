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
    public class CoberturaManager : ICoberturasRepository<CoberturaModel>
    {
        private readonly DBCACContext dBCACContext;
        private readonly ILogger<CoberturaModel> _logger;

        public CoberturaManager(DBCACContext dBCACContext, ILogger<CoberturaModel> logger)
        {
            this.dBCACContext = dBCACContext;
            _logger = logger;
        }

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
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
