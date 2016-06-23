using DomainLayer.Identity;
using DomainLayer.Contact;
using DomainLayer.DataAccess.Record;

namespace web.Infrastructure
{
    public class OrganizationsRecordManager : RecordManager<Organization, IAppUser>
    {
        public OrganizationsRecordManager(IDocumentRecordStore<Organization, IAppUser> store)
            : base(store)
        { }
    }
}