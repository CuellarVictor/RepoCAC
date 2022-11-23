using ApiAuditCAC.Domain.Dto;
using ApiAuditCAC.Domain.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Data.MongoDataAccess
{
    public interface IMongoCollections
    {
        //public List<Object> Get();
        //public Object Get(string id, String collectionName);
        //public Object Insert(Object collection, String collectionName);
        //public void Update(Object id, Object registro, String collectionName);
        //public void Remove(Object id, String collectionName);

        void AddOrUpdateMassive(List<RegistroAuditoriaMongo> collection, String collectionName);
    }
}
