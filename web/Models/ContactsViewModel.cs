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

        public EnPosition Position { get; set; }

        //<email adress, note>
        public IList<string> Emails { get; set; }

        public IList<string> Phones { get; set; }

        //reference to the Organization collection
        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }

        public string Remarks { get; set; }
    }
}