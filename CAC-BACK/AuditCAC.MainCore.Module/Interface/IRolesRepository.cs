using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IRolesRepository<TEntity>
    {
        /// <summary>
        /// Consulta todos los registros
        /// </summary>
        /// <returns>Todos los registros almacenados</returns>
        Task<IEnumerable<RolesModel>> GetAll();

        /// <summary>
        /// Consulta un registro filtrados por Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Registro consultado</returns>
        Task<RolesModel> GetById(string id);

        /// <summary>
        /// Para crear un nuevo registro.
        /// </summary>
        /// <param name="entity">Registro nuevo</param>
        /// <returns>Registro creado</returns>
        Task Add(RolesModel entity);

        /// <summary>
        /// Para actualizar un nuevo registro.
        /// </summary>
        /// <param name="dbEntity">Registro anterior</param>
        /// <param name="entity">Registro nuevo</param>
        /// <returns>Registro creado</returns>
        Task Update(RolesModel dbEntity, RolesModel entity);

        /// <summary>
        /// Eliminar un registro filtrados por Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Registro eliminado</returns>
        Task Delete(RolesModel entity);
    }
}
