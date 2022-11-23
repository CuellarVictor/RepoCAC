using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.MongoModels
{
    public class RegistroAuditoriaMongo
    {

        public int _id { get; set; }

        public int IdRadicado { get; set; }
        public Dictionary<String, Object> Body { get; set; }        

    }
}
