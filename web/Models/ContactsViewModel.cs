using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainLayer.Contact;

namespace web.Models
{
    public class ContactsViewModel
    {
        public string Id { get; set; }

        public string Shurname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public IPosition Position { get; set; }

        //<email adress, note>
        public IDictionary<string, string> Emails { get; set; }

        public IDictionary<string, string> Phones { get; set; }

        //reference to the Organization collection
        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }

        public string Remarks { get; set; }
    }
}