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
    public class VariableManager : IVariableRepository<VariableModel>
    {
        private readonly DBCACContext dBCACContext;
        private readonly ILogger<VariableModel> _logger;

        public VariableManager(DBCACContext dBCACContext, ILogger<VariableModel> logger)
        {
            this.dBCACContext = dBCACContext;
            _logger = logger;
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
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
