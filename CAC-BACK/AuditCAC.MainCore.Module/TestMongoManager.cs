using AuditCAC.Dal.Data;
using AuditCAC.Domain.MongoEntities;
using AuditCAC.MainCore.Module.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class TestMongoManager : ITestMongoRepository
    {
        //private readonly DBMongoContext dBMongoContext;

        ////Constructor.
        //public TestMongoManager(DBMongoContext dBMongoContext)
        //{
        //    this.dBMongoContext = dBMongoContext;
        //}

        //Constructor del tutorial
        internal DBMongoContext dBMongoContext = new DBMongoContext();
        private IMongoCollection<TestMongoModel> testMongoCollection;

        public TestMongoManager()
        {
            testMongoCollection = dBMongoContext.db.GetCollection<TestMongoModel>("TestMongo");
        }

        //Metodos
        public async Task<List<TestMongoModel>> GetAll()
        {
            return await testMongoCollection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }
        public async Task<TestMongoModel> Get(string Id)
        {
            return await testMongoCollection.FindAsync(new BsonDocument{ { "_id",  new ObjectId(Id)} } ).Result.FirstAsync(); //FirstOrDefaultAsync
        }
        
        public async Task Add(TestMongoModel entity)
        {
            var Filter = Builders<TestMongoModel>.Filter.Eq(x => x.Id, entity.Id);
            await testMongoCollection.ReplaceOneAsync(Filter, entity);

        }

        public async Task Update(TestMongoModel dbEntity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string Id)
        {
            var Filter = Builders<TestMongoModel>.Filter.Eq(x => x.Id, new ObjectId(Id));
            await testMongoCollection.DeleteOneAsync(Filter);
        }
    }
}
