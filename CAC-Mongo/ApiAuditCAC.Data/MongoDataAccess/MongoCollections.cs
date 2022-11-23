using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiAuditCAC.Domain.Dto;
using ApiAuditCAC.Domain.MongoModels;
using MongoDB.Driver;

namespace ApiAuditCAC.Data.MongoDataAccess
{
    public class MongoCollections : IMongoCollections
    {
        private readonly IMongoDatabase database;
        private IMongoCollection<Object> mongoCollection;
        private IMongoCollection<RegistroAuditoriaMongo> mongoCollection2;

        public MongoCollections(IMongoDatabaseSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
            mongoCollection = database.GetCollection<Object>(settings.CollectionName);
        }

        //public List<Object> Get()
        //{
            
        //    return mongoCollection.Find(colletion => true).ToList();
        //}

        //public Object Get(string id, String collectionName)
        //{
        //    mongoCollection2 = database.GetCollection<RegistrosDto>(collectionName);

        //    List<RegistrosDto> registros = mongoCollection2.Find<RegistrosDto>(colletion => true).ToList();

        //    foreach (RegistrosDto registrosDto in registros)
        //    {
        //        if (((dynamic)registros[0]._v).IdRadicado == int.Parse(id))
        //        {
        //            return registrosDto.id;
        //        }
        //    }

        //    return null;
        //}


        public void AddOrUpdateMassive(List<RegistroAuditoriaMongo> collection, String collectionName)
        {
            mongoCollection2 = database.GetCollection<RegistroAuditoriaMongo>(collectionName);

            List<RegistroAuditoriaMongo> registros = mongoCollection2.Find<RegistroAuditoriaMongo>(colletion => true).ToList();

            List<RegistroAuditoriaMongo> recordInsert = new List<RegistroAuditoriaMongo>();


            collection?.All(x =>
            {
                if (registros.Where(r => r._id == x._id).Any())
                {
                    try
                    {
                        database.GetCollection<RegistroAuditoriaMongo>(collectionName).DeleteOne(reg => reg._id == x._id);
                        mongoCollection2.InsertOne(x);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    

                }
                else
                {
                    recordInsert.Add(x);
                }

                return true;
            });

            mongoCollection2.InsertMany(recordInsert);

        }

        //public Object Insert(Object collection, String collectionName)
        //{
        //    mongoCollection = database.GetCollection<Object>(collectionName);
        //    mongoCollection.InsertOne(collection);

        //    return collection;
        //}

        //public void Update(Object id, Object registro, String collectionName)
        //{
        //    database.GetCollection<RegistrosDto>(collectionName).ReplaceOne(reg => reg.id == id, (RegistrosDto)registro);
        //}

        //public void Remove(Object id, String collectionName) =>
        //    database.GetCollection<RegistrosDto>(collectionName).DeleteOne(reg => reg.id == id);
    }
}
