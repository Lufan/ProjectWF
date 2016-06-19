using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Query;

namespace DomainLayer.Contact
{
    public sealed class OrganizationQueryStore : QueryStore<IOrganization>
    {
        public OrganizationQueryStore(IContactDbContext dbcontext)
            : base(dbcontext.Create(), "Organizations")
        { }
    }
}
