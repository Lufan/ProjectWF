using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLayer.Contact
{
    public sealed class Contact : IContact
    {
        public Contact()
        {

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
        public string Shurname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        private IPosition _position;
        public IPosition Position
        {
            get
            {
                if (_position == null)
                {
                    _position = new EnPosition();
                }
                return _position;
            }
            set
            {
                _position = value != null ? new EnPosition(value) : null;
            }
        }

        private IDictionary<string, string> _emails;
        //<email adress, note>
        public IDictionary<string, string> Emails
        {
            get
            {
                if (_emails == null)
                {
                    _emails = new Dictionary<string, string>();
                }
                return _emails;
            }
            set
            {
                _emails = value != null ? new Dictionary<string, string>(value) : null;
            }
        }

        private IDictionary<string, string> _phones;
        //<phone number, note>
        public IDictionary<string, string> Phones
        {
            get
            {
                if (_phones == null)
                {
                    _phones = new Dictionary<string, string>();
                }
                return _phones;
            }
            set
            {
                _phones = value != null ? new Dictionary<string, string>(value) : null;
            }
        }

        //reference to the Organization collection
        [BsonRequired]
        public string OrganizationId { get; set; }

        public string Remarks { get; set; }
    }
}
