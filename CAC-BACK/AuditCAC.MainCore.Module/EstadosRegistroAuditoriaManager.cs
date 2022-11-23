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
    public class EstadosRegistroAuditoriaManager: IEstadosRegistroAuditoriaRepository<EstadosRegistroAuditoriaModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<MedicionManager> _logger;

        //Constructor
        public EstadosRegistroAuditoriaManager(DBAuditCACContext dBAuditCACContext, ILogger<MedicionManager> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos.
        public async Task<IEnumerable<EstadosRegistroAuditoriaModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.EstadosRegistroAuditoria.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.EstadosRegistroAuditoriaModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<EstadosRegistroAuditoriaModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.EstadosRegistroAuditoria.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //public async Task Add(EstadosRegistroAuditoriaModel entity)
        //{
        //    try
        //    {
        //        _dBAuditCACContext.EstadosRegistroAuditoria.Add(entity);
        //        await _dBAuditCACContext.SaveChangesAsync();
        //        //return NoContent;
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw;
        //    }
        //}

        //public async Task Update(EstadosRegistroAuditoriaModel dbEntity, EstadosRegistroAuditoriaModel entity)
        //{
        //    try
        //    {
        //        //dbEntity.Id = entity.Id;
        //        dbEntity.Codigo = entity.Codigo;
        //        dbEntity.Nombre = entity.Nombre;
        //        dbEntity.Descripción = entity.Descripción;

        //        await _dBAuditCACContext.SaveChangesAsync();
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw;
        //    }
        //}

        //public async Task Delete(EstadosRegistroAuditoriaModel entity)
        //{
        //    try
        //    {
        //        _dBAuditCACContext.EstadosRegistroAuditoria.Remove(entity);
        //        await _dBAuditCACContext.SaveChangesAsync();
        //    }
        //    catch (Exception Ex)
        //    {

        //        throw;
        //    }
        //}
    }
}
