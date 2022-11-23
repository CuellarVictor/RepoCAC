using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Lider.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AuditCAC.MainCore.Module.Lider
{
    public class LiderManager : ILiderManager
    {
        #region Dependencias
        private readonly DBAuditCACContext _context;
        #endregion

        #region Constructor
        public LiderManager(DBAuditCACContext context)
        {
            _context = context;       
        }
        #endregion

        #region Metodos
        public async Task<string> GetIssuesLider(string id_auditor)
        {
            var query = "EXEC [dbo].GetCountLeaderIssues @id_auditor";

            List<SqlParameter> parametros = new List<SqlParameter> { 
                new SqlParameter {ParameterName="@id_auditor", Value=id_auditor}
            };

            var Data = await _context.LiderIssuesResponse.FromSqlRaw<LiderIssuesResponseModel>(query, parametros.ToArray()).ToListAsync();
                          
            return Data[0].NroIssues.ToString();
        }
        #endregion 
    }
}
