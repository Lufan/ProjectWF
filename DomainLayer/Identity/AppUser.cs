using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DomainLayer.Identity
{
    public class AppUser : IAppUser
    {

        public AppUser()
        {
            _Id = ObjectId.GenerateNewId();
        }

        public AppUser(string userName)
            : this()
        {
            UserName = userName;
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

        public string UserName { get; set; }

        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }

        //foreign key to the contacts collection (table) with detail contact info
        public string ContactId { get; set; }

        //user roles for restrict access into the app
        private List<AppUserRole> _roles;
        public List<AppUserRole> Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new List<AppUserRole>();
                }
                return _roles;
            }
            set
            {
                _roles = value != null ? new List<AppUserRole>(value) : null;
            }
        }

        //user has access only to the allowed projects
        private List<string> _allowedProjectsNumbers;
        public List<string> AllowedProjectNumbers
        {
            get
            {
                if (_allowedProjectsNumbers == null)
                {
                    _allowedProjectsNumbers = new List<string>();
                }
                return _allowedProjectsNumbers;
            }
            set
            {
                _allowedProjectsNumbers = value != null ? new List<string>(value) : null;
            }
        }
    }
}