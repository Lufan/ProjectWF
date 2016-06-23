using DomainLayer.Identity;
using DomainLayer.Contact;
using DomainLayer.DataAccess.Record;

namespace web.Infrastructure
{
    public class ContactsRecordManager : RecordManager<Contact, IAppUser>
    {
        public ContactsRecordManager(IDocumentRecordStore<Contact, IAppUser> store)
            : base(store)
        { }
    }
}