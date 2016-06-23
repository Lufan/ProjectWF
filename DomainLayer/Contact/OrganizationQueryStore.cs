using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Query;

namespace DomainLayer.Contact
{
    public sealed class OrganizationQueryStore : QueryStore<Organization>
    {
        public OrganizationQueryStore(IContactDbContext dbcontext)
            : base(dbcontext.Create(), "Organizations")
        { }
    }
}
