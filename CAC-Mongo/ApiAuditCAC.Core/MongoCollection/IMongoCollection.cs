using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Core.MongoCollection
{
    public interface IMongoCollection
    {
        public Task<List<Dictionary<String, Object>>> CreateCollection( string pathLog);
    }
}
