using System;
using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLayer.Identity
{
    public class AppUserRole : IRole, DataAccess.IDocument
    {
        public AppUserRole()
        {
            _Id = ObjectId.GenerateNewId();
        }

        public string Id
        {
            get
            {
                return _Id.ToString();
            }
        }

        [BsonId]
        public ObjectId _Id { get; set; }
        public string Name { get; set; }
    }
}