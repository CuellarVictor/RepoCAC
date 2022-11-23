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
    public class VariableXMedicionManager : IVariableXMedicionRepository<VariableXMedicionModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<VariableXMedicionModel> _logger;

        //Constructor
        public VariableXMedicionManager(DBAuditCACContext dBAuditCACContext, ILogger<VariableXMedicionModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos.
        public async Task<IEnumerable<VariableXMedicionModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.VariableXMedicionModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.VariableXMedicionModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<VariableXMedicionModel> GetByVariableIdOrMedicionId(InputsGetVariableXMedicionDto inputs)
        //public async Task<List<VariableXMedicionModel>> GetByVariableIdOrMedicionId(InputsGetVariableXMedicionDto inputs)
        {
            try
            {
                //return await _dBAuditCACContext.VariableXMedicionModel.FirstOrDefaultAsync(x => x.VariableId == VariableId);
                string sql = "EXEC [dbo].[DuplicarvariablesMedicion] @VariableId, @MedicionId";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@VariableId", Value = inputs.VariableId},
                    new SqlParameter { ParameterName = "@MedicionId", Value = inputs.MedicionId},

                };

                var Data = await _dBAuditCACContext.VariableXMedicionModel.FromSqlRaw<VariableXMedicionModel>(sql, parms.ToArray()).FirstOrDefaultAsync();

                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<VariableXMedicionModel> GetByVariableId(int VariableId)
        {
            try
            {
                return await _dBAuditCACContext.VariableXMedicionModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.VariableId == VariableId);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<VariableXMedicionModel> GetByMedicionId(int MedicionId)
        {
            try
            {
                return await _dBAuditCACContext.VariableXMedicionModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.MedicionId == MedicionId);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Add(VariableXMedicionModel entity)
        {
            try
            {
                _dBAuditCACContext.VariableXMedicionModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(VariableXMedicionModel dbEntity, VariableXMedicionModel entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.VariableId = entity.VariableId;
                dbEntity.MedicionId = entity.MedicionId;
                dbEntity.EsGlosa = entity.EsGlosa;
                dbEntity.EsVisible = entity.EsVisible;
                dbEntity.EsCalificable = entity.EsCalificable;
                dbEntity.Activo = entity.Activo;
                //dbEntity.CreatedBy = entity.CreatedBy;
                //dbEntity.CreationDate = entity.CreationDate;
                dbEntity.ModifyBy = entity.ModifyBy;
                dbEntity.ModificationDate = entity.ModificationDate;
                dbEntity.EnableDC = entity.EnableDC;
                dbEntity.EnableNC = entity.EnableNC;
                dbEntity.EnableND = entity.EnableND;
                dbEntity.CalificacionXDefecto = entity.CalificacionXDefecto;
                dbEntity.SubGrupoId = entity.SubGrupoId;
                dbEntity.Encuesta = entity.Encuesta;                
                dbEntity.Orden = entity.Orden;

                await _dBAuditCACContext.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Delete(VariableXMedicionModel entity)
        {
            try
            {
                //_dBAuditCACContext.VariableXMedicionModel.Remove(entity);
                //await _dBAuditCACContext.SaveChangesAsync();

                entity.Status = false;

                await _dBAuditCACContext.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }
    }
}
