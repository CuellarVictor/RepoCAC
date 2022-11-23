using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface ICoberturasErrorRepository<TEntity>
    {
        /* Procedures. */
        Task<List<CoberturasErrorModel>> GetcacCoberturasError(InputsCoberturasErrorDto inputsCoberturasErrorDto);
    }
}
