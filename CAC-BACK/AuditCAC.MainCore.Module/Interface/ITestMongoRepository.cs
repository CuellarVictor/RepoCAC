using AuditCAC.Domain.MongoEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface ITestMongoRepository
    {
        Task<List<TestMongoModel>> GetAll();
        Task<TestMongoModel> Get(string Id);
        Task Add(TestMongoModel entity);
        Task Update(TestMongoModel dbEntity);
        Task Delete(string Id);
    }
}
