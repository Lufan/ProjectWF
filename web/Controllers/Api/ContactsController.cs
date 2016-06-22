using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;


using web.Infrastructure;
using DomainLayer.Contact;
using DomainLayer.Identity;
using web.Models;

namespace web.Controllers
{
    [Authorize]
    public class ApiContactsController : ApiController
    {
        public ApiContactsController(
            IQueryManager<Contact> contactQM, 
            IQueryManager<IOrganization> organizationQM,
            IRecordManager<IContact, IAppUser> contactRM
            )
        {
            this.contactQM = contactQM;
            this.organizationQM = organizationQM;
            this.contactRM = contactRM;
        }

        // GET: api/ApiContacts
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            return Ok(GetViewModel());
        }

        // GET: api/ApiContacts/5
        [AllowAnonymous]
        public IHttpActionResult Get(string id)
        {
            try
            {
                var _contact = contactQM.FindByIdAsync(id);
                var contact = _contact.Result;
                if (contact != null)
                {
                    var organizationName = "";
                    // TO DO - select all organization names using only one query to database 
                    if (contact.OrganizationId != null)
                    {
                        organizationName = organizationQM.FindByIdAsync(contact.OrganizationId).Result.OrganizationName;
                    }
                    var result = new ContactsViewModel
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Shurname = contact.Shurname,
                        Patronymic = contact.Patronymic,
                        Emails = contact.Emails,
                        Phones = contact.Phones,
                        OrganizationId = contact.OrganizationId,
                        OrganizationName = organizationName,
                        Position = contact.Position,
                        Remarks = contact.Remarks
                    };
                    return Ok(result);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return NotFound();
        }

        // POST: api/ApiContacts
        public void Post([FromBody]ContactsViewModel contact)
        {
            if (contact != null)
            {
                var ct = contactQM.FindByIdAsync(contact.Id).Result;
                if (ct == null)
                {
                    ct = new Contact
                    {
                        Name = contact.Name,
                        Shurname = contact.Shurname,
                        Patronymic = contact.Patronymic,
                        Emails = contact.Emails,
                        Phones = contact.Phones,
                        OrganizationId = contact.OrganizationId,
                        Position = contact.Position,
                        Remarks = contact.Remarks
                    };
                    var user_query = UserManager.Users.Select(e => e).Where(e => e.Id == User.Identity.GetUserId());
                    if (user_query.Count() == 0) return;
                    contactRM.CreateAsync(ct, user_query.First());
                } else
                {
                    ct = contactQM.FindByIdAsync(contact.Id).Result;
                    if (ct == null) return;
                    ct.Name = contact.Name;
                    ct.Shurname = contact.Shurname;
                    ct.Patronymic = contact.Patronymic;
                    ct.Emails = contact.Emails;
                    ct.Phones = contact.Phones;
                    ct.OrganizationId = contact.OrganizationId;
                    ct.Position = contact.Position;
                    ct.Remarks = contact.Remarks;

                    var user_query = UserManager.Users.Select(e => e).Where(e => e.Id == User.Identity.GetUserId());
                    if (user_query.Count() == 0) return;

                    contactRM.UpdateAsync(ct, user_query.First());
                }
            }
        }

        // PUT: api/ApiContacts/5
        // Only update the contact with id and never create new one.
        public void Put(string id, [FromBody]ContactsViewModel contact)
        {
            if (id == null || contact == null) return;

            var ct = contactQM.FindByIdAsync(id).Result;
            if (ct == null) return;
            ct.Name = contact.Name;
            ct.Shurname = contact.Shurname;
            ct.Patronymic = contact.Patronymic;
            ct.Emails = contact.Emails;
            ct.Phones = contact.Phones;
            ct.OrganizationId = contact.OrganizationId;
            ct.Position = contact.Position;
            ct.Remarks = contact.Remarks;

            var user_query = UserManager.Users.Select(e => e).Where(e => e.Id == User.Identity.GetUserId());
            if (user_query.Count() == 0) return;

            contactRM.UpdateAsync(ct, user_query.First());
        }

        // DELETE: api/ApiContacts/5
        public void Delete(string id)
        {
            if (id == null) return;
            var ct = contactQM.FindByIdAsync(id).Result;
            if (ct == null) return;

            var user_query = UserManager.Users.Select(e => e).Where(e => e.Id == User.Identity.GetUserId());
            if (user_query.Count() == 0) return;

            // TO DO - maybe best to be don't delete the contact and only mark it as deleted?
            contactRM.DeleteAsync(ct, user_query.First());
        }

        private IEnumerable<ContactsViewModel> GetViewModel(int count = 10, int start_pos = 0)
        {
            var contacts = contactQM.TakeNAsync(count, start_pos);
            IList<ContactsViewModel> result = new List<ContactsViewModel>(count);
            foreach (var contact in contacts.Result)
            {
                var organizationName = "";
                // TO DO - select all organization names using only one query to database 
                if (contact.OrganizationId != null)
                {
                    organizationName = organizationQM.FindByIdAsync(contact.OrganizationId).Result.OrganizationName;
                }
                result.Add(
                    new ContactsViewModel
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Shurname = contact.Shurname,
                        Patronymic = contact.Patronymic,
                        Emails = contact.Emails,
                        Phones = contact.Phones,
                        OrganizationId = contact.OrganizationId,
                        OrganizationName = organizationName,
                        Position = contact.Position,
                        Remarks = contact.Remarks
                    });
            }
            return result.AsEnumerable();
        }

        private IQueryManager<Contact> contactQM;
        private IQueryManager<IOrganization> organizationQM;

        private IRecordManager<IContact, IAppUser> contactRM;

        private UserManager<AppUser> UserManager
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<UserManager<AppUser>>();
            }
        }
    }
}
