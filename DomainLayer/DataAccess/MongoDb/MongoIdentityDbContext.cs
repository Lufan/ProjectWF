using System.Configuration;
using System;

namespace DomainLayer.DataAccess.MongoDb
{
    public sealed class MongoIdentityDbContext : DomainLayer.Identity.IIdentityDbContext
    {
        public DataAccess.IDbContext Create()
        {
            // TO DO: get connectionStringName (or full connection string?) from config file
            string connectionString = ConfigurationManager.ConnectionStrings["IdentityDb"].ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionStringName",
                    "Connection string with name: \"IdentityDb\" is null or empty.");
            }
            return new MongoDbContext(connectionString);
        }
    }
}