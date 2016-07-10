using System.Collections.Generic;

namespace DomainLayer.Contact
{
    public interface IAdress
    {
        string Country { get; set; }

        string District { get; set; }

        string Region { get; set; }

        string City { get; set; }

        string Index { get; set; }

        IList<string> AdressLines { get; set; }

        //<email adress, note>
        IEnumerable<Email> Emails { get; set; }

        //<phone number, note>
        IEnumerable<Phone> Phones { get; set; }
    }
}
