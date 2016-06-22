
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DomainLayer.DataAccess.MongoDb
{
    public sealed class MongoContactDbContext : Contact.IContactDbContext
    {
        public IDbContext Create()
        {
            // TO DO: get connectionStringName (or full connection string?) from config file
            return new MongoDbContext("ContactDb");
        }
    }
}
