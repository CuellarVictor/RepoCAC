using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto.Cobertura;
using AuditCAC.Domain.Entities.Cobertura;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Cobertura
{
    public class CoberturaManager : ICoberturaRepository<CoberturaXUsuarioModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Constructor
        public CoberturaManager(DBAuditCACContext dBAuditCACContext)
        {
            this._dBAuditCACContext = dBAuditCACContext;
        }


        //Metodos
        public async Task<List<CoberturaXUsuarioModel>> GetEnfermedadesMadreXUsuario(InputCoberturaDto objCobertura)
        {
            try
            {
                string sql = "EXEC [dbo].[GETENFERMEDADESMADREXUSUARIO] @idUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    new SqlParameter { ParameterName = "@idUsuario", Value = objCobertura.idUsuario} 
                };

                var Data = await _dBAuditCACContext.CoberturaXUsuarioModel.FromSqlRaw<CoberturaXUsuarioModel>(sql, parms.ToArray()).ToListAsync();
                return Data;

            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }
    }
}
