using DomainLayer.DataAccess.Query;
using DomainLayer.Contact;

namespace web.Infrastructure
{
    public class ContactsQueryManager : QueryManager<Contact>
    {
        public ContactsQueryManager(IDocumentQueryStore<Contact> store)
            : base(store)
        { }
    }
}