using DomainLayer.DataAccess.Query;
using DomainLayer.Contact;

namespace web.Infrastructure
{
    public class ContactsQueryManager : QueryManager<IContact>
    {
        public ContactsQueryManager(IDocumentQueryStore<IContact> store)
            : base(store)
        { }
    }
}