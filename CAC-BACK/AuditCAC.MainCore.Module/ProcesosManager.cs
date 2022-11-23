using AuditCAC.Dal.Data;
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
    public class ProcesosManager : IProcesosRepository<ProcessModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<ProcessModel> _logger;

        public ProcesosManager(DBAuditCACContext dBAuditCACContext, ILogger<ProcessModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos.
        public async Task<IEnumerable<ProcessModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.ProcesosModel.ToListAsync();
                //return await this.dBAuditCACContext.MedicionModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<ProcessModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.ProcesosModel.FirstOrDefaultAsync((System.Linq.Expressions.Expression<Func<ProcessModel, bool>>)(x => x.ProcessId == id));
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        public async Task Add(ProcessModel entity)
        {
            try
            {
                _dBAuditCACContext.ProcesosModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(ProcessModel dbEntity, ProcessModel entity)
        {
            try
            {
                //dbEntity.ProcessId = entity.ProcessId;
                //dbEntity.Status= entity.Status;
                //dbEntity.Name= entity.Name;
                //dbEntity.Class = entity.Class;
                //dbEntity.Method = entity.Method;
                //dbEntity.LifeTime = entity.LifeTime;

                await _dBAuditCACContext.SaveChangesAsync();

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Delete(ProcessModel entity)
        {
            try
            {
                _dBAuditCACContext.ProcesosModel.Remove(entity);
                await _dBAuditCACContext.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<List<ProcessModel>> GetProcesos(InputsProcesosDto ProcesosInput)
        {
            try
            {
                string sql = "EXEC [dbo].[GetMedicionCriterio] " +
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
                     //Create parameters
                    new SqlParameter { ParameterName = "@PageNumber", Value = ProcesosInput.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = ProcesosInput.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@ProccesId", Value = ProcesosInput.ProccesId},
                    new SqlParameter { ParameterName = "@Status", Value = ProcesosInput.Status},
                    new SqlParameter { ParameterName = "@Name", Value = ProcesosInput.Name},
                    new SqlParameter { ParameterName = "@Class", Value = ProcesosInput.Class},
                    new SqlParameter { ParameterName = "@Method", Value = ProcesosInput.Method},
                    new SqlParameter { ParameterName = "@LifeTime", Value = ProcesosInput.LifeTime}
                };
                var Data = await _dBAuditCACContext.ProcesosModel.FromSqlRaw<ProcessModel>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw ex;
            }
        }
    }
}
