using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IVariableRepository<TEntity>
    {
        /* Procedures. */
        Task<List<VariableModel>> GetCacvariable(InputsVariableDto inputsVariableDto);        
    }
}
