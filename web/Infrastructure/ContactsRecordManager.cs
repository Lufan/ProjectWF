using DomainLayer.Identity;
using DomainLayer.Contact;
using DomainLayer.DataAccess.Record;

namespace web.Infrastructure
{
    public class ContactsRecordManager : RecordManager<IContact, IAppUser>
    {
        public ContactsRecordManager(IDocumentRecordStore<IContact, IAppUser> store)
            : base(store)
        { }
    }
}