using DomainLayer.DataAccess.Query;
using DomainLayer.Contact;


namespace web.Infrastructure
{
    public class OrganizationsQueryManager : QueryManager<Organization>
    {
        public OrganizationsQueryManager(IDocumentQueryStore<Organization> store)
            : base(store)
        { }
    }
}