using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IProcesosRepository<TEntity>
    {
        Task<IEnumerable<ProcessModel>> GetAll();
        Task<ProcessModel> Get(int id);
        Task Add(ProcessModel entity);
        Task Update(ProcessModel dbEntity, ProcessModel entity);
        Task Delete(ProcessModel entity);
        Task<List<ProcessModel>> GetProcesos(InputsProcesosDto Procesos);
    }
}
