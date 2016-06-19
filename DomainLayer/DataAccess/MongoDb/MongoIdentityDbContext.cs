﻿
namespace DomainLayer.DataAccess.MongoDb
{
    public sealed class MongoIdentityDbContext : Identity.IIdentityDbContext
    {
        public IDbContext Create()
        {
            // TO DO: get connectionStringName (or full connection string?) from config file
            return new MongoDbContext("IdentityDb");
        }
    }
}