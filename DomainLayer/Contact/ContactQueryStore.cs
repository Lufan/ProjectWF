﻿using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Query;

namespace DomainLayer.Contact
{
    public sealed class ContactQueryStore : QueryStore<Contact>
    {
        public ContactQueryStore(IContactDbContext dbcontext)
            : base(dbcontext.Create(), "Contacts")
        {
        }
    }
}
