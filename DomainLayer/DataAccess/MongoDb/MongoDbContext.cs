using System;
using MongoDB.Driver;
using System.Configuration;

namespace DomainLayer.DataAccess.MongoDb
{
    public class MongoDbContext : IDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentNullException("connectionString",
                    "Connection string is null or empty.");
            }
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionStringName",
                    "Connection string with name: " + connectionStringName + " is null or empty.");
            }
            // TO DO try catch MongoDb errors
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            if (url.DatabaseName == null)
            {
                throw new ArgumentNullException("connectionString",
                    "Connection string :" + connectionString + " have not database name.");
            }
            else
            {
                var database = client.GetDatabase(url.DatabaseName);
                if (database == null)
                {
                    throw new ArgumentNullException("database", "Can not connect to: " + url.DatabaseName + ".");
                }
                _database = database;
            }
        }

        public IDatabase GetDatabase()
        {
            return new _mongoDatabase(_database);
        }

        public void Dispose()
        {
            //do nothing
        }

        private class _mongoDatabase : IDatabase
        {
            private readonly IMongoDatabase _database;

            public _mongoDatabase(IMongoDatabase database)
            {
                _database = database;
            }


            IDataTable<TDocument> IDatabase.GetCollection<TDocument>(string collectionName)
            {
                return new MongoDataTable<TDocument>(_database.GetCollection<TDocument>(collectionName));
            }
        }
    }
}