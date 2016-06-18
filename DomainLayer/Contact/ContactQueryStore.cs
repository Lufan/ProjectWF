using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Query;

namespace DomainLayer.Contact
{
    public sealed class ContactQueryStore : QueryStore<IContact>
    {
        public ContactQueryStore(IDbContext dbcontext)
            : base(dbcontext, "Contacts")
        {
        }
    }
}
