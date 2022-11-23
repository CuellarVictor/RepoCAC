using AuditCAC.Dal.Data;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class EstadoVariableManager: IEstadoVariableRepository<EstadoVariableModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<EstadoVariableModel> _logger;

        //Constructor
        public EstadoVariableManager(DBAuditCACContext dBAuditCACContext, ILogger<EstadoVariableModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos.
        public async Task<IEnumerable<EstadoVariableModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.EstadoVariableModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.EstadoVariableModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<EstadoVariableModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.EstadoVariableModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Add(EstadoVariableModel entity)
        {
            try
            {
                _dBAuditCACContext.EstadoVariableModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(EstadoVariableModel dbEntity, EstadoVariableModel entity)
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

        public async Task Delete(EstadoVariableModel entity)
        {
            try
            {
                //_dBAuditCACContext.EstadoVariableModel.Remove(entity);
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
