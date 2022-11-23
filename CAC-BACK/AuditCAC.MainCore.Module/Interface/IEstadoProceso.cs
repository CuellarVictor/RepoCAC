using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IEstadoProceso<TEntity>
    {
        Task<IEnumerable<ProcessModel>> GetPercentageProcess();
    }
}
