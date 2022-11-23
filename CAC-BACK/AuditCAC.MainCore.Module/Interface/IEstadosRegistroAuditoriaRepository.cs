using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IEstadosRegistroAuditoriaRepository<TEntity>
    {
        Task<IEnumerable<EstadosRegistroAuditoriaModel>> GetAll();
        Task<EstadosRegistroAuditoriaModel> Get(int id);
        //Task Add(EstadosRegistroAuditoriaModel entity);
        //Task Update(EstadosRegistroAuditoriaModel dbEntity, EstadosRegistroAuditoriaModel entity);
        //Task Delete(EstadosRegistroAuditoriaModel entity);
    }
}
