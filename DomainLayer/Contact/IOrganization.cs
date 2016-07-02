using DomainLayer.DataAccess;

namespace DomainLayer.Contact
{
    public interface IOrganization : IDocument
    {
        string OrganizationName { get; set; }

        Adress Adress { get; set; }

        string Remarks { get; set; }
    }
}
