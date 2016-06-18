using System.Collections.Generic;

namespace DomainLayer.Contact
{
    public interface IAdress
    {
        string Country { get; set; }

        string District { get; set; }

        string City { get; set; }

        string Index { get; set; }

        IDictionary<string, string> AdressLines { get; set; }

        IDictionary<string, string> Phones { get; set; }
    }
}
