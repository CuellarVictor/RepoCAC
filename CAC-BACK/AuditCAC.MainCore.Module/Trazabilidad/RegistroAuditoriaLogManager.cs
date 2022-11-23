using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Trazabilidad.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Trazabilidad
{
    public class RegistroAuditoriaLogManager : IRegistroAuditoriaLogManager
    {
        #region Dependency
        private readonly DBAuditCACContext _dBAuditCACContext;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructor
        public RegistroAuditoriaLogManager(DBAuditCACContext dBAuditCACContext)
        {
            _dBAuditCACContext = dBAuditCACContext;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Consuta registros de log auditoria
        /// </summary>
        /// <param name="inputsDto">Modelo parametros de consulta</param>
        /// <returns>Modelo Registros Auditoria Log</returns>
        public async Task<List<RegistrosAuditoriaLogModel>> ConsultaLogAccion(InputsGetRegistrosAuditoriaLogDto inputsDto)
        {
            try {
                List<RegistrosAuditoriaLogModel> oReturn = new List<RegistrosAuditoriaLogModel>();

                if (inputsDto.IdAuditores.Count() > 0)
                {
                    string sql = "EXEC [dbo].[SP_Consulta_Log_Accion] @ParametroBusqueda, @FechaInicial, @FechaFinal, @IdAuditores, @Paginate, @MaxRows, @MedicionId";

                    #region Comentado debido a que generaba error.
                    //string idsConcatenados = "";

                    //inputsDto.IdAuditores?.All(x =>
                    //{
                    //    idsConcatenados += "'" + x + "',";
                    //    return true;
                    //});

                    //idsConcatenados = idsConcatenados.Substring(0, idsConcatenados.Length - 1);
                    #endregion

                    List<SqlParameter> parms = new List<SqlParameter>
                    { 
                        // Create parameters
                        new SqlParameter { ParameterName = "@ParametroBusqueda", Value = inputsDto.ParametroBusqueda},
                        new SqlParameter { ParameterName = "@FechaInicial", Value = inputsDto.FechaInicial},
                        new SqlParameter { ParameterName = "@FechaFinal", Value = inputsDto.FechaFinal},
                        //new SqlParameter { ParameterName = "@IdAuditores", Value = idsConcatenados},
                        new SqlParameter { ParameterName = "@IdAuditores", Value = String.Join("'',''", inputsDto.IdAuditores.ToArray())},
                        new SqlParameter { ParameterName = "@Paginate", Value = inputsDto.PageNumber},
                        new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                        new SqlParameter { ParameterName = "@MedicionId", Value = inputsDto.MedicionId},
                    };

                    oReturn = await _dBAuditCACContext.RegistrosAuditoriaLogModel.FromSqlRaw<RegistrosAuditoriaLogModel>(sql, parms.ToArray()).ToListAsync();

                    if (oReturn.Count() > 0)
                    {
                        int paginas = inputsDto.MaxRows / oReturn[0].CountQuery;
                        oReturn.Select(x => x.Paginas = paginas);
                    }
                }

                return oReturn;


            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }
        }
        #endregion
    }
}
