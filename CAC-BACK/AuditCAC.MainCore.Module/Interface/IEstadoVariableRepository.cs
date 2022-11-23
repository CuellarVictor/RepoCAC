using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IEstadoVariableRepository<TEntity>
    {
        Task<IEnumerable<EstadoVariableModel>> GetAll();
        Task<EstadoVariableModel> Get(int id);
        Task Add(EstadoVariableModel entity);        
        Task Update(EstadoVariableModel dbEntity, EstadoVariableModel entity);
        Task Delete(EstadoVariableModel entity);
    }
}
