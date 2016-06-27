using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using DomainLayer.DataAccess;


namespace DomainLayer.Contact
{
    public sealed class Contact : IDocument, IContact
    {
        public Contact()
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

        public string Shurname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        private EnPosition _position;
        public EnPosition Position
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

        private IList<string> _emails;
        //<email adress>
        public IList<string> Emails
        {
            get
            {
                if (_emails == null)
                {
                    _emails = new List<string>();
                }
                return _emails;
            }
            set
            {
                _emails = value != null ? new List<string>(value) : null;
            }
        }

        private IList<string> _phones;
        //<phone number>
        public IList<string> Phones
        {
            get
            {
                if (_phones == null)
                {
                    _phones = new List<string>();
                }
                return _phones;
            }
            set
            {
                _phones = value != null ? new List<string>(value) : null;
            }
        }

        //reference to the Organization collection
        [BsonRequired]
        public string OrganizationId { get; set; }

        public string Remarks { get; set; }
    }
}
