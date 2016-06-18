using DomainLayer.DataAccess.Query;
using DomainLayer.Contact;


namespace web.Infrastructure
{
    public class OrganizationsQueryManager : QueryManager<IOrganization>
    {
        public OrganizationsQueryManager(IDocumentQueryStore<IOrganization> store)
            : base(store)
        { }
    }
}