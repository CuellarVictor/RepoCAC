using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IVariableXMedicionRepository<TEntity>
    {
        Task<IEnumerable<VariableXMedicionModel>> GetAll();
        Task<VariableXMedicionModel> GetByVariableIdOrMedicionId(InputsGetVariableXMedicionDto inputs);
        Task<VariableXMedicionModel> GetByVariableId(int VariableId);
        Task<VariableXMedicionModel> GetByMedicionId(int MedicionId);
        Task Add(VariableXMedicionModel entity);
        Task Update(VariableXMedicionModel dbEntity, VariableXMedicionModel entity);
        Task Delete(VariableXMedicionModel entity);
    }
}
