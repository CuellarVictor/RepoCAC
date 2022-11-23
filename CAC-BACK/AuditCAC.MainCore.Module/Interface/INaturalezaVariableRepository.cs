using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface INaturalezaVariableRepository<TEntity>
    {
        Task<IEnumerable<NaturalezaVariableModel>> GetAll();
        Task<NaturalezaVariableModel> Get(int id);
        Task Add(NaturalezaVariableModel entity);        
        Task Update(NaturalezaVariableModel dbEntity, NaturalezaVariableModel entity);
        Task Delete(NaturalezaVariableModel entity);
    }
}
