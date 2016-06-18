using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Record;
using DomainLayer.Identity;

namespace DomainLayer.Contact
{
    public sealed class ContactRecordStore : RecordStore<IContact, IAppUser>
    {
        public ContactRecordStore(IDbContext context)
            : base(context, "Contacts")
        { }
    }
}
