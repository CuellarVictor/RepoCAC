using AuditCAC.Dal.Data;
using AuditCAC.Dal.Entities;
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
    public class NaturalezaVariableManager : INaturalezaVariableRepository<NaturalezaVariableModel>
    {

        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<NaturalezaVariableModel> _logger;

        //Constructor
        public NaturalezaVariableManager(DBAuditCACContext dBAuditCACContext, ILogger<NaturalezaVariableModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos.
        public async Task<IEnumerable<NaturalezaVariableModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.NaturalezaVariableModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.NaturalezaVariableModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<NaturalezaVariableModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.NaturalezaVariableModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        public async Task Add(NaturalezaVariableModel entity)
        {
            try
            {
                _dBAuditCACContext.NaturalezaVariableModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(NaturalezaVariableModel dbEntity, NaturalezaVariableModel entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.Valor = entity.Valor;                
                dbEntity.Descripcion = entity.Descripcion;
                dbEntity.Activo = entity.Activo;
                dbEntity.ModifyBy = entity.ModifyBy;
                dbEntity.ModifyDate = entity.ModifyDate;

                await _dBAuditCACContext.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Delete(NaturalezaVariableModel entity)
        {
            try
            {
                //_dBAuditCACContext.NaturalezaVariableModel.Remove(entity);
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
