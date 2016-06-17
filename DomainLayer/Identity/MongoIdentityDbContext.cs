namespace DomainLayer.Identity
{
    public sealed class MongoIdentityDbContext
    {
        public static DataAccess.IDbContext Create()
        {
            // TO DO: get db name from config file
            return new DataAccess.MongoDb.MongoDbContext("IdentityDb");
        }
    }
}