namespace DomainLayer.Identity
{
    public sealed class MongoIdentityDbContext
    {
        public static DataAccess.IDbContext Create()
        {
            return new DataAccess.MongoDb.MongoDbContext("IdentityDb");
        }
    }
}