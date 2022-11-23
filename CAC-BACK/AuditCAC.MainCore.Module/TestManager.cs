using AuditCAC.Dal.Data;
using AuditCAC.Dal.Entities;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AuditCAC.MainCore.Module
{
    public class TestManager : IDataRepository<Test>
    {

        readonly DBAuditCACContext testContext;
        readonly DBCACContext testContextCAC;

        public TestManager(DBAuditCACContext context, DBCACContext dBCACContext) 
        {
            this.testContext = context;
            this.testContextCAC = dBCACContext;
        }
        public void Add(Test entity)
        {
            testContext.Test.Add(entity);
            testContext.SaveChanges(); 
        }

        public void Delete(Test entity)
        {
            testContext.Test.Remove(entity);
            testContext.SaveChanges();
        }

        public Test Get(long id)
        {
            return testContext.Test
                  .FirstOrDefault(e => e.ID == id); 
        }

        public IEnumerable<Test> GetAll()
        {
            return this.testContext.Test.ToList();
        }

        public void Update(Test dbEntity, Test entity)
        {
            dbEntity.Name = entity.Name;
            testContext.SaveChanges();
        }
    }
}
