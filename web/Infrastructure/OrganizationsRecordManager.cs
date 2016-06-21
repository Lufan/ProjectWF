using DomainLayer.Identity;
using DomainLayer.Contact;
using DomainLayer.DataAccess.Record;

namespace web.Infrastructure
{
    public class OrganizationsRecordManager : RecordManager<IOrganization, IAppUser>
    {
        public OrganizationsRecordManager(IDocumentRecordStore<IOrganization, IAppUser> store)
            : base(store)
        { }
    }
}