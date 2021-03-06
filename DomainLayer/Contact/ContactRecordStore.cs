﻿using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Record;
using DomainLayer.Identity;

namespace DomainLayer.Contact
{
    public sealed class ContactRecordStore : RecordStore<Contact, IAppUser>
    {
        public ContactRecordStore(IContactDbContext context)
            : base(context.Create(), "Contacts")
        { }
    }
}
