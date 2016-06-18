
namespace DomainLayer.Contact
{
    public sealed class MongoContactDbContext : DataAccess.MongoDb.MongoDbContext
    {
        public MongoContactDbContext()
            : base("ContactDb")
        { }

        public static DomainLayer.DataAccess.IDbContext Create()
        {
            return new DomainLayer.DataAccess.MongoDb.MongoDbContext("ContactDb");
        }
    }
}
