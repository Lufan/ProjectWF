using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Record;
using DomainLayer.Identity;

namespace DomainLayer.Contact
{
    public sealed class OrganizationRecordStore : RecordStore<IOrganization, IAppUser>
    {
        public OrganizationRecordStore(IContactDbContext context)
            : base(context.Create(), "Organizations")
        { }
    }
}
