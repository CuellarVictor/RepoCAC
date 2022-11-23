using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Dal.Data
{
    public class DBMongoContext
    {
        public MongoClient MDBclient;

        public IMongoDatabase db;

        public DBMongoContext()
        {
            MDBclient = new MongoClient("mongodb://127.0.0.1:27017"); //Conection String here.

            //Instanciamos la BD si existe. SI no existe, Creamos la BD.
            db = MDBclient.GetDatabase("TestMongoDB");
        }
    }
}
