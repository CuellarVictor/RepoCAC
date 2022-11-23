using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System.Collections.Generic;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IDataRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(long id);
        void Add(TEntity entity);
        void Update(TEntity dbEntity, TEntity entity);
        void Delete(TEntity entity);
    }
}
