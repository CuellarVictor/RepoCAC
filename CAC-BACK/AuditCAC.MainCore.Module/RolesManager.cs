using AuditCAC.Dal.Data;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AuditCAC.Domain.Dto;
//using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AuditCAC.Domain.Dto;
//using Microsoft.Extensions.Configuration;
//using System.IO;


namespace AuditCAC.MainCore.Module
{
    public class RolesManager : IRolesRepository<RolesModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<RolesModel> _logger;                

        // Constructor.
        public RolesManager(DBAuditCACContext dBAuditCACContext, ILogger<RolesModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        // Metodos.

        /// <summary>
        /// Consulta todos los registros
        /// </summary>
        /// <returns>Todos los registros almacenados</returns>
        public async Task<IEnumerable<RolesModelDto>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.RolesModelDto.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta un registro filtrados por Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Registro consultado</returns>
        public async Task<RolesModelDto> GetById(string id)
        {
            try
            {
                return await _dBAuditCACContext.RolesModelDto.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para crear un nuevo registro.
        /// </summary>
        /// <param name="entity">Registro nuevo</param>
        /// <returns>Registro creado</returns>
        public async Task Add(RolesModelDto entity)
        {
            try
            {
                _dBAuditCACContext.RolesModelDto.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para actualizar un nuevo registro.
        /// </summary>
        /// <param name="dbEntity">Registro anterior</param>
        /// <param name="entity">Registro nuevo</param>
        /// <returns>Registro creado</returns>
        public async Task Update(RolesModelDto dbEntity, RolesModelDto entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.Name = entity.Name;
                dbEntity.NormalizedName = entity.NormalizedName;
                dbEntity.ConcurrencyStamp = entity.ConcurrencyStamp;
                dbEntity.EsLider = entity.EsLider;

                await _dBAuditCACContext.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Eliminar un registro filtrados por Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Registro eliminado</returns>
        public async Task Delete(RolesModelDto entity)
        {
            try
            {
                _dBAuditCACContext.RolesModelDto.Remove(entity);
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
