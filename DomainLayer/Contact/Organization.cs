using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLayer.Contact
{
    public sealed class Organization : IOrganization
    {
        public Organization()
        {
            _Id = ObjectId.GenerateNewId();
        }

        [BsonId]
        public ObjectId _Id { get; set; }

        public string Id
        {
            get
            {
                return _Id.ToString();
            }
        }

        [BsonRequired]
        public string OrganizationName { get; set; }

        private IAdress _adress;
        public IAdress Adress
        {
            get
            {
                if (_adress == null)
                {
                    _adress = new Adress();
                }
                return _adress;
            }
            set
            {
                _adress = value != null ? new Adress(value) : null;
            }
        }

        public string Remarks { get; set; }
    }
}
