using System.Collections.Generic;
using DomainLayer.DataAccess;

namespace DomainLayer.Contact
{
    public interface IContact : IDocument
    {
        string Shurname { get; set; }

        string Name { get; set; }

        string Patronymic { get; set; }

        EnPosition Position { get; set; }

        //<email adress, note>
        IEnumerable<Email> Emails { get; set; }

        //<phone number, note>
        IEnumerable<Phone> Phones { get; set; }

        //reference to the Organization collection
        string OrganizationId { get; set; }

        string Remarks { get; set; }
    }
}
