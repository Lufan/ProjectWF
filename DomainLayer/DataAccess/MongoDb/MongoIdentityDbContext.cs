namespace DomainLayer.DataAccess.MongoDb
{
    public sealed class MongoIdentityDbContext : DomainLayer.Identity.IIdentityDbContext
    {
        public DataAccess.IDbContext Create()
        {
            // TO DO: get db name from config file
            return new MongoDbContext("IdentityDb");
        }
    }
}